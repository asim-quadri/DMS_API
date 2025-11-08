using ComplianceAPI.Models;
using ComplianceAPI.Models.DataModels;
using ComplianceAPI.Models.Enums;
using Microsoft.EntityFrameworkCore;

using Db = ComplianceAPI.Models.DataModels;

using RefApprovalStatus = ComplianceAPI.Models.Enums.RefApprovalStatus;

namespace ComplianceAPI.Repository
{
    public interface IRegulatoryAuthorityRepository
    {
        Task<Result> PostRegulatoryAuthority(PostRegulatoryAuthorities regulatoryAuthorities, int approveStatus);

        Task<string> GetNextRegulatoryAuthRefCode();

        Task<List<PostRegulatoryAuthorities>> GetAllPendingRegAuth(long id);

        Task<List<ComplianceAPI.Models.RegulatoryAuthorities>> GetAllRegulatoryAuthorities();

        Task<Result> PostCountryRegulatoryAuthorityMapping(Models.CountryRegulatoryAuthorityMapping countryRegulatoryAuthorityMapping, int approveStatus);

        Task<List<Models.CountryRegulatoryAuthorityMapping>> GetAllPendingRegAuthMapping(long id);

        Task<List<Models.CountryRegulatoryAuthorityMapping>> GetAllRegAuthMapping();

        Task<List<Models.RegulatoryAuthorities>> GetRegulatoryAuthoritiesListCountry(int countryId);
    }

    public class RegulatoryAuthorityRepository : IRegulatoryAuthorityRepository
    {
        private readonly ComplianceDbContext _context;
        private readonly ILogger<RegulatoryAuthorityRepository> _logger;

        public RegulatoryAuthorityRepository(ComplianceDbContext dbContext, ILogger<RegulatoryAuthorityRepository> logger)
        {
            _context = dbContext;
            _logger = logger;
        }

        public async Task<List<PostRegulatoryAuthorities>> GetAllPendingRegAuth(long id)
        {
            var regList = new List<PostRegulatoryAuthorities>();
            try
            {
                regList = await (from approval in _context.RegulatoryAuthorityApproval
                                 join regAuth in _context.RegulatoryAuthority on approval.RegulatoryAuthorityId equals regAuth.Id into RegAuthJoin
                                 from regAuth in RegAuthJoin.DefaultIfEmpty()
                                 join user in _context.Users on approval.CreatedBy equals user.Id into UserJoin
                                 from user in UserJoin.DefaultIfEmpty()
                                 join status in _context.RefApprovalStatus on approval.ApprovalStatus equals status.Id into statusJoin
                                 from status in statusJoin.DefaultIfEmpty()
                                 join manager in _context.Users on approval.ManagerId equals manager.Id into managerJoin
                                 from manager in managerJoin.DefaultIfEmpty()
                                 where approval.CreatedBy == id || approval.ManagerId == id
                                 //where approval.ApprovalStatus == (int)RefApprovalStatus.Pending
                                 //where regAuth.Status == 0
                                 orderby approval.ModifiedOn ?? approval.CreatedOn descending
                                 select new PostRegulatoryAuthorities
                                 {
                                     RegulatoryAuthorityCode = regAuth.RegulatoryAuthorityCode,
                                     RegulatoryAuthorityName = regAuth.RegulatoryAuthorityName,
                                     RegulatoryAuthorityReferenceCode = regAuth.RegulatoryAuthorityReferenceCode,
                                     Id = regAuth.Id,
                                     ManagerId = approval.ManagerId,
                                     CreatedBy = approval.CreatedBy,
                                     CreatedOn = regAuth.CreatedOn,
                                     ModifiedBy = regAuth.ModifiedBy,
                                     ModifiedOn = regAuth.ModifiedOn,
                                     UID = regAuth.UID,
                                     UserName = user.FullName,
                                     ManagerName = manager.FullName,
                                     ApproveStatus = status.Status
                                 }).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllPendingRegAuth");
            }
            return regList;
        }

