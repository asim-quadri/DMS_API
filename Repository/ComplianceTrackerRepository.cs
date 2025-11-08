using ComplianceAPI.Models;
using ComplianceAPI.Models.DataModels;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ComplianceAPI.Repository
{
    public interface IComplianceTrackerRepository
    {
        Task<List<ComplianceRegulationGroupModel>> GetAllRegulationGroups();

        Task<List<TOCListModel>> GetTOCList(long complianceId);

        Task<List<TOCListModel>> GetRegulationTOCList(long regulationId);

        Task<List<RegulationListModel>> GetRegComplianceDetails();

        Task<List<RegulationListModel>> GetRegComplianceDetailsByEntityId(long entityId);

        Task<List<Models.Entity>> GetEntities();

        Task<Models.Entity> GetEntityById(long? entityId);

        Task<bool> PostComplianceTracker(ComplianceTracker complianceList);

        Task<List<RegulationSetupCompliance>> GetAllCompliances();
    }

    public class ComplianceTrackerRepository : IComplianceTrackerRepository
    {
        protected readonly IUnitOfWork _unitOfWork;
        public readonly ComplianceDbContext _dbContext;

        public ComplianceTrackerRepository(ComplianceDbContext dbContext, IUnitOfWork unitOfWork)
        {
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<ComplianceRegulationGroupModel>> GetAllRegulationGroups()
        {
            using (var connection = _unitOfWork.ConnectionFactory())
            {
                var mul = await connection.QueryMultipleAsync("[product_owner].USP_GETALLREGULATIONGROUP", commandType: System.Data.CommandType.StoredProcedure);
                var result = mul.Read<ComplianceRegulationGroupModel>().ToList();
                mul.Dispose();
                return result;
            }
        }

        public async Task<List<TOCListModel>> GetTOCList(long complianceId)
        {
            var res = _dbContext.TOCImprisonment.Where(w => w.ComplianceId == complianceId).Select(s => new TOCListModel()
            {
                Id = s.Id,
                RuleType = s.TOCRuleType,
                TypeOfComplianceName = s.TOCRuleType,
            }).Union(_dbContext.TOCIntrestPenality.Where(w => w.ComplianceId == complianceId).Select(s => new TOCListModel()
            {
                Id = s.Id,
                RuleType = s.TOCRuleType,
                TypeOfComplianceName = s.TOCRuleType
            })).Union(_dbContext.TOCDues.Where(w => w.ComplianceId == complianceId).Select(s => new TOCListModel()
            {
                Id = s.Id,
                RuleType = s.TOCRuleType,
                TypeOfComplianceName = s.TOCRuleType,
            })).Union(_dbContext.TOCParameter.Where(w => w.ComplianceId == complianceId).Select(s => new TOCListModel()
            {
                Id = s.Id,
                RuleType = s.TOCRuleType,
                TypeOfComplianceName = s.TOCRuleType,
            })
            ).Union(_dbContext.TOCRegistration.Where(w => w.ComplianceId == complianceId).Select(s => new TOCListModel()
            {
                TypeOfComplianceName = "Registration",
                Id = s.Id,
                RuleType = s.RegistrationName
            }))

            .GroupBy(g => new { g.TypeOfComplianceName, g.RuleType }).Select(s => s.First()).ToList();

            return res;
        }

        public async Task<List<TOCListModel>> GetRegulationTOCList(long regulationId)
        {
            // Fetch data from the database
            var regulations = await _dbContext.RegulationSetupDetails.ToListAsync();
            var compliances = await _dbContext.RegulationSetupCompliance.ToListAsync();
            var tocRegistration = await _dbContext.TOCRegistration.ToListAsync();
            var tocImprisonments = await _dbContext.TOCImprisonment.ToListAsync();
            var tocIntrestPenalities = await _dbContext.TOCIntrestPenality.ToListAsync();
            var tocDues = await _dbContext.TOCDues.ToListAsync();
            var tocParameters = await _dbContext.TOCParameter.ToListAsync();

            var res = tocImprisonments.Where(w => w.ComplianceId == regulationId).Select(s => new TOCListModel()
            {
                Id = s.Id,
                RuleType = s.TOCRuleType,
                TypeOfComplianceName = s.TOCRuleType,
            }).Union(tocIntrestPenalities.Where(w => w.ComplianceId == regulationId).Select(s => new TOCListModel()
            {
                Id = s.Id,
                RuleType = s.TOCRuleType,
                TypeOfComplianceName = s.TOCRuleType
            })).Union(tocDues.Where(w => w.ComplianceId == regulationId).Select(s => new TOCListModel()
            {
                Id = s.Id,
                RuleType = s.TOCRuleType,
                TypeOfComplianceName = s.TOCRuleType,
            })).Union(tocParameters.Where(w => w.ComplianceId == regulationId).Select(s => new TOCListModel()
            {
                Id = s.Id,
                RuleType = s.TOCRuleType,
                TypeOfComplianceName = s.TOCRuleType,
            })
            ).Union(tocRegistration.Where(w => w.ComplianceId == regulationId).Select(s => new TOCListModel()
            {
                TypeOfComplianceName = "Registration",
                Id = s.Id,
                RuleType = s.RegistrationName
            }))
            .GroupBy(g => new { g.TypeOfComplianceName, g.RuleType }).Select(s => s.First()).ToList();

            return res;
        }

        public async Task<List<RegulationListModel>> GetRegComplianceDetails()
        {
            try
            {
                // Fetch data from the database
                var regulations = await _dbContext.RegulationSetupDetails.ToListAsync();
                var compliances = await _dbContext.RegulationSetupCompliance.ToListAsync();
                var tocRegistration = await _dbContext.TOCRegistration.ToListAsync();
                var tocImprisonments = await _dbContext.TOCImprisonment.ToListAsync();
                var tocIntrestPenalities = await _dbContext.TOCIntrestPenality.ToListAsync();
                var tocDues = await _dbContext.TOCDues.ToListAsync();
                var tocParameters = await _dbContext.TOCParameter.ToListAsync();

                // Helper method to construct TOC list
                List<TOCListModel> GetRegulationTOCList(long regulationSetupId)
                {
                    return tocImprisonments
                        .Where(t => t.RegulationSetupId == regulationSetupId)
                        .Select(t => new TOCListModel { Id = t.Id, TypeOfComplianceName = t.TOCRuleType, RuleType = t.TOCRuleType, TypeOfComplianceUID = t.UID, TypeOfComplianceId = t.ComplianceId })
                        .Union(tocIntrestPenalities.Where(t => t.RegulationSetupId == regulationSetupId)
                            .Select(t => new TOCListModel { Id = t.Id, TypeOfComplianceName = t.TOCRuleType, RuleType = t.TOCRuleType, TypeOfComplianceUID = t.UID, TypeOfComplianceId = t.ComplianceId }))
                        .Union(tocDues.Where(t => t.RegulationSetupId == regulationSetupId)
                            .Select(t => new TOCListModel { Id = t.Id, TypeOfComplianceName = t.TOCRuleType, RuleType = t.TOCRuleType, TypeOfComplianceUID = t.UID, TypeOfComplianceId = t.ComplianceId }))
                        .Union(tocParameters.Where(t => t.RegulationSetupId == regulationSetupId)
                            .Select(t => new TOCListModel { Id = t.Id, TypeOfComplianceName = t.TOCRuleType, RuleType = t.TOCRuleType, TypeOfComplianceUID = t.UID, TypeOfComplianceId = t.ComplianceId }))
                        .Union(tocRegistration.Where(t => t.RegulationSetupId == regulationSetupId)
                            .Select(t => new TOCListModel { Id = t.Id, TypeOfComplianceName = "Registration", RuleType = "Registration", TypeOfComplianceUID = t.UID, TypeOfComplianceId = t.ComplianceId }))
                        .GroupBy(t => new { t.TypeOfComplianceName, t.RuleType })
                        .Select(g => g.First())
                        .ToList();
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
                List<ListComplianceModel> GetCompliances(long regulationId)
                {
                    return compliances
                        .Where(c => c.RegulationSetupId == regulationId)
                        .Select(c => new ListComplianceModel
                        {
                            Id = c.Id,
                            ComplianceName = c.ComplianceName,
                            ComplianceUID = c.UID,
                            RuleType = "Compliance",
                            TOC = GetTOCList(c.Id),
                            Compliance = c.Id == null || c.Id == 0 ? new List<ListComplianceModel>() : GetChildCompliances((long)c.Id),
                            ParentComplianceId = c.ParentComplianceId
                        }).ToList();
                }

                // Recursive method to get child compliances
                List<ListComplianceModel> GetChildCompliances(long parentId)
                {
                    return compliances
                        .Where(c => c.ParentComplianceId == parentId)
                        .Select(c => new ListComplianceModel
                        {
                            Id = c.Id,
                            ComplianceName = c.ComplianceName,
                            ComplianceUID = c.UID,
                            RuleType = "Compliance",
                            TOC = GetTOCList(c.Id),
                            Compliance = c.Id == null || c.Id == 0 ? new List<ListComplianceModel>() : GetChildCompliances((long)c.Id),
                            ParentComplianceId = c.ParentComplianceId
                        }).ToList();
                }

                // Construct the result list
                var result = regulations.Select(r => new RegulationListModel
                {
                    Id = r.Id,
                    RegulationSetupUID = r.UID,
                    RegulationName = r.RegulationName,
                    RuleType = "Regulation",
                    Compliance = r.Id == null || r.Id == 0 ? new List<ListComplianceModel>() : GetCompliances((long)r.Id),
                    TOC = r.Id == null ? new List<TOCListModel>() : GetRegulationTOCList((long)r.Id)
                }).ToList();

                return result;
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                throw new ApplicationException("An error occurred while fetching regulations.", ex);
            }

            //return await dbContext.RegulationSetupHistory.ToListAsync();
        }

        public async Task<List<Models.Entity>> GetEntities()
        {
            try
            {
                using (var connection = _unitOfWork.ConnectionFactory())
                {
                    var parameters = new { };
                    var mul = await connection.QueryMultipleAsync("[client].[USP_GETALLENTITYDETAILS]", parameters, commandType: System.Data.CommandType.StoredProcedure);
                    var entity = mul.Read<Models.Entity>().ToList();
                    mul.Dispose();
                    //List<Models.DataModels.Entity> entity = new List<Models.DataModels.Entity>();
                    //entity = await _dbContext.Entity.ToListAsync();
                    return entity;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Models.Entity> GetEntityById(long? entityId)
        {
            try
            {
                using (var connection = _unitOfWork.ConnectionFactory())
                {
                    var parameters = new { entityId = entityId };
                    var mul = await connection.QueryMultipleAsync("[client].[USP_GETALLENTITYDETAILS]", parameters, commandType: System.Data.CommandType.StoredProcedure);
                    var entity = mul.Read<Models.Entity>().FirstOrDefault();
                    mul.Dispose();
                    //List<Models.DataModels.Entity> entity = new List<Models.DataModels.Entity>();
                    //entity = await _dbContext.Entity.ToListAsync();
                    return entity;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<RegulationListModel>> GetRegComplianceDetailsByEntityId(long entityId)
        {
            try
            {
                var compliancetracker = new List<ComplianceTracker>();
                using (var connection = _unitOfWork.ConnectionFactory())
                {
                    var parameters = new { EntityId = entityId };
                    var mul = await connection.QueryMultipleAsync("[dbo].USP_GETCOMPLIANCETRACKERLIST", parameters, commandType: System.Data.CommandType.StoredProcedure);
                    compliancetracker = mul.Read<ComplianceTracker>().ToList();
                    mul.Dispose();
                }

                // Fetch data from the database
                var regulations = await _dbContext.RegulationSetupDetails.ToListAsync();
                var compliances = await _dbContext.RegulationSetupCompliance.ToListAsync();
                var tocRegistration = await _dbContext.TOCRegistration.ToListAsync();
                var tocImprisonments = await _dbContext.TOCImprisonment.ToListAsync();
                var tocIntrestPenalities = await _dbContext.TOCIntrestPenality.ToListAsync();
                var tocDues = await _dbContext.TOCDues.ToListAsync();
                var tocParameters = await _dbContext.TOCParameter.ToListAsync();

                // Helper method to construct TOC list
                List<TOCListModel> GetRegulationTOCList(long regulationSetupId)
                {
                    return tocImprisonments
                        .Where(t => t.RegulationSetupId == regulationSetupId)
                        .Select(t => new TOCListModel { Id = t.Id, TypeOfComplianceName = t.TOCRuleType, RuleType = t.TOCRuleType, TypeOfComplianceUID = t.UID, TypeOfComplianceId = t.ComplianceId })
                        .Union(tocIntrestPenalities.Where(t => t.RegulationSetupId == regulationSetupId)
                            .Select(t => new TOCListModel { Id = t.Id, TypeOfComplianceName = t.TOCRuleType, RuleType = t.TOCRuleType, TypeOfComplianceUID = t.UID, TypeOfComplianceId = t.ComplianceId }))
                        .Union(tocDues.Where(t => t.RegulationSetupId == regulationSetupId)
                            .Select(t => new TOCListModel { Id = t.Id, TypeOfComplianceName = t.TOCRuleType, RuleType = t.TOCRuleType, TypeOfComplianceUID = t.UID, TypeOfComplianceId = t.ComplianceId }))
                        .Union(tocParameters.Where(t => t.RegulationSetupId == regulationSetupId)
                            .Select(t => new TOCListModel { Id = t.Id, TypeOfComplianceName = t.TOCRuleType, RuleType = t.TOCRuleType, TypeOfComplianceUID = t.UID, TypeOfComplianceId = t.ComplianceId }))
                        .Union(tocRegistration.Where(t => t.RegulationSetupId == regulationSetupId)
                            .Select(t => new TOCListModel { Id = t.Id, TypeOfComplianceName = "Registration", RuleType = "Registration", TypeOfComplianceUID = t.UID, TypeOfComplianceId = t.ComplianceId }))
                        .GroupBy(t => new { t.TypeOfComplianceName, t.RuleType })
                        .Select(g => g.First())
                        .ToList();
                }

                List<ComplianceTracker> GetRegulationCompianceList(long regulationSetupId)
                {
                    var compliancetracker = new List<ComplianceTracker>();
                    using (var connection = _unitOfWork.ConnectionFactory())
                    {
                        //Task task = Task.Run(async() =>
                        //{
                        //var parameters = new { RegulationId = regulationSetupId };
                        //var mul = await connection.QueryMultipleAsync("[dbo].USP_GETOCLISTBYREGID", parameters, commandType: System.Data.CommandType.StoredProcedure);
                        //compliancetracker = mul.Read<ComplianceTracker>().ToList();
                        //mul.Dispose();
                        //});
                        //task.Wait();

                        compliancetracker = (from ti in tocImprisonments
                                             join tip in tocIntrestPenalities on ti.RegulationSetupId equals tip.RegulationSetupId
                                             join td in tocDues on ti.RegulationSetupId equals td.RegulationSetupId
                                             //join tp in tocParameters on ti.RegulationSetupId equals tp.RegulationSetupId
                                             //join tr in tocRegistration on ti.RegulationSetupId equals tr.RegulationSetupId
                                             where ti.RegulationSetupId == regulationSetupId
                                             select new ComplianceTracker()
                                             {
                                                 TOCRuleType = ti.TOCRuleType,
                                                 RegulationSetupId = regulationSetupId,
                                                 DueDate = Convert.ToDateTime(td.DueDate),
                                                 Frequency = td.Frequency,
                                                 ComplianceId = Convert.ToInt32(ti.ComplianceId),
                                                 ForTheMonth = td.ForTheMonth
                                             }).ToList();

                        return compliancetracker;
                    }
                }

                List<ComplianceTracker> GetTOCListByComplianceId(long complianceId)
                {
                    var compliancetracker = new List<ComplianceTracker>();
                    using (var connection = _unitOfWork.ConnectionFactory())
                    {
                        //Task task = Task.Run(async() =>
                        //{
                        //    var parameters = new { ComplianceId = complianceId };
                        //    var mul = await connection.QueryMultipleAsync("[dbo].USP_GETOCLISTBYCOMPLIANCEID", parameters, commandType: System.Data.CommandType.StoredProcedure);
                        //    compliancetracker = mul.Read<ComplianceTracker>().ToList();
                        //    mul.Dispose();
                        //});
                        //task.Wait();
                        compliancetracker = (from ti in tocImprisonments
                                             join tip in tocIntrestPenalities on ti.ComplianceId equals tip.ComplianceId
                                             join td in tocDues on ti.ComplianceId equals td.ComplianceId
                                             //join tp in tocParameters on ti.ComplianceId equals tp.ComplianceId
                                             //join tr in tocRegistration on ti.ComplianceId equals tr.ComplianceId
                                             where ti.ComplianceId == complianceId
                                             select new ComplianceTracker()
                                             {
                                                 TOCRuleType = ti.TOCRuleType,
                                                 RegulationSetupId = Convert.ToInt32(ti.RegulationSetupId),
                                                 DueDate = Convert.ToDateTime(td.DueDate),
                                                 Frequency = td.Frequency,
                                                 ComplianceId = complianceId,
                                                 ForTheMonth = td.ForTheMonth
                                             }).ToList();

                        return compliancetracker;
                    }
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
                List<ListComplianceModel> GetCompliances(long regulationId)
                {
                    return compliances
                        .Where(c => c.RegulationSetupId == regulationId)
                        .Select(c => new ListComplianceModel
                        {
                            Id = c.Id,
                            ComplianceName = c.ComplianceName,
                            ComplianceUID = c.UID,
                            RuleType = "Compliance",
                            TOC = GetTOCList(c.Id),
                            Compliance = c.Id == null || c.Id == 0 ? new List<ListComplianceModel>() : GetChildCompliances((long)c.Id),
                            ParentComplianceId = c.ParentComplianceId,
                            ComplianceTrackers = c.Id == null || c.Id == 0 ? new List<ComplianceTracker>() : GetTOCListByComplianceId((long)c.Id),
                        }).ToList();
                }

                // Recursive method to get child compliances
                List<ListComplianceModel> GetChildCompliances(long parentId)
                {
                    return compliances
                        .Where(c => c.ParentComplianceId == parentId)
                        .Select(c => new ListComplianceModel
                        {
                            Id = c.Id,
                            ComplianceName = c.ComplianceName,
                            ComplianceUID = c.UID,
                            RuleType = "Compliance",
                            TOC = GetTOCList(c.Id),
                            Compliance = c.Id == null || c.Id == 0 ? new List<ListComplianceModel>() : GetChildCompliances((long)c.Id),
                            ParentComplianceId = c.ParentComplianceId,
                            ComplianceTrackers = c.Id == null || c.Id == 0 ? new List<ComplianceTracker>() : GetTOCListByComplianceId((long)c.Id),
                        }).ToList();
                }

                // Construct the result list
                var result = regulations.Join(compliancetracker, reg => reg.Id, comp => comp.RegulationSetupId, (reg, comp) => new { Regulations = reg, ComplianceTrackers = comp }).Select(r => new RegulationListModel
                {
                    Id = r.Regulations.Id,
                    RegulationSetupUID = r.Regulations.UID,
                    RegulationName = r.Regulations.RegulationName,
                    RuleType = "Regulation",
                    Compliance = r.Regulations.Id == null || r.Regulations.Id == 0 ? new List<ListComplianceModel>() : GetCompliances((long)r.Regulations.Id),
                    TOC = r.Regulations.Id == null ? new List<TOCListModel>() : GetRegulationTOCList((long)r.Regulations.Id),
                    ComplianceTrackers = r.Regulations.Id == null ? new List<ComplianceTracker>() : GetRegulationCompianceList((long)r.Regulations.Id)
                }).ToList();

                return result;
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                throw new ApplicationException("An error occurred while fetching regulations.", ex);
            }

            //return await dbContext.RegulationSetupHistory.ToListAsync();
        }

        public async Task<bool> PostComplianceTracker(ComplianceTracker compliance)
        {
            using (var sqlContext = _unitOfWork.ContextFactory())
            {
                try
                {
                    //foreach (var compliance in compliance)
                    //{
                    var inputdata = new
                    {
                        RegulationId = compliance.RegulationSetupId,
                        ComplianceId = compliance.ComplianceId,
                        Frequency = compliance.Frequency,
                        DueDate = compliance.DueDate,
                        DueAmount = compliance.DueAmount,
                        AmountPaid = compliance.AmountPaid,
                        ActualDate = compliance.ActualDate,
                        PayableAmount = compliance.PayableAmount,
                        EntityId = compliance.EntityId,
                        Reason = compliance.Reason
                    };

                    var obj = await sqlContext.Connection.QueryAsync<object>("[client].[USP_SAVECOMPLIANCETRACKER]", inputdata, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
                    sqlContext.Commit();
                    //}
                }
                catch (Exception ex)
                {
                    sqlContext.Rollback();
                    throw ex;
                }
                return true;
            }
        }

        public async Task<List<RegulationSetupCompliance>> GetAllCompliances()
        {
            return await _dbContext.RegulationSetupCompliance.Where(x => x.Status == 1).ToListAsync();
        }
    }
}