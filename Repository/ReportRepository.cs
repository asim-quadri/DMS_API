using ComplianceAPI.Models;
using ComplianceAPI.Models.DataModels;
using Microsoft.EntityFrameworkCore;
using Dto = ComplianceAPI.Models;

namespace ComplianceAPI.Repository
{
    public interface IReportRepository
    {
        Task<List<Dto.Country>> GetAllCountriesAsync();

        Task<List<Dto.ReportMaster>> GetAllReportMaster();

        Task<List<Dto.RegulationStupDetails>> GetAllRegulationWithCountriesAsync();

        Task<List<RegulationComplianceWithCountryModel>> GetAllComplianceWithCountryAsync();

        Task<List<EntityTypeModel>> GetAllEntityTypes();

        Task<List<IndustryrMapping>> GetIndustryCountryMapping();

    }

    public class ReportRepository : IReportRepository
    {
        private readonly ComplianceDbContext _context;
        private readonly ILogger<ReportRepository> _logger;

        public ReportRepository(ComplianceDbContext context, ILogger<ReportRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Dto.Country>> GetAllCountriesAsync()
        {
            var countries = new List<Dto.Country>();
            try
            {
                countries = await (from country in _context.Country
                                   join CurrencyCodes in _context.CurrencyCodes
                                       on country.CurrencyId equals CurrencyCodes.Id into CurrencyCodesJoin
                                   from CurrencyCodes in CurrencyCodesJoin.DefaultIfEmpty()
                                   join user in _context.Users
                                      on country.CreatedBy equals user.Id into userJoin
                                   from user
                                   in userJoin.DefaultIfEmpty()
                                   join manager in _context.Users
                                     on country.ModifiedBy equals manager.Id into managerJoin
                                   from manager
                                   in managerJoin.DefaultIfEmpty()
                                   orderby country.ModifiedOn ?? country.CreatedOn descending
                                   where country != null && country.Status != 0
                                   select new Models.Country
                                   {
                                       Id = country.Id != null ? (long)country.Id : 0,
                                       CountryName = country.CountryName,
                                       CountryCode = country.CountryCode,
                                       CountryCodeNumber = country.CountryCodeNumber,
                                       FinancialStartDate = country.FinancialStartDate,
                                       FinancialEndDate = country.FinancialEndDate,
                                       Status = country.Status,
                                       CurrencyId = country.CurrencyId,
                                       CurrencyCode = CurrencyCodes != null ? CurrencyCodes.CurrencyCode : null,
                                       CreatedOn = country.CreatedOn,
                                       CreatedBy = country.CreatedBy != null ? (int?)country.CreatedBy : null,
                                       ModifiedOn = country.ModifiedOn,
                                       ModifiedBy = country.ModifiedBy != null ? (int?)country.ModifiedBy : null,
                                       UID = country.UID,
                                       CountryReferenceCode = country.CountryReferenceCode,
                                       AddedBy = user != null ? user.FullName : string.Empty,
                                       ApprovedBy = manager != null ? manager.FullName : string.Empty,
                                   }).Distinct().ToListAsync();

                return countries;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ReportRepo:-An error occurred while retrieving countries");
            }
            return countries;
        }

        public async Task<List<RegulationStupDetails>> GetAllRegulationWithCountriesAsync()
        {
            try
            {
                var results = await (from rs in _context.RegulationSetupDetails
                                     join c in _context.Country on rs.CountryId equals c.Id
                                     join s in _context.States on rs.StateId equals s.Id into stateJoin
                                     from s in stateJoin.DefaultIfEmpty()
                                     join u in _context.Users on rs.CreatedBy equals u.Id into userJoin
                                     from u in userJoin.DefaultIfEmpty()
                                     join m in _context.Users on rs.ModifiedBy equals m.Id into managerJoin
                                     from m in managerJoin.DefaultIfEmpty()
                                     join rg in _context.RegulationGroups on rs.RegulationGroupId equals rg.Id
                                     join e in _context.EntityType on rs.EntityTypeId equals e.Id
                                     select new RegulationStupDetails
                                     {
                                         Id = rs.Id,
                                         UID = rs.UID,
                                         RegulationName = rs.RegulationName,
                                         RegulationType = rs.RegulationType,
                                         CountryId = rs.CountryId,
                                         CountryName = c.CountryName,
                                         StateId = rs.StateId,
                                         StateName = s != null ? s.StateName : null,
                                         RegulationGroupId = rs.RegulationGroupId,
                                         RegulationGroupName = rg.RegulationGroupName,
                                         EntityTypeId = rs.EntityTypeId,
                                         EntityType = e.EntityType,
                                         MajorIndustryId = rs.MajorIndustryId,
                                         MinorIndustryId = rs.MinorIndustryId,
                                         Description = rs.Description,
                                         Status = rs.Status,
                                         CreatedOn = rs.CreatedOn,
                                         CreatedBy = rs.CreatedBy,
                                         ModifiedBy = rs.ModifiedBy,
                                         ModifiedOn = rs.ModifiedOn,
                                         isParameterChecked = rs.IsParameterChecked,
                                         RegulationSetupDetailsReferenceCode = rs.RegulationSetupDetailsReferenceCode,
                                         ApprovedBy = m.FullName,
                                         AddedBy = u.FullName
                                     }).ToListAsync();

                return results;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ReportRepo:-An error occurred while retrieving regulation details");
                return new List<RegulationStupDetails>();
            }
        }

        public async Task<List<Dto.ReportMaster>> GetAllReportMaster()
        {
            var report = new List<Dto.ReportMaster>();
            try
            {
                report = await _context.ReportMasters
                    .Where(r => r.IsActive)
                    .Select(r => new Dto.ReportMaster
                    {
                        Id = r.Id,
                        ReportSequence = r.ReportSequence,
                        ReportDescription = r.ReportDescription,
                        IsActive = r.IsActive
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ReportRepo:-An error occurred while retrieving the report master");
            }
            return report;
        }

        public async Task<List<RegulationComplianceWithCountryModel>> GetAllComplianceWithCountryAsync()
        {
            try
            {
                // 1. Fetch parameters grouped by RegulationSetupId and TOCRuleType
                var tocParamRaw = await (
                    from tp in _context.TOCParameter
                    join p in _context.Parameter on tp.ParameterTypeId equals p.Id
                    select new
                    {
                        tp.RegulationSetupId,
                        tp.TOCRuleType,
                        ParameterName = p.ParameterName,
                        ParameterValue = tp.ParameterTypeValue
                    }).ToListAsync();

                var tocParamGrouped = tocParamRaw
                    .GroupBy(x => new { x.RegulationSetupId, x.TOCRuleType })
                    .ToDictionary(
                        g => new { g.Key.RegulationSetupId, g.Key.TOCRuleType },
                        g => new
                        {
                            ParameterNames = string.Join(", ", g.Select(x => x.ParameterName).Distinct()),
                            ParameterValues = string.Join(", ", g.Select(x => x.ParameterValue).Distinct())
                        });

                // 2. Registration-based compliance (take only first per regulation)
                var registrationComplianceRaw = await (
                     from rsd in _context.RegulationSetupDetails
                     join c in _context.Country on rsd.CountryId equals c.Id
                     join rg in _context.RegulationGroups on rsd.RegulationGroupId equals rg.Id
                     join tocReg in _context.TOCRegistration on rsd.Id equals tocReg.RegulationSetupId
                     join rsc in _context.RegulationSetupCompliance on rsd.Id equals rsc.RegulationSetupId into rscJoin
                     from rsc in rscJoin.DefaultIfEmpty()
                     join user in _context.Users on tocReg.CreatedBy equals user.Id
                     into userJoin
                     from user in userJoin.DefaultIfEmpty()
                     join manger in _context.Users on tocReg.ModifiedBy equals manger.Id
                    into mangerJoin
                     from manger in mangerJoin.DefaultIfEmpty()
                     select new
                     {
                         rsd.Id,
                         rsd.RegulationSetupDetailsReferenceCode,
                         rsd.RegulationName,
                         rg.RegulationGroupName,
                         c.CountryName,
                         tocReg.RegulationSetupTypeOfComplianceRegisterRC,
                         tocReg.RegistrationName,
                         tocReg.SectionNameOfRegister,
                         tocReg.CreatedOn,
                         rg.Status,
                         rsc.RegulationSetupComplianceReferenceCode,
                         rsc.ComplianceName,
                         user.FullName,
                         approvedby = manger.FullName
                     }).ToListAsync();

                var registrationCompliance = registrationComplianceRaw
                    .GroupBy(x => x.Id)
                    .Select(g =>
                    {
                        var x = g.First(); // take only the first registration per regulation
                        var key = new { RegulationSetupId = x.Id, TOCRuleType = "Registration" };
                        tocParamGrouped.TryGetValue(key, out var paramGroup);

                        return new RegulationComplianceWithCountryModel
                        {
                            Country = x.CountryName,
                            RegulationCode = x.RegulationSetupDetailsReferenceCode,
                            RegulationName = x.RegulationName,
                            RegulationGroupName = x.RegulationGroupName,
                            ComplianceCode = x.RegulationSetupTypeOfComplianceRegisterRC ?? string.Empty,
                            ComplianceName = x.RegistrationName ?? string.Empty,
                            Compliancetype = "Registration",
                            ComplianceSection = x.SectionNameOfRegister ?? string.Empty,
                            Compliancestatus = x.Status,
                            ComplianceEffectiveDate = null,
                            ComplianceInactiveDate = null,
                            ApplicableByParameter = paramGroup?.ParameterNames ?? string.Empty,
                            parametervalue = paramGroup?.ParameterValues ?? string.Empty,
                            ParentComplaincecode = x.RegulationSetupComplianceReferenceCode ?? string.Empty,
                            ParentComplianceName = x.ComplianceName ?? string.Empty,
                            CreatedOn = x.CreatedOn,
                            AddedBy = x.FullName,
                            ApprovedBy = x.FullName
                        };
                    }).ToList();
                // 3. Dues-based compliance
                var duesComplianceRaw = await (
                    from rsd in _context.RegulationSetupDetails
                    join c in _context.Country on rsd.CountryId equals c.Id
                    join rg in _context.RegulationGroups on rsd.RegulationGroupId equals rg.Id
                    join dues in _context.TOCDues on rsd.Id equals dues.RegulationSetupId
                    join rsc in _context.RegulationSetupCompliance on rsd.Id equals rsc.RegulationSetupId into rscJoin
                    from rsc in rscJoin.DefaultIfEmpty()
                    join user in _context.Users on dues.CreatedBy equals user.Id
                    into userJoin
                    from user in userJoin.DefaultIfEmpty()
                    join manger in _context.Users on dues.ModifiedBy equals manger.Id
                   into mangerJoin
                    from manger in mangerJoin.DefaultIfEmpty()
                    select new
                    {
                        rsd.Id,
                        rsd.RegulationSetupDetailsReferenceCode,
                        rsd.RegulationName,
                        rg.RegulationGroupName,
                        c.CountryName,
                        dues.DuesReferenceCode,
                        dues.SectionNameofDues,
                        dues.TOCRuleType,
                        dues.CreatedOn,
                        rsc.RegulationSetupComplianceReferenceCode,
                        rsc.ComplianceName,
                        rsc.Status,
                        user.FullName,
                        approvedBy = manger.FullName
                    }).ToListAsync();

                var duesCompliance = duesComplianceRaw.Select(x =>
                {
                    var key = new { RegulationSetupId = x.Id, TOCRuleType = x.TOCRuleType };
                    tocParamGrouped.TryGetValue(key, out var paramGroup);

                    return new RegulationComplianceWithCountryModel
                    {
                        Country = x.CountryName,
                        RegulationCode = x.RegulationSetupDetailsReferenceCode,
                        RegulationName = x.RegulationName,
                        RegulationGroupName = x.RegulationGroupName,
                        ComplianceCode = x.DuesReferenceCode ?? string.Empty,
                        ComplianceSection = x.SectionNameofDues ?? string.Empty,
                        Compliancetype = x.TOCRuleType ?? string.Empty,
                        Compliancestatus = x.Status,
                        ComplianceEffectiveDate = null,
                        ComplianceInactiveDate = null,
                        ApplicableByParameter = paramGroup?.ParameterNames ?? string.Empty,
                        parametervalue = paramGroup?.ParameterValues ?? string.Empty,
                        ParentComplaincecode = x.RegulationSetupComplianceReferenceCode ?? string.Empty,
                        ParentComplianceName = x.ComplianceName ?? string.Empty,
                        ComplianceName = x.ComplianceName ?? string.Empty,
                        CreatedOn = x.CreatedOn,
                        AddedBy = x.FullName,
                        ApprovedBy = x.approvedBy
                    };
                }).ToList();

                // 4. Combine and return
                return registrationCompliance.Concat(duesCompliance).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving all compliance with country");
                return new List<RegulationComplianceWithCountryModel>();
            }
        }

        public async Task<List<EntityTypeModel>> GetAllEntityTypes()
        {
            var entityDetails = await (
            from e in _context.Entity
            join c in _context.Country on e.CountryId equals c.Id
            join et in _context.EntityType on e.EntityTypeId equals et.Id
            join addedUser in _context.Users on e.CreatedBy equals addedUser.Id
            join approvedUser in _context.Users on e.ModifiedBy equals approvedUser.Id into approvedJoin
            from approvedUser in approvedJoin.DefaultIfEmpty()
            select new EntityTypeModel
            {
                CountryCode = c.CountryCode,
                CountryName = c.CountryName,
                StatusId = et.Status,
                CreatedOn = e.CreatedOn,
                InactiveDate = null,
                EntityTypeCode = et.EntityTypeCode,
                EntityTypeName = et.EntityType,
                CreatedByName = addedUser.FullName,
                ApprovedBy = approvedUser.FullName,
                EntityTypeReferenceCode = et.EntityTypeReferenceCode
            }).ToListAsync();
            return entityDetails;
        }



        public async Task<List<IndustryrMapping>> GetIndustryCountryMapping()
        {
            var result = await (

                    from im in _context.IndustryrMapping
                    join c in _context.Country on im.CountryId equals c.Id
                    join maj in _context.MajorIndustries on im.MajorIndustryId equals maj.Id
                    join min in _context.MinorIndustries on im.MinorIndustryId equals min.Id
                    join addedUser in _context.Users on im.CreatedBy equals addedUser.Id
                    join approvedUser in _context.Users on im.ModifiedBy equals approvedUser.Id into approvedJoin
                    from approvedUser in approvedJoin.DefaultIfEmpty()
                    select new IndustryrMapping
                    {
                        CountryCode = c.CountryCode,
                        CountryName = c.CountryName,
                        Status = im.Status,
                        CreatedOn = im.CreatedOn,
                        InactivatedDate = null,
                        MajorIndustryName = maj.MajorIndustryName,
                        MajorIndustryCode = maj.MajorIndustryCode,
                        MinorIndustryName = min.MinorIndustryName,
                        MinorIndustryCode = min.MinorIndustryCode,
                        CreatedByName = addedUser.FullName,
                        ApprovedByName = approvedUser.FullName,
                        MajorIndustryReferenceCode = maj.MajorIndustryReferenceCode
                    }).ToListAsync();

            return result;

        }
    }
}