        public async Task<string> GetNextRegulatoryAuthRefCode()
        {
            var maxId = await _context.RegulatoryAuthority.MaxAsync(e => (long?)e.Id) ?? 0;
            // Increment by 1
            var nextId = maxId + 1;
            // Format as 'RA' + right 3 digits, padded with zeros
            var lastCode = $"{ReferencePrefixes.RegulatoryAuthority}{nextId.ToString().PadLeft(3, '0')}";
            return lastCode;
        }

        public async Task<Result> PostRegulatoryAuthority(PostRegulatoryAuthorities regulatoryAuthorities, int approveStatus)
        {
            var regAuth = new Result() { Success = false };
            try
            {
                regulatoryAuthorities.Status = approveStatus == (int)RefApprovalStatus.Approved ? 1 : 0;
                if (regulatoryAuthorities.Id.HasValue && regulatoryAuthorities.Id > 0)
                {
                    var regAuthDb = await _context.RegulatoryAuthority.FindAsync(regulatoryAuthorities.Id.Value);
                    if (regAuthDb != null)
                    {
                        regAuthDb.RegulatoryAuthorityCode = regulatoryAuthorities.RegulatoryAuthorityCode;
                        regAuthDb.RegulatoryAuthorityName = regulatoryAuthorities.RegulatoryAuthorityName;
                        regAuthDb.RegulatoryAuthorityReferenceCode = regulatoryAuthorities.RegulatoryAuthorityReferenceCode;
                        regAuthDb.ModifiedBy = regulatoryAuthorities.ModifiedBy;
                        regAuthDb.ModifiedOn = DateTime.UtcNow;
                        regAuthDb.Status = regulatoryAuthorities.Status;
                        _context.RegulatoryAuthority.Update(regAuthDb);
                        await _context.SaveChangesAsync();
                        AddApproveRegAuth(regulatoryAuthorities, approveStatus);
                    }
                    regAuth.Success = true;

                    if (approveStatus == (int)RefApprovalStatus.Approved)
                    {
                        regAuth.Message = "Regulatory Authority updated successfully";
                    }
                    else if (approveStatus == (int)RefApprovalStatus.Rejected)
                    {
                        regAuth.Message = "Regulatory Authority update was rejected";
                    }
                    else if (approveStatus == (int)RefApprovalStatus.Reviewed)
                    {
                        regAuth.Message = "Regulatory Authority update has been reviewed";
                    }
                    else if (approveStatus == (int)RefApprovalStatus.Forward)
                    {
                        regAuth.Message = "Regulatory Authority update has been forwarded";
                    }
                    else
                    {
                        regAuth.Message = "Regulatory Authority updated with unknown status";
                    }
                }
                else
                {
                    var newRegAuthDb = new Db.RegulatoryAuthorities
                    {
                        RegulatoryAuthorityCode = regulatoryAuthorities.RegulatoryAuthorityCode,
                        RegulatoryAuthorityName = regulatoryAuthorities.RegulatoryAuthorityName,
                        RegulatoryAuthorityReferenceCode = regulatoryAuthorities.RegulatoryAuthorityReferenceCode,
                        CreatedBy = regulatoryAuthorities.CreatedBy,
                        Status = regulatoryAuthorities.Status,
                        CreatedOn = DateTime.UtcNow,
                        UID = Guid.NewGuid().ToString()
                    };
                    await _context.RegulatoryAuthority.AddAsync(newRegAuthDb);
                    await _context.SaveChangesAsync();
                    regulatoryAuthorities.Id = newRegAuthDb.Id; // Set the generated ID back to the input model
                    AddApproveRegAuth(regulatoryAuthorities, approveStatus);
                    regAuth.Success = true;
                    if (approveStatus == (int)RefApprovalStatus.Approved)
                    {
                        regAuth.Message = "Regulatory Authority created successfully";
                    }
                    else
                    {
                        regAuth.Message = "Regulatory Authority created successfully and sent to Approval";
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in PostRegulatoryAuthority");
                regAuth.Message = "An error occurred while processing the request.";
            }

            return regAuth;
        }

        public async Task<List<Models.RegulatoryAuthorities>> GetAllRegulatoryAuthorities()
        {
            var regList = new List<Models.RegulatoryAuthorities>();
            try
            {
                // Get all regulatory authorities with status != 0
                var regAuths = await _context.RegulatoryAuthority
                    .Where(v => v.Status != 0)
                    .ToListAsync();

                regList.AddRange(regAuths.Select(r => new Models.RegulatoryAuthorities
                {
                    Id = r.Id,
                    RegulatoryAuthorityCode = r.RegulatoryAuthorityCode,
                    RegulatoryAuthorityName = r.RegulatoryAuthorityName,
                    RegulatoryAuthorityReferenceCode = r.RegulatoryAuthorityReferenceCode,
                    CreatedBy = r.CreatedBy,
                    CreatedOn = r.CreatedOn
                }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllRegulatoryAuthorities");
            }
            return regList;
        }

        public Result AddApproveRegAuth(PostRegulatoryAuthorities regulatoryAuthorities, int approveStatus)
        {
            var result = new Result() { Success = false };
            try
            {
                // Check if approval already exists for this RegulatoryAuthorityId and ManagerId
                var existingApproval = _context.RegulatoryAuthorityApproval
                    .FirstOrDefault(c => c.RegulatoryAuthorityId == regulatoryAuthorities.Id && c.ManagerId == regulatoryAuthorities.ManagerId && c.CreatedBy == regulatoryAuthorities.CreatedBy);

                if (existingApproval != null)
                {
                    existingApproval.ApprovalStatus = approveStatus;
                    existingApproval.ModifiedBy = regulatoryAuthorities.ModifiedBy;
                    existingApproval.ManagerId = regulatoryAuthorities.ManagerId;
                    existingApproval.ModifiedOn = DateTime.UtcNow;
                    _context.RegulatoryAuthorityApproval.Update(existingApproval);
                    result.Message = "Approval updated successfully.";
                }
                else
                {
                    // Add new approval
                    var newApproval = new Db.RegulatoryAuthorityApproval
                    {
                        RegulatoryAuthorityId = regulatoryAuthorities.Id ?? 0,
                        ManagerId = regulatoryAuthorities.ManagerId,
                        CreatedBy = regulatoryAuthorities.CreatedBy,
                        CreatedOn = DateTime.UtcNow,
                        UID = Guid.NewGuid(),
                        ApprovalStatus = approveStatus
                    };
                    _context.RegulatoryAuthorityApproval.Add(newApproval);
                    result.Message = "Approval added successfully.";
                }

                _context.SaveChanges();
                result.Success = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AddApproveRegAuth");
                result.Message = "An error occurred while processing the approval.";
            }

            return result;
        }

        public async Task<Result> PostCountryRegulatoryAuthorityMapping(Models.CountryRegulatoryAuthorityMapping countryRegulatoryAuthorityMapping, int approveStatus)
        {
            var result = new Result() { Success = false };
            try
            {
                countryRegulatoryAuthorityMapping.Status = approveStatus == (int)RefApprovalStatus.Approved ? 1 : 0;
                if (countryRegulatoryAuthorityMapping.Id.HasValue && countryRegulatoryAuthorityMapping.Id > 0)
                {
                    var regAuthDb = await _context.CountryRegulatoryAuthorityMapping.FindAsync(countryRegulatoryAuthorityMapping.Id.Value);
                    if (regAuthDb != null)
                    {
                        // Check for duplicate before update
                        var duplicate = await _context.CountryRegulatoryAuthorityMapping
                            .AnyAsync(c => c.CountryId == countryRegulatoryAuthorityMapping.CountryId
                                        && c.RegulatoryAuthorityId == countryRegulatoryAuthorityMapping.RegulatoryAuthorityId
                                        && c.Id != countryRegulatoryAuthorityMapping.Id.Value && c.Status == countryRegulatoryAuthorityMapping.Status);

                        if (duplicate)
                        {
                            result.Message = "Duplicate Country Regulatory Authority Mapping exists.";
                            return result;
                        }

                        regAuthDb.CountryId = countryRegulatoryAuthorityMapping.CountryId;
                        regAuthDb.RegulatoryAuthorityId = countryRegulatoryAuthorityMapping.RegulatoryAuthorityId;
                        regAuthDb.ModifiedBy = countryRegulatoryAuthorityMapping.ModifiedBy;
                        regAuthDb.ModifiedOn = DateTime.UtcNow;
                        regAuthDb.Status = countryRegulatoryAuthorityMapping.Status;
                        _context.CountryRegulatoryAuthorityMapping.Update(regAuthDb);
                        await _context.SaveChangesAsync();
                        if (approveStatus == (int)RefApprovalStatus.Approved)
                        {
                            result.Message = "Country Regulatory Authority Mapping updated successfully";
                        }
                        else if (approveStatus == (int)RefApprovalStatus.Rejected)
                        {
                            result.Message = "Country Regulatory Authority Mapping  was rejected";
                        }
                        else if (approveStatus == (int)RefApprovalStatus.Reviewed)
                        {
                            result.Message = "Country Regulatory Authority Mapping  has been reviewed";
                        }
                        else if (approveStatus == (int)RefApprovalStatus.Forward)
                        {
                            result.Message = "Country Regulatory Authority Mapping update has been forwarded";
                        }
                        else
                        {
                            result.Message = "Country Regulatory Authority Mapping updated with unknown status";
                        }
                        await AddCountryRegulatoryAuthorites(countryRegulatoryAuthorityMapping, approveStatus);
                        result.Success = true;
                    }
                }
                else
                {
                    var existingMapping = await _context.CountryRegulatoryAuthorityMapping
                        .FirstOrDefaultAsync(c => c.CountryId == countryRegulatoryAuthorityMapping.CountryId
                                               && c.RegulatoryAuthorityId == countryRegulatoryAuthorityMapping.RegulatoryAuthorityId);
                    if (existingMapping == null)
                    {
                        var newMapping = new Db.CountryRegulatoryAuthorityMapping
                        {
                            CountryId = countryRegulatoryAuthorityMapping.CountryId,
                            RegulatoryAuthorityId = countryRegulatoryAuthorityMapping.RegulatoryAuthorityId,
                            CreatedBy = countryRegulatoryAuthorityMapping.CreatedBy,
                            CreatedOn = DateTime.UtcNow,
                            Status = countryRegulatoryAuthorityMapping.Status,
                            UID = Guid.NewGuid().ToString(),
                            CountryRegAuthReferenceCode = ""
                        };
                        await _context.CountryRegulatoryAuthorityMapping.AddAsync(newMapping);
                        await _context.SaveChangesAsync();
                        countryRegulatoryAuthorityMapping.Id = newMapping.Id;
                        await AddCountryRegulatoryAuthorites(countryRegulatoryAuthorityMapping, approveStatus);
                        result.Success = true;
                        if (approveStatus == (int)RefApprovalStatus.Approved)
                        {
                            result.Message = "Country Regulatory Authority Mapping created successfully";
                        }
                        else
                        {
                            result.Message = "Country Regulatory Authority Mapping created successfully and sent to approval";
                        }
                    }
                    else
                    {
                        result.Message = "Country Regulatory Authority Mapping already exists.";
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in PostCountryRegulatoryAuthorityMapping");
                result.Message = "An error occurred while processing the request.";
            }

            return result;
        }

        private async Task AddCountryRegulatoryAuthorites(Models.CountryRegulatoryAuthorityMapping countryRegulatoryAuthorityMapping, int approveStatus)
        {
            try
            {
                // Check if approval already exists for this CountryRegulatoryAuthorityMappingId
                var existingApproval = await _context.CountryRegulatoryAuthorityMappingApproval
                    .FirstOrDefaultAsync(a => a.CountryRegulatoryAuthorityMappingId == (countryRegulatoryAuthorityMapping.Id) && a.CreatedBy == countryRegulatoryAuthorityMapping.CreatedBy);

                if (existingApproval != null)
                {
                    // Update approval status
                    // If there are more fields to update, add them here
                    existingApproval.ModifiedBy = countryRegulatoryAuthorityMapping.ModifiedBy;
                    existingApproval.ModifiedOn = DateTime.UtcNow;
                    existingApproval.ManagerId = countryRegulatoryAuthorityMapping.ManagerId ?? 0;
                    existingApproval.ApproveStatus = approveStatus;
                    _context.CountryRegulatoryAuthorityMappingApproval.Update(existingApproval);
                }
                else
                {
                    var newApproval = new CountryRegulatoryAuthorityMappingApproval
                    {
                        CountryRegulatoryAuthorityMappingId = countryRegulatoryAuthorityMapping.Id ?? 0,
                        CreatedBy = countryRegulatoryAuthorityMapping.CreatedBy,
                        CreatedOn = DateTime.UtcNow,

                        ManagerId = countryRegulatoryAuthorityMapping.ManagerId ?? 0,
                        UID = Guid.NewGuid().ToString(),
                        ApproveStatus = approveStatus
                    };

                    await _context.CountryRegulatoryAuthorityMappingApproval.AddAsync(newApproval);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AddCountryRegulatoryAuthorities");
            }
        }

        public async Task<List<Models.CountryRegulatoryAuthorityMapping>> GetAllPendingRegAuthMapping(long id)
        {
            var regList = new List<Models.CountryRegulatoryAuthorityMapping>();
            try
            {
                regList = await (from mappingApproval in _context.CountryRegulatoryAuthorityMappingApproval
                                 join regAuth in _context.CountryRegulatoryAuthorityMapping on mappingApproval.CountryRegulatoryAuthorityMappingId equals regAuth.Id into RegAuthJoin
                                 from regAuth in RegAuthJoin.DefaultIfEmpty()
                                 join reg in _context.RegulatoryAuthority on regAuth.RegulatoryAuthorityId equals reg.Id into regJoin
                                 from reg in regJoin.DefaultIfEmpty()
                                 join country in _context.Country on regAuth.CountryId equals country.Id into countryJoin
                                 from country in countryJoin.DefaultIfEmpty()
                                 join user in _context.Users on mappingApproval.CreatedBy equals user.Id into UserJoin
                                 from user in UserJoin.DefaultIfEmpty()
                                 join status in _context.RefApprovalStatus on mappingApproval.ApproveStatus equals status.Id into statusJoin
                                 from status in statusJoin.DefaultIfEmpty()
                                 join manager in _context.Users on mappingApproval.ManagerId equals manager.Id into managerJoin
                                 from manager in managerJoin.DefaultIfEmpty()
                                 where mappingApproval.CreatedBy == id || mappingApproval.ManagerId == id
                                 //where approval.ApprovalStatus == (int)RefApprovalStatus.Pending
                                 //where regAuth.Status == 0
                                 orderby mappingApproval.ModifiedOn ?? mappingApproval.CreatedOn descending
                                 select new Models.CountryRegulatoryAuthorityMapping
                                 {
                                     RegulatoryAuthorityId = regAuth.RegulatoryAuthorityId,
                                     ManagerId = mappingApproval.ManagerId,
                                     CreatedBy = mappingApproval.CreatedBy,
                                     CreatedOn = regAuth.CreatedOn,
                                     ModifiedBy = regAuth.ModifiedBy,
                                     ModifiedOn = regAuth.ModifiedOn,
                                     UID = regAuth.UID,
                                     Id = regAuth.Id,
                                     UserName = user.FullName,
                                     ManagerName = manager.FullName,
                                     ApproveStatus = status.Status,
                                     RegAuthName = reg.RegulatoryAuthorityName,
                                     CountryId = regAuth.CountryId,
                                     CountryName = country.CountryName
                                 }).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllPendingRegAuth");
            }
            return regList;
        }

        public async Task<List<Models.CountryRegulatoryAuthorityMapping>> GetAllRegAuthMapping()
        {
            var regList = new List<Models.CountryRegulatoryAuthorityMapping>();
            try
            {
                regList = await (from mappingApproval in _context.CountryRegulatoryAuthorityMappingApproval
                                 join regAuth in _context.CountryRegulatoryAuthorityMapping on mappingApproval.CountryRegulatoryAuthorityMappingId equals regAuth.Id into RegAuthJoin
                                 from regAuth in RegAuthJoin.DefaultIfEmpty()
                                 join reg in _context.RegulatoryAuthority on regAuth.RegulatoryAuthorityId equals reg.Id into regJoin
                                 from reg in regJoin.DefaultIfEmpty()
                                 join country in _context.Country on regAuth.CountryId equals country.Id into countryJoin
                                 from country in countryJoin.DefaultIfEmpty()
                                 join user in _context.Users on mappingApproval.CreatedBy equals user.Id into UserJoin
                                 from user in UserJoin.DefaultIfEmpty()
                                 join status in _context.RefApprovalStatus on mappingApproval.ApproveStatus equals status.Id into statusJoin
                                 from status in statusJoin.DefaultIfEmpty()
                                 join manager in _context.Users on mappingApproval.ManagerId equals manager.Id into managerJoin
                                 from manager in managerJoin.DefaultIfEmpty()
                                 where mappingApproval.ApproveStatus == (int)RefApprovalStatus.Approved
                                 where regAuth.Status == 1
                                 orderby mappingApproval.ModifiedOn ?? mappingApproval.CreatedOn descending
                                 select new Models.CountryRegulatoryAuthorityMapping
                                 {
                                     RegulatoryAuthorityId = regAuth.RegulatoryAuthorityId,
                                     ManagerId = mappingApproval.ManagerId,
                                     CreatedBy = mappingApproval.CreatedBy,
                                     CreatedOn = regAuth.CreatedOn,
                                     ModifiedBy = regAuth.ModifiedBy,
                                     ModifiedOn = regAuth.ModifiedOn,
                                     UID = regAuth.UID,
                                     Id = regAuth.Id,
                                     UserName = user.FullName,
                                     ManagerName = manager.FullName,
                                     ApproveStatus = status.Status,
                                     RegAuthName = reg.RegulatoryAuthorityName,
                                     CountryId = regAuth.CountryId,
                                     CountryName = country.CountryName
                                 }).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllPendingRegAuth");
            }
            return regList;
        }

        public async Task<List<Models.RegulatoryAuthorities>> GetRegulatoryAuthoritiesListCountry(int countryId)
        {
            var listComplianceList = new List<Models.RegulatoryAuthorities>();
            try
            {
                listComplianceList = await (from mappingApproval in _context.CountryRegulatoryAuthorityMapping
                                            join regulatoryAuthority in _context.RegulatoryAuthority on mappingApproval.RegulatoryAuthorityId equals regulatoryAuthority.Id into regulatoryAuthorityJoin
                                            from regulatoryAuthority in regulatoryAuthorityJoin.DefaultIfEmpty()
                                            where mappingApproval.CountryId == countryId
                                            select new Models.RegulatoryAuthorities
                                            {
                                                Id = regulatoryAuthority.Id,
                                                RegulatoryAuthorityCode = regulatoryAuthority.RegulatoryAuthorityCode ?? string.Empty,
                                                RegulatoryAuthorityName = regulatoryAuthority.RegulatoryAuthorityName
                                            }).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetRegulatoryAuthoritiesListCountry");
            }

            return listComplianceList;
        }
    }
}