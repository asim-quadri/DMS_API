using ComplianceAPI.Models;
using ComplianceAPI.Models.DataModels;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using Organization = ComplianceAPI.Models.DataModels.Organization;
namespace ComplianceAPI.Repository
{
    public interface IDmsService
    {
        public Task<object> getcomseqdata();
    }
    public class DmsService : IDmsService
    {
        private readonly ComplianceDbContext dbContext;
        private readonly string _connectionString;

        public DmsService(ComplianceDbContext dbContext)
        {
            this.dbContext = dbContext;
            _connectionString = dbContext.Database.GetDbConnection().ConnectionString;
        }

        private IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
        public async Task<object> getcomseqdata()
        {
            var reg = await GetRegSetupHistory();
            return new { regulation=reg,organization=await GetOrgEntityLists(),
                announcement = reg
            };
        }

        public async Task<List<ReuglationListModle>> GetRegSetupHistory()
        {
                try
                {
                    // Fetch data from the database
                    var regulations = await dbContext.RegulationSetupDetails.ToListAsync();
                    var compliances = await dbContext.RegulationSetupCompliance.ToListAsync();
                    var tocRegistration = await dbContext.TOCRegistration.ToListAsync();
                    var tocImprisonments = await dbContext.TOCImprisonment.ToListAsync();
                    var tocIntrestPenalities = await dbContext.TOCIntrestPenality.ToListAsync();
                    var tocDues = await dbContext.TOCDues.ToListAsync();
                    var tocParameters = await dbContext.TOCParameter.ToListAsync();

                    // Helper method to construct TOC list
                    List<TOCListModel> GetRegulationTOCList(long regulationSetupId)
                    {
                        var list = tocImprisonments
                            .Where(t => t.RegulationSetupId == regulationSetupId)
                            .Select(t => new TOCListModel { Id = t.Id, TypeOfComplianceName = t.TOCRuleType, RuleType = t.TOCRuleType, TypeOfComplianceUID = t.UID, TypeOfComplianceId = t.ComplianceId })
                            .Union(tocRegistration.Where(t => t.RegulationSetupId == regulationSetupId)
                                .Select(t => new TOCListModel { Id = t.Id, TypeOfComplianceName = "Registration", RuleType = "Registration", TypeOfComplianceUID = t.UID, TypeOfComplianceId = t.ComplianceId }))
                            .Union(tocIntrestPenalities.Where(t => t.RegulationSetupId == regulationSetupId)
                                .Select(t => new TOCListModel { Id = t.Id, TypeOfComplianceName = t.TOCRuleType, RuleType = t.TOCRuleType, TypeOfComplianceUID = t.UID, TypeOfComplianceId = t.ComplianceId }))
                            .Union(tocDues.Where(t => t.RegulationSetupId == regulationSetupId)
                                .Select(t => new TOCListModel { Id = t.Id, TypeOfComplianceName = t.TOCRuleType, RuleType = t.TOCRuleType, TypeOfComplianceUID = t.UID, TypeOfComplianceId = t.ComplianceId }))
                            .Union(tocParameters.Where(t => t.RegulationSetupId == regulationSetupId)
                                .Select(t => new TOCListModel { Id = t.Id, TypeOfComplianceName = t.TOCRuleType, RuleType = t.TOCRuleType, TypeOfComplianceUID = t.UID, TypeOfComplianceId = t.ComplianceId }))

                            .GroupBy(t => new { t.TypeOfComplianceName, t.RuleType })
                            .Select(g => g.First())
                            .ToList();
                        return list;
                    }

                    // Helper method to construct TOC list
                    List<TOCListModel> GetTOCList(long complianceId)
                    {
                        return tocImprisonments
                            .Where(t => t.ComplianceId == complianceId)
                            .Select(t => new TOCListModel { Id = t.Id, TypeOfComplianceName = t.TOCRuleType, RuleType = t.TOCRuleType, TypeOfComplianceUID = t.UID, TypeOfComplianceId = t.ComplianceId })
                            .Union(tocIntrestPenalities.Where(t => t.ComplianceId == complianceId)
                                .Select(t => new TOCListModel { Id = t.Id, TypeOfComplianceName = t.TOCRuleType, RuleType = t.TOCRuleType, TypeOfComplianceUID = t.UID, TypeOfComplianceId = t.ComplianceId }))
                            .Union(tocDues.Where(t => t.ComplianceId == complianceId)
                                .Select(t => new TOCListModel { Id = t.Id, TypeOfComplianceName = t.TOCRuleType, RuleType = t.TOCRuleType, TypeOfComplianceUID = t.UID, TypeOfComplianceId = t.ComplianceId }))
                            .Union(tocParameters.Where(t => t.ComplianceId == complianceId)
                                .Select(t => new TOCListModel { Id = t.Id, TypeOfComplianceName = t.TOCRuleType, RuleType = t.TOCRuleType, TypeOfComplianceUID = t.UID, TypeOfComplianceId = t.ComplianceId }))
                            .Union(tocRegistration.Where(t => t.ComplianceId == complianceId)
                                .Select(t => new TOCListModel { Id = t.Id, TypeOfComplianceName = "Registration", RuleType = "Registration", TypeOfComplianceUID = t.UID, TypeOfComplianceId = t.ComplianceId }))
                            .GroupBy(t => new { t.TypeOfComplianceName, t.RuleType })
                            .Select(g => g.First())
                            .ToList();
                    }

                    // Recursive method to get compliances
                    List<ComplianceListModel> GetCompliances(long regulationId)
                    {
                        return compliances
                            .Where(c => c.RegulationSetupId == regulationId)
                            .Select(c => new ComplianceListModel
                            {
                                Id = c.Id,
                                ComplianceName = c.ComplianceName,
                                ComplianceUID = c.UID,
                                RuleType = "Compliance",
                                TOC = GetTOCList(c.Id),
                                Compliance = c.Id == null || c.Id == 0 ? new List<ComplianceListModel>() : GetChildCompliances((long)c.Id),
                                ParentComplianceId = c.ParentComplianceId
                            }).ToList();
                    }

                    // Recursive method to get child compliances
                    List<ComplianceListModel> GetChildCompliances(long parentId)
                    {
                        return compliances
                            .Where(c => c.ParentComplianceId == parentId)
                            .Select(c => new ComplianceListModel
                            {
                                Id = c.Id,
                                ComplianceName = c.ComplianceName,
                                ComplianceUID = c.UID,
                                RuleType = "Compliance",
                                TOC = GetTOCList(c.Id),
                                Compliance = c.Id == null || c.Id == 0 ? new List<ComplianceListModel>() : GetChildCompliances((long)c.Id),
                                ParentComplianceId = c.ParentComplianceId
                            }).ToList();
                    }

                    // Construct the result list
                    var result = regulations.Select(r => new ReuglationListModle
                    {
                        Id = r.Id,
                        RegulationSetupUID = r.UID,
                        RegulationName = r.RegulationName,
                        majorIndustryId = r.MajorIndustryId,
                        RuleType = "Regulation",
                        RegulationSetupDetailsReferenceCode = r.RegulationSetupDetailsReferenceCode,
                        Compliance = r.Id == null || r.Id == 0 ? new List<ComplianceListModel>() : GetCompliances((long)r.Id),
                        TOC = r.Id == null ? new List<TOCListModel>() : GetRegulationTOCList((long)r.Id),
                        IsParameterChecked = r.IsParameterChecked
                    }).ToList();

                    return result;
                }
                catch (Exception ex)
                {
                    // Log or handle the exception as needed
                    throw new ApplicationException("An error occurred while fetching regulations.", ex);
                }
            }

        public async Task<List<OrganizationEntityList>> GetOrgEntityLists()
        {
            try
            {
                List<Organization> organizations;

                organizations = await dbContext.Organizations.Where(a => a.Status == 2).ToListAsync();

                var entities = await dbContext.Entity.ToListAsync();

                List<EntityList> GetEntities(long orgId)
                {
                    return entities
                        .Where(c => c.OrganizationId == orgId && c.Status == 2)
                        .Select(c => new EntityList
                        {
                            EntityId = c.Id,
                            EntityName = c.EntityName
                        }).ToList();
                }

                var result = organizations.Select(r => new OrganizationEntityList
                {
                    Id = r.Id,
                    OrganizationName = r.OrganizationName,
                    IsOrganization = r.BillingLevelId == 1 ? true : false,
                    EntityList = GetEntities(r.Id)
                }).ToList();
                return result;
            }
            catch (Exception ex)
            {
                return new List<OrganizationEntityList>();
            }
        }
        private static long SafeLong(dynamic val)
        {
            if (val == null || val is DBNull) return 0;
            return Convert.ToInt64(val);
        }

    }
}
