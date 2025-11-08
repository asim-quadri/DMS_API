using ComplianceAPI.Models;
using ComplianceAPI.Models.DataModels;
using ComplianceAPI.Models.Enums;
using Microsoft.EntityFrameworkCore;

using Db = ComplianceAPI.Models.DataModels;
using Helper = ComplianceAPI.Models.Enums;

namespace ComplianceAPI.Repository
{
    public interface IConcernedMinistryRepository
    {
        Task<Result> PostConcernedMinistry(PostConcernedMinistry postConcernedMinistry, int approveStatus);

        Task<string> GetNextConcernedMinistryRefCode();

        Task<List<PostConcernedMinistry>> GetAllPendingConcernedMinistry(long id);

        Task<List<ComplianceAPI.Models.ConcernedMinistry>> GetAllConcernedMinistry();

        Task<Result> PostCountryConcernedMinistryMapping(Models.CountryConcernedMinistryMapping countryConcernedMinistryMapping, int approveStatus);

        Task<List<Models.CountryConcernedMinistryMapping>> GetAllPendingConcernedMinistryMapping(long id);

        Task<List<Models.CountryConcernedMinistryMapping>> GetAllConcernedMinistryMapping();

        Task<List<Models.ConcernedMinistry>> GetConcernedMinistryListCountry(int countryId);
    }

    public class ConcernedMinistryRepository : IConcernedMinistryRepository
    {
        private readonly ComplianceDbContext _context;
        private readonly ILogger<ConcernedMinistryRepository> _logger;

        public ConcernedMinistryRepository(ComplianceDbContext dbContext, ILogger<ConcernedMinistryRepository> logger)
        {
            _context = dbContext;
            _logger = logger;
        }

        public async Task<List<Models.ConcernedMinistry>> GetAllConcernedMinistry()
        {
            var concernedMinistries = new List<Models.ConcernedMinistry>();
            try
            {
                var conMins = await _context.ConcernedMinistries
                    .Where(v => v.Status != 0)
                    .ToListAsync();

                concernedMinistries.AddRange(conMins.Select(r => new Models.ConcernedMinistry
                {
                    Id = r.Id,
                    ConcernedMinistryCode = r.ConcernedMinistryCode,
                    ConcernedMinistryName = r.ConcernedMinistryName,
                    ConcernedMinistryReferenceCode = r.ConcernedMinistryReferenceCode,
                    CreatedBy = r.CreatedBy,
                    CreatedOn = r.CreatedOn
                }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllConcernedMinistry");
            }
            return concernedMinistries;
        }

        public async Task<List<Models.CountryConcernedMinistryMapping>> GetAllConcernedMinistryMapping()
        {
            var regList = new List<Models.CountryConcernedMinistryMapping>();
            try
            {
                regList = await (from mappingApproval in _context.CountryConcernedMinistryMappingApproval
                                 join counConMin in _context.CountryConcernedMinistryMapping on mappingApproval.CountryConcernedMinistryMappingId equals counConMin.Id into conMinJoin
                                 from conMin in conMinJoin.DefaultIfEmpty()
                                 join reg in _context.ConcernedMinistries on conMin.ConcernedMinistryId equals reg.Id into regJoin
                                 from reg in regJoin.DefaultIfEmpty()
                                 join country in _context.Country on conMin.CountryId equals country.Id into countryJoin
                                 from country in countryJoin.DefaultIfEmpty()
                                 join user in _context.Users on mappingApproval.CreatedBy equals user.Id into UserJoin
                                 from user in UserJoin.DefaultIfEmpty()
                                 join status in _context.RefApprovalStatus on mappingApproval.ApproveStatus equals status.Id into statusJoin
                                 from status in statusJoin.DefaultIfEmpty()
                                 join manager in _context.Users on mappingApproval.ManagerId equals manager.Id into managerJoin
                                 from manager in managerJoin.DefaultIfEmpty()
                                 where mappingApproval.ApproveStatus == (int)Helper.RefApprovalStatus.Approved
                                 where conMin.Status == 1
                                 orderby mappingApproval.ModifiedOn ?? mappingApproval.CreatedOn descending
                                 select new Models.CountryConcernedMinistryMapping
                                 {
                                     ConcernedMinistryId = conMin.ConcernedMinistryId,
                                     ManagerId = mappingApproval.ManagerId,
                                     CreatedBy = mappingApproval.CreatedBy,
                                     CreatedOn = conMin.CreatedOn,
                                     ModifiedBy = conMin.ModifiedBy,
                                     ModifiedOn = conMin.ModifiedOn,
                                     UID = conMin.UID,
                                     Id = conMin.Id,
                                     UserName = user.FullName,
                                     ManagerName = manager.FullName,
                                     ApproveStatus = status.Status,
                                     ConcernedMinistryName = reg.ConcernedMinistryName,
                                     CountryConcernedMinistryReferenceCode = reg.ConcernedMinistryReferenceCode,
                                     CountryId = conMin.CountryId,
                                     CountryName = country.CountryName
                                 }).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllPendingRegAuth");
            }
            return regList;
        }

        public async Task<List<PostConcernedMinistry>> GetAllPendingConcernedMinistry(long id)
        {
            var conMinList = new List<PostConcernedMinistry>();
            try
            {
                conMinList = await (from approval in _context.ConcernedMinistryApproval
                                    join conMin in _context.ConcernedMinistries on approval.ConcernedMinistryId equals conMin.Id into conMinJoin
                                    from conMin in conMinJoin.DefaultIfEmpty()
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
                                    select new PostConcernedMinistry
                                    {
                                        ConcernedMinistryCode = conMin.ConcernedMinistryCode,
                                        ConcernedMinistryName = conMin.ConcernedMinistryName,
                                        ConcernedMinistryReferenceCode = conMin.ConcernedMinistryReferenceCode,
                                        Id = conMin.Id,
                                        ManagerId = approval.ManagerId,
                                        CreatedBy = approval.CreatedBy,
                                        CreatedOn = conMin.CreatedOn,
                                        ModifiedBy = conMin.ModifiedBy,
                                        ModifiedOn = conMin.ModifiedOn,
                                        UID = conMin.UID,
                                        UserName = user.FullName,
                                        ManagerName = manager.FullName,
                                        ApproveStatus = status.Status
                                    }).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllPendingConcernedMinistry");
            }
            return conMinList;
        }

        public async Task<List<Models.CountryConcernedMinistryMapping>> GetAllPendingConcernedMinistryMapping(long id)
        {
            var concernedMinistrymappingList = new List<Models.CountryConcernedMinistryMapping>();
            try
            {
                concernedMinistrymappingList = await (from mappingApproval in _context.CountryConcernedMinistryMappingApproval
                                                      join concernedMinistry in _context.CountryConcernedMinistryMapping on mappingApproval.CountryConcernedMinistryMappingId equals concernedMinistry.Id into concernedMinistryJoin
                                                      from concernedMinistry in concernedMinistryJoin.DefaultIfEmpty()
                                                      join concernedMinistries in _context.ConcernedMinistries on concernedMinistry.ConcernedMinistryId equals concernedMinistries.Id into concernedMinistriesJoin
                                                      from concernedMinistries in concernedMinistriesJoin.DefaultIfEmpty()
                                                      join country in _context.Country on concernedMinistry.CountryId equals country.Id into countryJoin
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
                                                      select new Models.CountryConcernedMinistryMapping
                                                      {
                                                          ConcernedMinistryId = concernedMinistry.ConcernedMinistryId,
                                                          ManagerId = mappingApproval.ManagerId,
                                                          CreatedBy = mappingApproval.CreatedBy,
                                                          CreatedOn = concernedMinistry.CreatedOn,
                                                          ModifiedBy = concernedMinistry.ModifiedBy,
                                                          ModifiedOn = concernedMinistry.ModifiedOn,
                                                          UID = concernedMinistry.UID,
                                                          Id = concernedMinistry.Id,
                                                          UserName = user.FullName,
                                                          ManagerName = manager.FullName,
                                                          ApproveStatus = status.Status,
                                                          ConcernedMinistryName = concernedMinistries.ConcernedMinistryName,
                                                          CountryId = concernedMinistry.CountryId,
                                                          CountryName = country.CountryName
                                                      }).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllPendingConcernedMinistryMapping");
            }
            return concernedMinistrymappingList;
        }

        public async Task<string> GetNextConcernedMinistryRefCode()
        {
            var maxId = await _context.ConcernedMinistries.MaxAsync(e => (long?)e.Id) ?? 0;
            // Increment by 1
            var nextId = maxId + 1;
            var lastCode = $"{ReferencePrefixes.ConcernedMinistry}{nextId.ToString().PadLeft(3, '0')}";
            return lastCode;
        }

        public async Task<Result> PostConcernedMinistry(PostConcernedMinistry postConcernedMinistry, int approveStatus)
        {
            var regAuth = new Result() { Success = false };
            try
            {
                postConcernedMinistry.Status = approveStatus == (int)Helper.RefApprovalStatus.Approved ? 1 : 0;
                if (postConcernedMinistry.Id.HasValue && postConcernedMinistry.Id > 0)
                {
                    // Update logic
                    var updateConMinDb = await _context.ConcernedMinistries.FindAsync(postConcernedMinistry.Id.Value);
                    if (updateConMinDb != null)
                    {
                        updateConMinDb.ConcernedMinistryReferenceCode = postConcernedMinistry.ConcernedMinistryReferenceCode;
                        updateConMinDb.ConcernedMinistryName = postConcernedMinistry.ConcernedMinistryName;
                        updateConMinDb.ConcernedMinistryCode = postConcernedMinistry.ConcernedMinistryCode;
                        updateConMinDb.ModifiedBy = postConcernedMinistry.ModifiedBy;
                        updateConMinDb.ModifiedOn = DateTime.UtcNow;
                        updateConMinDb.Status = postConcernedMinistry.Status;
                        _context.ConcernedMinistries.Update(updateConMinDb);
                        await _context.SaveChangesAsync();
                        AddApproveConcernedMinistry(postConcernedMinistry, approveStatus);
                    }
                    regAuth.Success = true;
                    if (approveStatus == (int)Helper.RefApprovalStatus.Approved)
                    {
                        regAuth.Message = "Concerned Ministry updated successfully";
                    }
                    else if (approveStatus == (int)Helper.RefApprovalStatus.Rejected)
                    {
                        regAuth.Message = "Concerned Ministry update was rejected";
                    }
                    else if (approveStatus == (int)Helper.RefApprovalStatus.Reviewed)
                    {
                        regAuth.Message = "Concerned Ministry update has been reviewed";
                    }
                    else if (approveStatus == (int)Helper.RefApprovalStatus.Forward)
                    {
                        regAuth.Message = "Concerned Ministry update has been forwarded";
                    }
                    else
                    {
                        regAuth.Message = "Concerned Ministry updated with unknown status";
                    }
                }
                else
                {
                    // Add logic
                    var newConMinDb = new Db.ConcernedMinistry
                    {
                        ConcernedMinistryCode = postConcernedMinistry.ConcernedMinistryCode,
                        ConcernedMinistryName = postConcernedMinistry.ConcernedMinistryName,
                        ConcernedMinistryReferenceCode = postConcernedMinistry.ConcernedMinistryReferenceCode,
                        CreatedBy = postConcernedMinistry.CreatedBy,
                        Status = postConcernedMinistry.Status,
                        CreatedOn = DateTime.UtcNow,
                        UID = Guid.NewGuid().ToString()
                    };
                    await _context.ConcernedMinistries.AddAsync(newConMinDb);
                    await _context.SaveChangesAsync();
                    postConcernedMinistry.Id = newConMinDb.Id; // Set the generated ID back to the input model
                    AddApproveConcernedMinistry(postConcernedMinistry, approveStatus);
                    regAuth.Success = true;
                    if (approveStatus == (int)Helper.RefApprovalStatus.Approved)
                    {
                        regAuth.Message = "Concerned Ministry created successfully";
                    }
                    else
                    {
                        regAuth.Message = "Concerned Ministry created successfully and sent to Approval";
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in PostConcerned Ministry");
                regAuth.Message = "Concerned Ministry:-An error occurred while processing the request.";
            }

            return regAuth;
        }

        public async Task<Result> PostCountryConcernedMinistryMapping(Models.CountryConcernedMinistryMapping countryConcernedMinistryMapping, int approveStatus)
        {
            var result = new Result() { Success = false };
            try
            {
                countryConcernedMinistryMapping.Status = approveStatus == (int)Helper.RefApprovalStatus.Approved ? 1 : 0;
                if (countryConcernedMinistryMapping.Id.HasValue && countryConcernedMinistryMapping.Id > 0)
                {
                    var regAuthDb = await _context.CountryConcernedMinistryMapping.FindAsync(countryConcernedMinistryMapping.Id.Value);
                    if (regAuthDb != null)
                    {
                        // Check for duplicate before update
                        var duplicate = await _context.CountryConcernedMinistryMapping
                            .AnyAsync(c => c.CountryId == countryConcernedMinistryMapping.CountryId
                                        && c.ConcernedMinistryId == countryConcernedMinistryMapping.ConcernedMinistryId
                                        && c.Id != countryConcernedMinistryMapping.Id.Value && c.Status == countryConcernedMinistryMapping.Status);

                        if (duplicate)
                        {
                            result.Message = "Duplicate Country Concerned Ministry Mapping exists.";
                            return result;
                        }

                        regAuthDb.CountryId = countryConcernedMinistryMapping.CountryId;
                        regAuthDb.ConcernedMinistryId = countryConcernedMinistryMapping.ConcernedMinistryId;
                        regAuthDb.ModifiedBy = countryConcernedMinistryMapping.ModifiedBy;
                        regAuthDb.ModifiedOn = DateTime.UtcNow;
                        regAuthDb.Status = countryConcernedMinistryMapping.Status;
                        _context.CountryConcernedMinistryMapping.Update(regAuthDb);
                        await _context.SaveChangesAsync();
                        if (approveStatus == (int)Helper.RefApprovalStatus.Approved)
                        {
                            result.Message = "Country Concerned Ministry Mapping updated successfully";
                        }
                        else if (approveStatus == (int)Helper.RefApprovalStatus.Rejected)
                        {
                            result.Message = "Country Concerned Ministry Mapping was rejected";
                        }
                        else if (approveStatus == (int)Helper.RefApprovalStatus.Reviewed)
                        {
                            result.Message = "Country Concerned Ministry Mapping has been reviewed";
                        }
                        else if (approveStatus == (int)Helper.RefApprovalStatus.Forward)
                        {
                            result.Message = "Country Concerned Ministry Mapping update has been forwarded";
                        }
                        else
                        {
                            result.Message = "Country Concerned Ministry Mapping updated with unknown status";
                        }

                        await AddCountryConcernedMinistryMapping(countryConcernedMinistryMapping, approveStatus);
                        result.Success = true;
                    }
                }
                else
                {
                    var existingMapping = await _context.CountryConcernedMinistryMapping
                        .FirstOrDefaultAsync(c => c.CountryId == countryConcernedMinistryMapping.CountryId
                                               && c.ConcernedMinistryId == countryConcernedMinistryMapping.ConcernedMinistryId);
                    if (existingMapping == null)
                    {
                        var newMapping = new Db.CountryConcernedMinistryMapping
                        {
                            CountryId = countryConcernedMinistryMapping.CountryId,
                            ConcernedMinistryId = countryConcernedMinistryMapping.ConcernedMinistryId,
                            CreatedBy = countryConcernedMinistryMapping.CreatedBy,
                            CreatedOn = DateTime.UtcNow,
                            Status = countryConcernedMinistryMapping.Status,
                            UID = Guid.NewGuid().ToString(),
                            CountryConMinReferenceCode = ""
                        };
                        await _context.CountryConcernedMinistryMapping.AddAsync(newMapping);
                        await _context.SaveChangesAsync();
                        countryConcernedMinistryMapping.Id = newMapping.Id;
                        await AddCountryConcernedMinistryMapping(countryConcernedMinistryMapping, approveStatus);
                        result.Success = true;
                        if (approveStatus == (int)Helper.RefApprovalStatus.Approved)
                        {
                            result.Message = "Country Concerned Ministry Mapping created successfully";
                        }
                        else
                        {
                            result.Message = "Country Concerned Ministry Mapping created successfully and sent to approval";
                        }
                    }
                    else
                    {
                        result.Message = "Country Concerned Ministry not found.";
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in PostCountryConcernedMinistryMapping");
                result.Message = "An error occurred while processing the request.";
            }

            return result;
        }

        private void AddApproveConcernedMinistry(PostConcernedMinistry postConcernedMinistry, int approveStatus)
        {
            try
            {
                var existingApproval = _context.ConcernedMinistryApproval
                    .FirstOrDefault(c => c.ConcernedMinistryId == postConcernedMinistry.Id && c.ManagerId == postConcernedMinistry.ManagerId && c.CreatedBy == postConcernedMinistry.CreatedBy);

                if (existingApproval != null)
                {
                    existingApproval.ApprovalStatus = approveStatus;
                    existingApproval.ModifiedBy = postConcernedMinistry.ModifiedBy;
                    existingApproval.ManagerId = postConcernedMinistry.ManagerId;
                    existingApproval.ModifiedOn = DateTime.UtcNow;
                    _context.ConcernedMinistryApproval.Update(existingApproval);
                }
                else
                {
                    // Add new approval
                    var newApproval = new Db.ConcernedMinistryApproval
                    {
                        ConcernedMinistryId = postConcernedMinistry.Id ?? 0,
                        ManagerId = postConcernedMinistry.ManagerId,
                        CreatedBy = postConcernedMinistry.CreatedBy,
                        CreatedOn = DateTime.UtcNow,
                        UID = Guid.NewGuid(),
                        ApprovalStatus = approveStatus
                    };
                    _context.ConcernedMinistryApproval.Add(newApproval);
                }

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AddApproveConcernedMinistry");
            }
        }

        private async Task AddCountryConcernedMinistryMapping(Models.CountryConcernedMinistryMapping countryConcernedMinistryMapping, int approveStatus)
        {
            try
            {
                var existingApproval = await _context.CountryConcernedMinistryMappingApproval
                    .FirstOrDefaultAsync(c => c.CountryConcernedMinistryMappingId == countryConcernedMinistryMapping.Id && c.ManagerId == countryConcernedMinistryMapping.ManagerId && c.CreatedBy == countryConcernedMinistryMapping.CreatedBy);
                if (existingApproval != null)
                {
                    existingApproval.ApproveStatus = approveStatus;
                    existingApproval.ModifiedBy = countryConcernedMinistryMapping.ModifiedBy;
                    existingApproval.ManagerId = countryConcernedMinistryMapping.ManagerId ?? 0;
                    existingApproval.ModifiedOn = DateTime.UtcNow;
                    _context.CountryConcernedMinistryMappingApproval.Update(existingApproval);
                }
                else
                {
                    // Add new approval
                    var newApproval = new Db.CountryConcernedMinistryMappingApproval
                    {
                        CountryConcernedMinistryMappingId = countryConcernedMinistryMapping.Id ?? 0,
                        ManagerId = countryConcernedMinistryMapping.ManagerId ?? 0,
                        CreatedBy = countryConcernedMinistryMapping.CreatedBy,
                        CreatedOn = DateTime.UtcNow,
                        UID = Guid.NewGuid().ToString(),
                        ApproveStatus = approveStatus
                    };
                    _context.CountryConcernedMinistryMappingApproval.Add(newApproval);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AddCountryRegulatoryAuthority");
            }
        }

        public async Task<List<Models.ConcernedMinistry>> GetConcernedMinistryListCountry(int countryId)
        {
            var listComplianceList = new List<Models.ConcernedMinistry>();
            try
            {
                listComplianceList = await (from mappingApproval in _context.CountryConcernedMinistryMapping
                                            join concernedMinistry in _context.ConcernedMinistries on mappingApproval.ConcernedMinistryId equals concernedMinistry.Id into concernedMinistryJoin
                                            from concernedMinistry in concernedMinistryJoin.DefaultIfEmpty()
                                            where mappingApproval.CountryId == countryId
                                            select new Models.ConcernedMinistry
                                            {
                                                Id = concernedMinistry.Id,
                                                ConcernedMinistryCode = concernedMinistry.ConcernedMinistryCode,
                                                ConcernedMinistryName = concernedMinistry.ConcernedMinistryName
                                            }).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetComplianceListCountry");
            }

            return listComplianceList;
        }
    }
}