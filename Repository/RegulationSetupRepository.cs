using Azure;
using ComplianceAPI.Helpers;
using ComplianceAPI.Helpers.Constants;
using ComplianceAPI.Models;
using ComplianceAPI.Models.DataModels;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Data;
using ConcernedMinistry = ComplianceAPI.Models.DataModels.ConcernedMinistry;
using MinorIndustry = ComplianceAPI.Models.DataModels.MinorIndustry;
using RefApprovalStatus = ComplianceAPI.Helpers.RefApprovalStatus;
using RegulatoryAuthorities = ComplianceAPI.Models.DataModels.RegulatoryAuthorities;
using Response = ComplianceAPI.Models.Response;

namespace ComplianceAPI.Repository
{
    public interface IRegulationSetupRepository
    {
        Task<string> GetNextRegulationSetupCode();

        Task<string> GetNextRegulationSetupComplianceCode();

        Task<RegulationStupDetails> AdminAddRegulationStupDetails(RegulationStupDetails regulationStupDetails);

        //Task<RegulationSetupParameters> AdminAddRegulationStupParameters(AddRegulationStupParameters regulationStupParameters);
        Task<RegulationStupDetails> AddRegulationStupDetails(RegulationStupDetails regulationStupDetails);

        //Task<RegulationSetupParameters> AddRegulationStupParameters(AddRegulationStupParameters regulationStupParameters);
        Task<RegulationStupDetails> GetAllRegulationSetupDetails(Guid? regSetupuid);

        Task<List<RegulationSetupDetails>> GetAllRegulationSetupDetails();

        Task<List<ReuglationListModle>> GetRegSetupHistory();

        Task<RegulationStupDetails> GetHistoryRegulationSetup(Guid? Uid);

        Task<List<RegulationSetupParameter>> GetAllRegulationSetupParameter();

        Task<bool> PostUpdateRegulationStupApproval(AccessModel access);

        Task<List<PendingApproval>> GetPendingRegulationSetupApproval(Guid? UserUID);

        Task<bool> PostRegulationSetupApproveAccess(AccessModel access);

        Task<bool> PostRegulationSetupRejectAccess(AccessModel access);

        Task<bool> PostRegulationSetupReviewAccess(AccessModel access);

        Task<RegulationStupDetails> ApproveRegulationSetup(AccessModel access);

        Task<RegulationStupDetails> RejectRegulationSetup(AccessModel access);

        Task<List<MinorIndustry>> GetMinorIndustrybyMajorID(long majorIndustoryId);

        Task<List<IndustryrMapping>> GetMinorIndustrybyMajorIDMap(List<long> majorIndustoryId, long countryId);

        Task<List<IndustryrMapping>> GetTOBMinorIndustrybyMajorIDMap(List<long> majorIndustoryId, List<long> minorIndustoryId, long countryId);

        Task<List<IndustryMappings>> GetIndustryMapping(long countryId);

        Task<RegSetupComplianceModel> GetRegulationSetupCompliance(Guid? complianceuid);

        Task<RegSetupComplianceModel> AddRegSetupComplianceAsync(RegSetupComplianceModel compliance);

        Task<bool> PostUpdateRegComplianceApproval(AccessModel access);

        Task<GetRegulationBasicAndParameterByCountryID> GetAllRegulationBasicAndParameterByCountryID(int countryId);

        Task<RegSetupComplianceModel> GetRegSetupComplianceHistory(Guid? Uid);

        Task<RegSetupComplianceModel> ApproveRegSetupCompliance(AccessModel access);

        Task<RegSetupComplianceModel> RejectRegSetupCompliance(AccessModel access);

        Task<TOCRegistrationModel> GetTOCRegistration(long tocRegId, long? complianceId, long? regulationid, string ruleType);

        Task<TOCRegistrationModel> SaveTOCRegistrationAsync(TOCRegistrationModel dto);

        Task<TOCRules> GetTOCRulesAsync(long complianceId, long regulationid, string ruleType);

        Task<TOCRules> SaveTOCRulesAsync(TOCRules rule);

        Task<bool> TOCComplianceApproval(AccessModel access);

        Task<object> GetTOCHistory(long historyId);

        Task<Response> ApproveTOCRegistration(AccessModel access);

        Task<Response> RejectTOC(AccessModel access);

        Task<Response> ApproveTOCRule(AccessModel access);

        Task<bool> AddRegulationSetupApprovalNotification(long createdBy, string regulationSetupName);

        Task<List<RegulationListModel>> GetRegulationsByCountryIds(List<long> countryIds);

        Task<string> GetNextRegulationSetupTypeOfComplianceRegisterCode();

        Task<string> GetNextRegulationSetupTypeOfComplianceDuesCode();

        /// <summary>
        /// Get Type of compliances
        /// </summary>
        /// <returns></returns>
        Task<List<TypeOfCompliance>> GetTypeOfCompliances();

        Task<List<TOCRegistration>> GetAllTocRegister();

        Task<List<TOCDues>> GetAllTOCDuesAsync();
    }

    public class RegulationSetupRepository : IRegulationSetupRepository
    {
        private readonly ComplianceDbContext dbContext;
        private readonly IHelperRepository helperRepository;
        private readonly IUnitOfWork unitOfWork;

        public RegulationSetupRepository(ComplianceDbContext dbContext, IHelperRepository helperRepository, IUnitOfWork unitOfWork)
        {
            this.dbContext = dbContext;
            this.helperRepository = helperRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<RegulationStupDetails> AdminAddRegulationStupDetails(RegulationStupDetails regulationStupDetails)
        {
            int superAdminCount = 0;
            string roleName = "";
            string message = "Successfully Saved";
            ComplianceAPI.Models.DataModels.RegulationSetupDetails regulationsetupdeatilobject = null;
            regulationsetupdeatilobject = new ComplianceAPI.Models.DataModels.RegulationSetupDetails
            {
                //EmpId = regulationStupDetails.CreatedBy.ToString(),
                RegulationType = regulationStupDetails.RegulationType,
                CountryId = regulationStupDetails.CountryId,
                StateId = regulationStupDetails.StateId,
                RegulationName = regulationStupDetails.RegulationName,
                RegulationGroupId = regulationStupDetails.RegulationGroupId,
                MajorIndustryId = regulationStupDetails.MajorIndustryId,
                MinorIndustryId = regulationStupDetails.MinorIndustryId,
                EntityTypeId = regulationStupDetails.EntityTypeId,
                Description = regulationStupDetails.Description,
                Status = 0,
                // ManagerId = regulationStupDetails.ManagerId,
                CreatedOn = DateTime.Now,
                CreatedBy = regulationStupDetails.CreatedBy,
            };
            dbContext.RegulationSetupDetails.Add(regulationsetupdeatilobject);
            dbContext.SaveChanges();

            return dbContext.RegulationSetupDetails
                .Where(p => p.Id == regulationsetupdeatilobject.Id)
                .Select(p => new RegulationStupDetails
                {
                    //Id = p.Id,
                    //EmpId = p.EmpId,
                    CountryId = p.CountryId,
                    StateId = p.StateId,
                    RegulationGroupId = p.RegulationGroupId,
                    Description = p.Description,
                    RegulationName = p.RegulationName,
                    RegulationType = p.RegulationType,
                    MajorIndustryId = p.MajorIndustryId,
                    MinorIndustryId = p.MinorIndustryId,
                    EntityTypeId = p.EntityTypeId,
                    Status = p.Status,
                    //ManagerId = p.ManagerId,
                    CreatedOn = p.CreatedOn,
                    CreatedBy = p.CreatedBy,
                    ModifiedBy = p.ModifiedBy,
                    ModifiedOn = p.ModifiedOn,
                    UID = p.UID,
                    ResponseCode = 1,
                    ResponseMessage = message
                })
                .FirstOrDefault();
        }

        public async Task<bool> PostUpdateRegulationStupApproval(AccessModel access)
        {
            var isSuperAdmin = helperRepository.IsSuperAdmin((Int64)access.CreatedBy);

            var approvalStatuses = dbContext.RefApprovalStatus
                                    .Where(s => new[] { "Approved", "Reviewed", "Pending", "Forward" }.Contains(s.Status))
                                    .ToDictionary(s => s.Status, s => s.Id);

            var approvedStatusId = approvalStatuses.GetValueOrDefault("Approved");
            var reviewerStatusId = approvalStatuses.GetValueOrDefault("Reviewed");
            var pendingApprovalId = approvalStatuses.GetValueOrDefault("Pending");
            var forwardId = approvalStatuses.GetValueOrDefault("Forward");

            var approvalStatusId = dbContext.RefApprovalStatus.Where(s => s.Status == RefApprovalStatusU.Pending.ToString())
                .Select(s => s.Id)
                .FirstOrDefault();

            var newManagerId = access.ManagerId ?? 0;

            if (isSuperAdmin)
            {
                if (approvalStatusId == approvedStatusId || approvalStatusId == pendingApprovalId)
                {
                    approvalStatusId = approvedStatusId;
                }
            }

            if (dbContext.RegulationSetupApproval.Any(ua => ua.HistoryId == access.HistoryId && ua.ApprovalStatus == pendingApprovalId && ua.CreatedBy == access.CreatedBy))
            {
                return false;
            }

            if (!dbContext.RegulationSetupApproval.Any(ua => ua.UID == access.UID))
            {
                dbContext.RegulationSetupApproval.Add(new RegulationSetupApproval
                {
                    //UserId = access.UserId,
                    ManagerId = access.ManagerId,
                    //ApprovalType = access.ApprovalTypeId,
                    ApprovalStatus = approvalStatusId,
                    CreatedBy = access.CreatedBy,
                    CreatedOn = DateTime.Now,
                    HistoryId = access.HistoryId,
                    UID = Guid.NewGuid(),
                });
            }
            else
            {
                var userApproval = dbContext.RegulationSetupApproval.FirstOrDefault(ua => ua.UID == access.UID);
                if (userApproval != null)
                {
                    userApproval.ApprovalStatus = approvalStatusId;
                    userApproval.ModifiedBy = access.CreatedBy;
                    userApproval.ModifiedOn = DateTime.Now;
                }
            }

            if (approvalStatusId == approvedStatusId || ((approvalStatusId == forwardId || approvalStatusId == approvedStatusId) && isSuperAdmin))
            {
                var user = dbContext.RegulationSetupDetails.FirstOrDefault(u => u.Id == access.UserId);
                if (user != null)
                {
                    user.Status = 1;
                    user.ModifiedBy = access.CreatedBy;
                    user.ModifiedOn = DateTime.Now;
                }
            }
            ;

            dbContext.SaveChanges();
            return true;
        }

        public async Task<List<PendingApproval>> GetPendingRegulationSetupApproval(Guid? UserUID)
        {
            try
            {
                using (var sqlContext = unitOfWork.ConnectionFactory())
                {
                    var mul = await sqlContext.QueryMultipleAsync("[product_owner].[USP_GET_REGULATION_SETUP_APPROVAL_LIST]", new { UserUID = UserUID }, commandType: System.Data.CommandType.StoredProcedure);
                    var Result = mul.Read<PendingApproval>().ToList();
                    mul.Dispose();
                    return Result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> PostRegulationSetupApproveAccess(AccessModel access)
        {
            var isSuperAdmin = helperRepository.IsSuperAdmin((Int64)access.CreatedBy);

            var statusIds = dbContext.RefApprovalStatus
                            .Where(s => s.Status == "Approved" || s.Status == "Reviewed" || s.Status == "Pending")
                            .ToDictionary(s => s.Status, s => s.Id);

            var approvedStatusId = statusIds.GetValueOrDefault("Approved");
            var reviewerStatusId = statusIds.GetValueOrDefault("Reviewed");
            var pendingApprovalId = statusIds.GetValueOrDefault("Pending");

            var approvalStatusId = dbContext.RefApprovalStatus.Where(s => s.Status == RefApprovalStatusU.Approved.ToString())
                .Select(s => s.Id)
                .FirstOrDefault();

            var newManagerId = access.ManagerId ?? 0;

            if (isSuperAdmin)
            {
                if (approvalStatusId == approvedStatusId || approvalStatusId == pendingApprovalId)
                {
                    approvalStatusId = approvedStatusId;
                }
            }

            if (dbContext.RegulationSetupApproval.Any(ua => ua.HistoryId == access.HistoryId && ua.ApprovalStatus == pendingApprovalId && ua.CreatedBy == access.CreatedBy))
            {
                return false;
            }

            if (!dbContext.RegulationSetupApproval.Any(ua => ua.UID == access.UID))
            {
                dbContext.RegulationSetupApproval.Add(new RegulationSetupApproval
                {
                    //UserId = access.UserId,
                    ManagerId = access.ManagerId,
                    //ApprovalType = access.ApprovalTypeId,
                    ApprovalStatus = approvalStatusId,
                    CreatedBy = access.CreatedBy,
                    CreatedOn = DateTime.Now
                });
            }
            else
            {
                var userApproval = dbContext.RegulationSetupApproval.FirstOrDefault(ua => ua.UID == access.UID);
                if (userApproval != null)
                {
                    userApproval.ApprovalStatus = approvalStatusId;
                    userApproval.ModifiedBy = access.CreatedBy;
                    userApproval.ModifiedOn = DateTime.Now;
                }
            }

            if (approvalStatusId == approvedStatusId || ((approvalStatusId == approvedStatusId) && isSuperAdmin))
            {
                var regulation = dbContext.RegulationSetupHistory.FirstOrDefault(u => u.HistoryId == access.HistoryId);
                if (regulation != null)
                {
                    ComplianceAPI.Models.DataModels.RegulationSetupDetails regulationSetuprobject = null;
                    if (regulation.HistoryId == 0 || regulation.HistoryId == null)
                    {
                        regulationSetuprobject = new ComplianceAPI.Models.DataModels.RegulationSetupDetails
                        {
                            //EmpId = regulation.EmpId,
                            RegulationName = regulation.RegulationName,
                            RegulationType = regulation.RegulationType,
                            Description = regulation.Description,
                            CountryId = regulation.CountryId,
                            StateId = regulation.StateId,
                            MajorIndustryId = regulation.MajorIndustryId,
                            MinorIndustryId = regulation.MinorIndustryId,
                            EntityTypeId = regulation.EntityTypeId,
                            Status = 1,
                            //ManagerId = regulation.ManagerId,
                            CreatedOn = DateTime.Now,
                            CreatedBy = regulation.CreatedBy,
                            IsParameterChecked = regulation.IsParameterChecked,
                        };
                        dbContext.RegulationSetupDetails.Add(regulationSetuprobject);
                        dbContext.SaveChanges();
                    }
                    else
                    {
                        regulationSetuprobject = dbContext.RegulationSetupDetails.FirstOrDefault(u => u.Id == regulation.HistoryId);
                        if (regulation != null)
                        {
                            regulationSetuprobject.RegulationName = regulation.RegulationName;
                            regulationSetuprobject.RegulationType = regulation.RegulationType;
                            regulationSetuprobject.Description = regulation.Description;
                            regulationSetuprobject.CountryId = regulation.CountryId;
                            regulationSetuprobject.StateId = regulation.StateId;
                            regulationSetuprobject.RegulationGroupId = regulation.RegulationGroupId;
                            regulationSetuprobject.MajorIndustryId = regulation.MajorIndustryId;
                            regulationSetuprobject.MinorIndustryId = regulation.MinorIndustryId;
                            regulationSetuprobject.EntityTypeId = regulation.EntityTypeId;
                            //regulationSetuprobject.EmpId = regulation.EmpId;
                            //regulationSetuprobject.ManagerId = regulation.ManagerId;
                            regulationSetuprobject.ModifiedBy = regulation.CreatedBy;
                            regulationSetuprobject.ModifiedOn = DateTime.Now;
                            regulationSetuprobject.Status = 1;
                            dbContext.Update(regulationSetuprobject);
                            // dbContext.SaveChanges();
                        }
                    }
                }
            }

            dbContext.SaveChanges();
            return true;
        }

        public async Task<bool> PostRegulationSetupRejectAccess(AccessModel access)
        {
            var isSuperAdmin = helperRepository.IsSuperAdmin((Int64)access.CreatedBy);

            var approvedStatusId = dbContext.RefApprovalStatus.Where(s => s.Status == "Approved").Select(s => s.Id)
                .FirstOrDefault();

            var reviewerStatusId = dbContext.RefApprovalStatus.Where(s => s.Status == "Reviewed").Select(s => s.Id)
                .FirstOrDefault();

            var pendingApprovalId = dbContext.RefApprovalStatus.Where(s => s.Status == "Pending").Select(s => s.Id)
                .FirstOrDefault();

            var rejectedId = dbContext.RefApprovalStatus.Where(s => s.Status == "Rejected").Select(s => s.Id)
                .FirstOrDefault();

            var approvalStatusId = dbContext.RefApprovalStatus.Where(s => s.Status == RefApprovalStatusU.Rejected.ToString())
                .Select(s => s.Id)
                .FirstOrDefault();

            var newManagerId = access.ManagerId ?? 0;

            if (isSuperAdmin)
            {
                if (approvalStatusId == approvedStatusId || approvalStatusId == pendingApprovalId)
                {
                    approvalStatusId = approvedStatusId;
                }
            }

            if (dbContext.RegulationSetupApproval.Any(ua => ua.CreatedBy == access.UserId && ua.ApprovalStatus == pendingApprovalId && ua.CreatedBy == access.CreatedBy))
            {
                return false;
            }

            if (!dbContext.RegulationSetupApproval.Any(ua => ua.UID == access.UID))
            {
                dbContext.RegulationSetupApproval.Add(new RegulationSetupApproval
                {
                    //UserId = access.UserId,
                    ManagerId = access.ManagerId,
                    //ApprovalType = access.ApprovalTypeId,
                    ApprovalStatus = approvalStatusId,
                    CreatedBy = access.CreatedBy,
                    CreatedOn = DateTime.Now
                });
            }
            else
            {
                var userApproval = dbContext.RegulationSetupApproval.FirstOrDefault(ua => ua.UID == access.UID);
                if (userApproval != null)
                {
                    userApproval.ApprovalStatus = approvalStatusId;
                    userApproval.ModifiedBy = access.CreatedBy;
                    userApproval.ModifiedOn = DateTime.Now;

                    if ((approvalStatusId == reviewerStatusId) && !isSuperAdmin)
                    {
                        access.CreatedBy = dbContext.RegulationSetupApproval.Where(ua => ua.UID == access.UID).Select(ua => ua.CreatedBy).FirstOrDefault();
                        dbContext.RegulationSetupApproval.Add(new RegulationSetupApproval
                        {
                            //UserId = access.UserId,
                            ManagerId = newManagerId,
                            //ApprovalType = access.ApprovalTypeId,
                            ApprovalStatus = approvedStatusId,
                            CreatedBy = access.CreatedBy,
                            CreatedOn = DateTime.Now
                        });
                    }
                }
            }

            if (approvalStatusId == approvedStatusId || ((approvalStatusId == approvedStatusId) && isSuperAdmin))
            {
                var user = dbContext.Parameter.FirstOrDefault(u => u.Id == access.UserId);
                if (user != null)
                {
                    user.Status = 1;
                    user.ModifiedBy = access.CreatedBy;
                    user.ModifiedOn = DateTime.Now;
                }
            }

            dbContext.SaveChanges();

            return true;
        }

        public async Task<bool> PostRegulationSetupReviewAccess(AccessModel access)
        {
            var isSuperAdmin = helperRepository.IsSuperAdmin((Int64)access.CreatedBy);

            var approvedStatusId = dbContext.RefApprovalStatus.Where(s => s.Status == "Approved").Select(s => s.Id)
                .FirstOrDefault();

            var reviewerStatusId = dbContext.RefApprovalStatus.Where(s => s.Status == "Reviewed").Select(s => s.Id)
                .FirstOrDefault();

            var pendingApprovalId = dbContext.RefApprovalStatus.Where(s => s.Status == "Pending").Select(s => s.Id)
                .FirstOrDefault();

            var rejectedId = dbContext.RefApprovalStatus.Where(s => s.Status == "Rejected").Select(s => s.Id)
                .FirstOrDefault();

            var approvalStatusId = dbContext.RefApprovalStatus.Where(s => s.Status == RefApprovalStatusU.Reviewed.ToString())
                .Select(s => s.Id)
                .FirstOrDefault();

            var newManagerId = access.ManagerId ?? 0;

            if (isSuperAdmin)
            {
                if (approvalStatusId == approvedStatusId || approvalStatusId == pendingApprovalId)
                {
                    approvalStatusId = approvedStatusId;
                }
            }

            if (dbContext.RegulationSetupApproval.Any(ua => ua.ApprovalStatus == pendingApprovalId && ua.CreatedBy == access.CreatedBy))
            {
                return false;
            }

            if (!dbContext.RegulationSetupApproval.Any(ua => ua.UID == access.UID))
            {
                dbContext.RegulationSetupApproval.Add(new RegulationSetupApproval
                {
                    //UserId = access.UserId,
                    ManagerId = access.ManagerId,
                    //ApprovalType = access.ApprovalTypeId,
                    ApprovalStatus = approvalStatusId,
                    CreatedBy = access.CreatedBy,
                    CreatedOn = DateTime.Now
                });
            }
            else
            {
                var parameterApproval = dbContext.RegulationSetupApproval.FirstOrDefault(ua => ua.UID == access.UID);
                if (parameterApproval != null)
                {
                    parameterApproval.ApprovalStatus = approvalStatusId;
                    parameterApproval.ModifiedBy = access.CreatedBy;
                    parameterApproval.ModifiedOn = DateTime.Now;

                    if (approvalStatusId == reviewerStatusId && !isSuperAdmin)
                    {
                        access.CreatedBy = dbContext.RegulationSetupApproval.Where(ua => ua.UID == access.UID).Select(ua => ua.CreatedBy).FirstOrDefault();
                        dbContext.RegulationSetupApproval.Add(new RegulationSetupApproval
                        {
                            //UserId = access.UserId,
                            ManagerId = newManagerId,
                            //ApprovalType = access.ApprovalTypeId,
                            ApprovalStatus = approvedStatusId,
                            CreatedBy = access.CreatedBy,
                            CreatedOn = DateTime.Now
                        });
                    }
                }
            }

            if (approvalStatusId == approvedStatusId || approvalStatusId == approvedStatusId && isSuperAdmin)
            {
                var user = dbContext.Users.FirstOrDefault(u => u.Id == access.UserId);
                if (user != null)
                {
                    user.Status = 1;
                    user.ModifiedBy = access.CreatedBy;
                    user.ModifiedOn = DateTime.Now;
                }
            }

            dbContext.SaveChanges();

            return true;
        }

        //public async Task<RegulationSetupParameters> AdminAddRegulationStupParameters(AddRegulationStupParameters regulationStupParameters)
        //{
        //    int superAdminCount = 0;
        //    string roleName = "";
        //    string message = "Successfully Saved";
        //    ComplianceAPI.Models.DataModels.RegulationSetupParameter regulationsetupparameterobject = null;
        //    regulationsetupparameterobject = new ComplianceAPI.Models.DataModels.RegulationSetupParameter
        //    {
        //        EmpId = regulationStupParameters.CreatedBy.ToString(),
        //        MajorIndustryId = regulationStupParameters.MajorIndustryId,
        //        MinorIndustryId = regulationStupParameters.MinorIndustryId,
        //        EntityTypeId = regulationStupParameters.EntityTypeId,
        //        ParameterTypeId = regulationStupParameters.ParameterTypeId,
        //        ParameterType = regulationStupParameters.ParameterType,
        //        ParameterMode = regulationStupParameters.ParameterMode,
        //        RegulationName = regulationStupParameters.RegulationName,
        //        Status = 0,
        //        ManagerId = regulationStupParameters.ManagerId,
        //        CreatedOn = DateTime.Now,
        //        CreatedBy = regulationStupParameters.CreatedBy,

        //    };
        //    dbContext.RegulationSetupParameter.Add(regulationsetupparameterobject);
        //    dbContext.SaveChanges();

        //    return dbContext.RegulationSetupParameter
        //        .Where(p => p.Id == regulationsetupparameterobject.Id)
        //        .Select(p => new RegulationSetupParameters
        //        {
        //            Id = p.Id,
        //            EmpId = p.EmpId,
        //            MajorIndustryId = p.MajorIndustryId,
        //            MinorIndustryId = p.MinorIndustryId,
        //            EntityTypeId = p.EntityTypeId,
        //            ParameterTypeId = p.ParameterTypeId,
        //            ParameterType = p.ParameterType,
        //            ParameterMode = p.ParameterMode,
        //            RegulationName = p.RegulationName,
        //            Status = p.Status,
        //            ManagerId = p.ManagerId,
        //            CreatedOn = p.CreatedOn,
        //            CreatedBy = p.CreatedBy,
        //            ModifiedBy = p.ModifiedBy,
        //            ModifiedOn = p.ModifiedOn,
        //            UID = p.UID,
        //            ResponseCode = 1,

        //            ResponseMessage = message
        //        })
        //        .FirstOrDefault();

        //}
        public async Task<RegulationStupDetails> AddRegulationStupDetails(RegulationStupDetails regulationStupDetails)
        {
            try
            {
                //int superAdminCount = 0;
                //string roleName = "";
                //string message = "Successfully Saved";
                //ComplianceAPI.Models.DataModels.RegulationSetupHistory regulationsetupdetailsobject = null;

                var isSuperAdmin = helperRepository.IsSuperAdmin((Int64)regulationStupDetails.CreatedBy);
                var stateList = await dbContext.CountryStateMapping.Where(x => x.CountryId == regulationStupDetails.CountryId).ToListAsync();
                if (isSuperAdmin)
                {
                    await AddUpdateRegulationSetupAsync(regulationStupDetails, isSuperAdmin);
                }
                if (regulationStupDetails.RegulationType != "State")
                {
                    var regSetupHistoryList = new RegulationSetupHistory();
                    //foreach (var param in stateList)
                    //{
                    var obj = new RegulationSetupHistory
                    {
                        RegulationId = regulationStupDetails.Id,
                        StateId = 0,
                        RegulationName = regulationStupDetails.RegulationName,
                        RegulationType = regulationStupDetails.RegulationType,
                        CountryId = (int?)regulationStupDetails.CountryId,
                        RegulationGroupId = regulationStupDetails.RegulationGroupId,
                        MajorIndustryId = regulationStupDetails.MajorIndustryId,
                        MinorIndustryId = regulationStupDetails.MinorIndustryId,
                        EntityTypeId = regulationStupDetails.EntityTypeId,
                        Description = regulationStupDetails.Description,
                        IsConcernedAndAuthorityChecked = regulationStupDetails.IsConcernedAndAuthorityChecked,
                        Status = 0,
                        IsParameterChecked = regulationStupDetails.isParameterChecked,

                        CreatedOn = DateTime.Now,
                        CreatedBy = regulationStupDetails.CreatedBy,
                        RegulationSetupDetailsReferenceCode = regulationStupDetails.RegulationSetupDetailsReferenceCode,
                        ConcernedMinistryId = regulationStupDetails.ConcernedMinistryId,
                        RegulatoryAuthorityId = regulationStupDetails.RegulatoryAuthorityId
                    };

                    //regulationStupDetails.StateId = param.StateId;
                    //ManagerId = regulationStupDetails.ManagerId,

                    dbContext.RegulationSetupHistory.Add(obj);
                    await dbContext.SaveChangesAsync();
                    regSetupHistoryList.HistoryId = obj.HistoryId;
                    // }

                    var caaMapping = new RegulationSetupCAAMappingHistory
                    {
                        RegulationSetupId = regSetupHistoryList.HistoryId,
                        ConcernedMinistryId = regulationStupDetails.ConcernedMinistryId,
                        RegulatoryAuthorityId = regulationStupDetails.RegulatoryAuthorityId
                    };
                    dbContext.RegulationSetupCAAMappingHistory.Add(caaMapping);

                    if (regulationStupDetails.RegulationSetupParameters != null)
                    {
                        foreach (var param in regulationStupDetails.RegulationSetupParameters)
                        {
                            param.CreatedBy = regulationStupDetails.CreatedBy;
                            param.CreatedOn = DateTime.Now;
                            param.Status = isSuperAdmin ? 1 : 0;
                            param.RegulationSetupId = regSetupHistoryList.HistoryId;
                            dbContext.RegulationSetupParameterHistory.Add(param);
                        }
                    }

                    var tob = dbContext.RegulationSetupIndustryTOBHistory.Where(f => f.RegulationSetupId == regulationStupDetails.Id);
                    if (tob.Count() > 0)
                        dbContext.RegulationSetupIndustryTOBHistory.RemoveRange(tob);

                    foreach (var MJ in regulationStupDetails.Industry)
                    {
                        foreach (var MI in MJ.MinorIndustries)
                        {
                            // Check if there are no TOBs
                            if (MI.TOBs == null || !MI.TOBs.Any())
                            {
                                // Insert a row with TOBId set to null
                                var InTob = new RegulationSetupIndustryTOBHistory
                                {
                                    RegulationSetupId = regSetupHistoryList.HistoryId,
                                    MajorIndustryId = MJ.MajorIndustryId,
                                    MinorIndustryId = MI.MinorIndustryId,
                                    TOBId = null, // Set TOBId to null
                                    CreatedBy = regulationStupDetails.CreatedBy,
                                    CreatedOn = DateTime.Now
                                };
                                dbContext.RegulationSetupIndustryTOBHistory.Add(InTob);
                            }
                            else
                            {
                                // Iterate through each TOB and insert rows
                                foreach (var tobr in MI.TOBs)
                                {
                                    var InTob = new RegulationSetupIndustryTOBHistory
                                    {
                                        RegulationSetupId = regSetupHistoryList.HistoryId,
                                        MajorIndustryId = MJ.MajorIndustryId,
                                        MinorIndustryId = MI.MinorIndustryId,
                                        TOBId = tobr.TOBId,
                                        CreatedBy = regulationStupDetails.CreatedBy,
                                        CreatedOn = DateTime.Now,
                                    };
                                    dbContext.RegulationSetupIndustryTOBHistory.Add(InTob);
                                }
                            }
                        }
                    }

                    //Legal entity adding

                    //// Step 1: Get existing records for this RegulationSetupId
                    //var existingEntities = await dbContext.RegulationSetupLegalEntityType
                    //    .Where(x => x.RegulationSetupId == regulationid)
                    //    .ToListAsync();

                    //// Step 2: Delete existing records if found
                    //if (existingEntities.Any())
                    //{
                    //    dbContext.RegulationSetupLegalEntityType.RemoveRange(existingEntities);
                    //}

                    // Step 3: Add new records
                    foreach (var entity in regulationStupDetails.LegalEntityType)
                    {
                        var historyEntity = new RegulationSetupLegalEntityTypeHistory
                        {
                            RegulationSetupId = regSetupHistoryList.HistoryId,   // override with history id
                            ComplianceId = entity.ComplianceId,
                            LegalEntityType = entity.LegalEntityType,
                            CreatedOn = DateTime.Now,
                            CreatedBy = regulationStupDetails.CreatedBy,
                            ModifiedBy = entity.ModifiedBy,
                            ModifiedOn = entity.ModifiedOn,
                            UID = Guid.NewGuid()
                        };

                        dbContext.RegulationSetupLegalEntityTypeHistory.Add(historyEntity);
                    }

                    await dbContext.SaveChangesAsync();

                    return new RegulationStupDetails() { HistoryId = regSetupHistoryList.HistoryId, ResponseCode = 1, ResponseMessage = isSuperAdmin ? "Saved Successfully" : "Request sent for Approval" };
                }
                else
                {
                    var request = new RegulationSetupHistory
                    {
                        //EmpId = regulationStupDetails.CreatedBy.ToString(),
                        RegulationName = regulationStupDetails.RegulationName,
                        RegulationType = regulationStupDetails.RegulationType,
                        CountryId = (int?)regulationStupDetails.CountryId,
                        StateId = (long)regulationStupDetails.StateId,
                        RegulationGroupId = regulationStupDetails.RegulationGroupId,
                        MajorIndustryId = regulationStupDetails.MajorIndustryId,
                        MinorIndustryId = regulationStupDetails.MinorIndustryId,
                        EntityTypeId = regulationStupDetails.EntityTypeId,
                        Description = regulationStupDetails.Description,
                        Status = 0,
                        IsParameterChecked = regulationStupDetails.isParameterChecked,
                        IsConcernedAndAuthorityChecked = regulationStupDetails.IsConcernedAndAuthorityChecked,
                        ConcernedMinistryId = regulationStupDetails.ConcernedMinistryId,
                        RegulatoryAuthorityId = regulationStupDetails.RegulatoryAuthorityId,
                        //ManagerId = regulationStupDetails.ManagerId,
                        CreatedOn = DateTime.Now,
                        CreatedBy = regulationStupDetails.CreatedBy,
                    };

                    dbContext.RegulationSetupHistory.Add(request);
                    await dbContext.SaveChangesAsync();
                    var caaMapping = new RegulationSetupCAAMappingHistory
                    {
                        RegulationSetupId = request.HistoryId,
                        ConcernedMinistryId = regulationStupDetails.ConcernedMinistryId,
                        RegulatoryAuthorityId = regulationStupDetails.RegulatoryAuthorityId
                    };
                    dbContext.RegulationSetupCAAMappingHistory.Add(caaMapping);
                    await dbContext.SaveChangesAsync();

                    if (regulationStupDetails.RegulationSetupParameters != null)
                    {
                        foreach (var param in regulationStupDetails.RegulationSetupParameters)
                        {
                            param.CreatedBy = regulationStupDetails.CreatedBy;
                            param.CreatedOn = DateTime.Now;
                            param.Status = isSuperAdmin ? 1 : 0;
                            param.RegulationSetupId = request.HistoryId;
                            dbContext.RegulationSetupParameterHistory.Add(param);
                        }
                    }

                    var tob = dbContext.RegulationSetupIndustryTOBHistory.Where(f => f.RegulationSetupId == regulationStupDetails.Id);
                    if (tob.Count() > 0)
                        dbContext.RegulationSetupIndustryTOBHistory.RemoveRange(tob);

                    foreach (var MJ in regulationStupDetails.Industry)
                    {
                        foreach (var MI in MJ.MinorIndustries)
                        {
                            // Check if there are no TOBs
                            if (MI.TOBs == null || !MI.TOBs.Any())
                            {
                                // Insert a row with TOBId set to null
                                var InTob = new RegulationSetupIndustryTOBHistory
                                {
                                    RegulationSetupId = request.HistoryId,
                                    MajorIndustryId = MJ.MajorIndustryId,
                                    MinorIndustryId = MI.MinorIndustryId,
                                    TOBId = null, // Set TOBId to null
                                    CreatedBy = regulationStupDetails.CreatedBy,
                                    CreatedOn = DateTime.Now
                                };
                                dbContext.RegulationSetupIndustryTOBHistory.Add(InTob);
                            }
                            else
                            {
                                // Iterate through each TOB and insert rows
                                foreach (var tobr in MI.TOBs)
                                {
                                    var InTob = new RegulationSetupIndustryTOBHistory
                                    {
                                        RegulationSetupId = request.HistoryId,
                                        MajorIndustryId = MJ.MajorIndustryId,
                                        MinorIndustryId = MI.MinorIndustryId,
                                        TOBId = tobr.TOBId,
                                        CreatedBy = regulationStupDetails.CreatedBy,
                                        CreatedOn = DateTime.Now
                                    };
                                    dbContext.RegulationSetupIndustryTOBHistory.Add(InTob);
                                }
                            }
                        }
                    }

                    foreach (var entity in regulationStupDetails.LegalEntityType)
                    {
                        var historyEntity = new RegulationSetupLegalEntityTypeHistory
                        {
                            RegulationSetupId = request.HistoryId,   // override with history id
                            ComplianceId = entity.ComplianceId,
                            LegalEntityType = entity.LegalEntityType,
                            CreatedOn = DateTime.Now,
                            CreatedBy = regulationStupDetails.CreatedBy,
                            ModifiedBy = entity.ModifiedBy,
                            ModifiedOn = entity.ModifiedOn,
                            UID = Guid.NewGuid()
                        };

                        dbContext.RegulationSetupLegalEntityTypeHistory.Add(historyEntity);
                    }

                    await dbContext.SaveChangesAsync();

                    return new RegulationStupDetails() { HistoryId = request.HistoryId, ResponseCode = 1, ResponseMessage = isSuperAdmin ? "Saved Successfully" : "Request sent for Approval" };
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //public async Task<RegulationSetupParameters> AddRegulationStupParameters(AddRegulationStupParameters regulationStupParameters)
        //{
        //    int superAdminCount = 0;
        //    string roleName = "";
        //    string message = "Successfully Saved";
        //    ComplianceAPI.Models.DataModels.RegulationSetupParameter regulationsetupprameterobject = null;
        //    regulationsetupprameterobject = new ComplianceAPI.Models.DataModels.RegulationSetupParameter
        //    {
        //        EmpId = regulationStupParameters.CreatedBy.ToString(),
        //        MajorIndustryId = regulationStupParameters.MajorIndustryId,
        //        MinorIndustryId = regulationStupParameters.MinorIndustryId,
        //        EntityTypeId = regulationStupParameters.EntityTypeId,
        //        ParameterTypeId = regulationStupParameters.ParameterTypeId,
        //        ParameterType = regulationStupParameters.ParameterType,
        //        ParameterMode = regulationStupParameters.ParameterMode,
        //        RegulationName = regulationStupParameters.RegulationName,
        //        Status = 0,
        //        ManagerId = regulationStupParameters.ManagerId,
        //        CreatedOn = DateTime.Now,
        //        CreatedBy = regulationStupParameters.CreatedBy,

        //    };
        //    dbContext.RegulationSetupParameter.Add(regulationsetupprameterobject);
        //    dbContext.SaveChanges();

        //    return dbContext.RegulationSetupParameter
        //        .Where(p => p.Id == regulationsetupprameterobject.Id)
        //        .Select(p => new RegulationSetupParameters
        //        {
        //            Id = p.Id,
        //            EmpId = p.EmpId,
        //            MajorIndustryId = p.MajorIndustryId,
        //            MinorIndustryId = p.MinorIndustryId,
        //            EntityTypeId = p.EntityTypeId,
        //            ParameterTypeId = p.ParameterTypeId,
        //            ParameterMode = p.ParameterMode,
        //            ParameterType = p.ParameterType,
        //            RegulationName = p.RegulationName,
        //            Status = p.Status,
        //            ManagerId = p.ManagerId,
        //            CreatedOn = p.CreatedOn,
        //            CreatedBy = p.CreatedBy,
        //            ModifiedBy = p.ModifiedBy,
        //            ModifiedOn = p.ModifiedOn,
        //            UID = p.UID,
        //            ResponseCode = 1,
        //            ResponseMessage = message
        //        })
        //        .FirstOrDefault();

        //}
        public async Task<RegulationStupDetails> GetAllRegulationSetupDetails(Guid? regSetupuid)
        {
            var result = await dbContext.RegulationSetupDetails.Where(f => f.UID == regSetupuid).FirstOrDefaultAsync();
            if (result != null)
            {
                var response = (from rs in dbContext.RegulationSetupDetails
                                join c in dbContext.Country on rs.CountryId equals c.Id
                                join s in dbContext.States on rs.StateId equals s.Id into stateJoin
                                from s in stateJoin.DefaultIfEmpty()
                                join rg in dbContext.RegulationGroups on rs.RegulationGroupId equals rg.Id

                                join e in dbContext.EntityType on rs.EntityTypeId equals e.Id
                                where rs.UID == result.UID || rs.CountryId == result.CountryId || rs.StateId == result.StateId || rs.RegulationGroupId == result.RegulationGroupId
                                || rs.MajorIndustryId == result.MajorIndustryId || rs.MinorIndustryId == result.MinorIndustryId || rs.EntityTypeId == result.EntityTypeId
                                select new RegulationStupDetails
                                {
                                    Id = result.Id,
                                    UID = result.UID,
                                    RegulationName = result.RegulationName,
                                    RegulationType = result.RegulationType,
                                    CountryId = result.CountryId,
                                    CountryName = c.CountryName,
                                    FinancialMonth=c.FinancialStartDate,
                                    StateId = result.StateId,
                                    StateName = s.StateName,
                                    RegulationGroupId = result.RegulationGroupId,
                                    RegulationGroupName = rg.RegulationGroupName,
                                    EntityTypeId = result.EntityTypeId,
                                    EntityType = e.EntityType,
                                    MajorIndustryId = result.MajorIndustryId,
                                    IsConcernedAndAuthorityChecked = result.IsConcernedAndAuthorityChecked,
                                    MinorIndustryId = result.MinorIndustryId,

                                    Description = result.Description,
                                    Status = result.Status,
                                    CreatedOn = result.CreatedOn,
                                    CreatedBy = result.CreatedBy,
                                    ModifiedBy = result.ModifiedBy,
                                    ModifiedOn = result.ModifiedOn,
                                    isParameterChecked = result.IsParameterChecked,
                                    RegulationSetupDetailsReferenceCode = result.RegulationSetupDetailsReferenceCode,
                                    RegulationSetupParameters = dbContext.RegulationSetupParameter

                                  .Where(p => p.RegulationSetupId == result.Id)
                                  .Select(k => new RegulationSetupParameterHistory
                                  {
                                      ParameterOperator = k.ParameterOperator,
                                      ParameterTypeId = k.ParameterTypeId,
                                      ParameterTypeValue = k.ParameterTypeValue,
                                      Id = k.Id,
                                      RegulationSetupId = k.RegulationSetupId,
                                      UID = k.UID,
                                      Sequence = k.Sequence
                                  }).ToList(),
                                }).FirstOrDefault();
                if (response != null)
                {
                    // Initialize the hierarchical structure
                    // Step 1: Load all RegulationSetupIndustryTOB records for this RegulationSetupId
                    var setupIndustries = dbContext.RegulationSetupIndustryTOB
                        .Where(r => r.RegulationSetupId == response.Id && r.TOCRuleType == null)
                        .ToList();

                    // Step 2: Get all needed Ids
                    var majorIds = setupIndustries.Select(r => r.MajorIndustryId).Distinct().ToList();
                    var minorIds = setupIndustries.Select(r => r.MinorIndustryId).Distinct().ToList();
                    var tobIds = setupIndustries.Where(r => r.TOBId != null).Select(r => r.TOBId!.Value).Distinct().ToList();

                    // Step 3: Preload lookup dictionaries to avoid N+1 queries
                    var majorDict = dbContext.MajorIndustries
                        .Where(m => majorIds.Contains(m.Id))
                        .ToDictionary(m => m.Id, m => m.MajorIndustryName);

                    var minorDict = dbContext.MinorIndustries
                        .Where(m => minorIds.Contains(m.Id))
                        .ToDictionary(m => m.Id, m => m.MinorIndustryName);

                    var tobDict = dbContext.TOBDetails
                        .Where(t => tobIds.Contains(t.Id))
                        .ToDictionary(t => t.Id, t => t.TOBName);

                    // Step 4: Build the hierarchy
                    response.Industry = setupIndustries
                      .GroupBy(r => r.MajorIndustryId)
                      .Where(majorGroup => majorGroup.Key != null && majorDict.ContainsKey(majorGroup.Key))
                      .Select(majorGroup => new RegulationMajorIndustry
                      {
                          MajorIndustryId = majorGroup.Key,
                          MajorIndustryName = majorDict[majorGroup.Key],
                          MinorIndustries = majorGroup
                              .GroupBy(m => m.MinorIndustryId)
                              .Where(minorGroup => minorGroup.Key != null && minorDict.ContainsKey(minorGroup.Key))
                              .Select(minorGroup => new RegulationMinorIndustry
                              {
                                  MinorIndustryId = minorGroup.Key,
                                  MinorIndustryName = minorDict[minorGroup.Key],
                                  TOBs = minorGroup
                                      .Where(t => t.TOBId != null && tobDict.ContainsKey(t.TOBId.Value))
                                      .Select(t => new RegulationTOB
                                      {
                                          TOBId = t.TOBId!.Value,
                                          TOBName = tobDict[t.TOBId.Value]
                                      })
                                      .DistinctBy(t => t.TOBId)
                                      .ToList()
                              })
                              .ToList()
                      })
                      .ToList();
                    var conAndRegAuthList = dbContext.RegulationSetupCAAMapping.Where(c => c.RegulationSetupId == response.Id);
                    response.RegulatoryAuthorityId = await conAndRegAuthList.Select(r => r.RegulatoryAuthorityId).FirstOrDefaultAsync();
                    response.ConcernedMinistryId = await conAndRegAuthList.Select(r => r.ConcernedMinistryId).FirstOrDefaultAsync();

                    response.ConcernedMinistry = await dbContext.ConcernedMinistries
                        .Where(c => c.Id == response.ConcernedMinistryId)
                        .Select(c => new Models.ConcernedMinistry
                        {
                            Id = c.Id,
                            ConcernedMinistryCode = c.ConcernedMinistryCode,
                            ConcernedMinistryName = c.ConcernedMinistryName,
                            ConcernedMinistryReferenceCode = c.ConcernedMinistryReferenceCode
                        })
                        .FirstOrDefaultAsync();

                    response.RegulatoryAuthorities = await dbContext.RegulatoryAuthority
                        .Where(c => c.Id == response.RegulatoryAuthorityId)
                        .Select(c => new Models.RegulatoryAuthorities
                        {
                            Id = c.Id,
                            RegulatoryAuthorityCode = c.RegulatoryAuthorityCode,
                            RegulatoryAuthorityName = c.RegulatoryAuthorityName,
                            RegulatoryAuthorityReferenceCode = c.RegulatoryAuthorityReferenceCode
                        })
                        .FirstOrDefaultAsync();

                    response.LegalEntityType = await GetLegalEntityTypesByRegulationSetupId(response.Id ?? 0);
                }

                return response;
            }
            return null;
        }

        public async Task<List<RegulationSetupDetails>> GetAllRegulationSetupDetails()
        {
            return await dbContext.RegulationSetupDetails.Where(r => r.Status == 1).ToListAsync();
        }

        public async Task<List<RegulationSetupParameter>> GetAllRegulationSetupParameter()
        {
            return await dbContext.RegulationSetupParameter.ToListAsync();
        }

        public async Task<GetRegulationBasicAndParameterByCountryID> GetAllRegulationBasicAndParameterByCountryID(int countryId)
        {
            try
            {
                var regulationDetails = (from r in dbContext.GetCountryRegulationGroupMapping
                                         join rg in dbContext.RegulationGroups on r.RegulationGroupId equals rg.Id
                                         join s in dbContext.CountryStateMapping on countryId equals s.CountryId
                                         join c in dbContext.Country on s.StateId equals c.Id
                                         join sl in dbContext.States on s.StateId equals sl.Id
                                         join I in dbContext.IndustryrMapping on countryId equals I.CountryId
                                         join mg in dbContext.MajorIndustries on I.MajorIndustryId equals mg.Id
                                         join mi in dbContext.MinorIndustries on I.MinorIndustryId equals mi.Id
                                         join em in dbContext.EntityTypeMapping on countryId equals em.CountryId
                                         join e in dbContext.EntityType on em.EntityTypeId equals e.Id
                                         where r.CountryId == countryId && s.CountryId == countryId && I.CountryId == countryId && em.CountryId == countryId
                                         select new GetRegulationBasicAndParameterByCountryID
                                         {
                                             CountryId = r.CountryId,
                                             CountryName = c.CountryName,
                                             RegulationGroupId = r.RegulationGroupId,
                                             RegulationGroupName = rg.RegulationGroupName,
                                             StateId = Convert.ToInt32(s.StateId),
                                             StateName = sl.StateName,
                                             MajorIndustryId = I.MajorIndustryId,
                                             MajorIndustryName = mg.MajorIndustryName,
                                             MinorIndustryId = I.MinorIndustryId,
                                             MinorIndustryName = mi.MinorIndustryName,
                                             EntityTypeId = em.EntityTypeId,
                                             EntityTypeName = e.EntityType
                                         }).FirstOrDefault();
                return regulationDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
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

            //return await dbContext.RegulationSetupHistory.ToListAsync();
        }

        public async Task<RegulationStupDetails> GetHistoryRegulationSetup(Guid? Uid)
        {
            var approval = await dbContext.RegulationSetupApproval.Where(f => f.UID == Uid).FirstOrDefaultAsync();
            var result = (from rs in dbContext.RegulationSetupHistory
                          join c in dbContext.Country on rs.CountryId equals c.Id
                          join s in dbContext.States on rs.StateId equals s.Id into statejoin
                          from state in statejoin.DefaultIfEmpty()
                          join rg in dbContext.RegulationGroups on rs.RegulationGroupId equals rg.Id into regulationgroupjoin
                          from regulationgroup in regulationgroupjoin.DefaultIfEmpty()
                          where rs.HistoryId == approval!.HistoryId
                          select new RegulationStupDetails
                          {
                              Id = rs.RegulationId,
                              HistoryId = rs.HistoryId,
                              RegulationName = rs.RegulationName,
                              RegulationType = rs.RegulationType,
                              CountryId = rs.CountryId,
                              CountryName = c.CountryName,
                              StateId = rs.StateId,
                              StateName = state.StateName,
                              RegulationGroupId = rs.RegulationGroupId,
                              RegulationGroupName = regulationgroup.RegulationGroupName,
                              IsConcernedAndAuthorityChecked = rs.IsConcernedAndAuthorityChecked,
                              ConcernedMinistryId = rs.ConcernedMinistryId,
                              RegulatoryAuthorityId = rs.RegulatoryAuthorityId,
                              Description = rs.Description,
                              Status = rs.Status,
                              ManagerId = rs.ManagerId,
                              CreatedOn = rs.CreatedOn,
                              CreatedBy = rs.CreatedBy,
                              ModifiedBy = rs.ModifiedBy,
                              ModifiedOn = rs.ModifiedOn,
                              UID = rs.UID,
                              RegulationSetupDetailsReferenceCode = rs.RegulationSetupDetailsReferenceCode,
                              isParameterChecked = rs.IsParameterChecked,
                              RegulationSetupParameters = dbContext.RegulationSetupParameterHistory
                                  .Where(p => p.RegulationSetupId == approval!.HistoryId)
                                  .Select(h => new RegulationSetupParameterHistory()
                                  {
                                      HistoryId = h.HistoryId,
                                      RegulationSetupId = h.RegulationSetupId,
                                      ParameterTypeId = h.ParameterTypeId,
                                      ParameterTypeValue = h.ParameterTypeValue,
                                      ParameterOperator = h.ParameterOperator,
                                      Sequence = h.Sequence
                                  }).ToList()
                          }).FirstOrDefault();

            if (result != null)
            {
                var industry = dbContext.RegulationSetupIndustryTOBHistory.Where(f => f.RegulationSetupId == result.HistoryId);

                var cmData = await dbContext.ConcernedMinistries
            .FirstOrDefaultAsync(m => m.Id == result.ConcernedMinistryId);

                var RaData = await dbContext.RegulatoryAuthority
                    .FirstOrDefaultAsync(a => a.Id == result.RegulatoryAuthorityId);

                result.RegulatoryAuthorities = new Models.RegulatoryAuthorities
                {
                    Id = RaData.Id,
                    RegulatoryAuthorityCode = RaData.RegulatoryAuthorityCode,
                    RegulatoryAuthorityReferenceCode = RaData.RegulatoryAuthorityReferenceCode,
                    RegulatoryAuthorityName = RaData.RegulatoryAuthorityName
                };

                result.ConcernedMinistry = new ComplianceAPI.Models.ConcernedMinistry
                {
                    Id = cmData.Id,
                    ConcernedMinistryCode = cmData.ConcernedMinistryCode,
                    ConcernedMinistryName = cmData.ConcernedMinistryName,
                    ConcernedMinistryReferenceCode = cmData.ConcernedMinistryReferenceCode,
                    CreatedBy = cmData.CreatedBy,
                    CreatedOn = cmData.CreatedOn,
                    Status = cmData.Status,
                    ModifiedBy = cmData.ModifiedBy?.ToString(),
                    ModifiedOn = cmData.ModifiedOn,
                    UID = cmData.UID
                };

                result.Industry = dbContext.RegulationSetupIndustryTOBHistory
                    .Where(r => r.RegulationSetupId == result.HistoryId)
                    .GroupBy(r => new { r.MajorIndustryId, })
                    .Select(majorGroup => new RegulationMajorIndustry
                    {
                        MajorIndustryId = (long)majorGroup.Key.MajorIndustryId!,
                        MajorIndustryName = dbContext.MajorIndustries
.Where(m => m.Id == (long)majorGroup.Key.MajorIndustryId)
.Select(m => m.MajorIndustryName)
.FirstOrDefault(),

                        MinorIndustries = majorGroup.GroupBy(m => new { m.MinorIndustryId })
                            .Select(minorGroup => new RegulationMinorIndustry
                            {
                                MinorIndustryId = (long)minorGroup.Key.MinorIndustryId,
                                MinorIndustryName = dbContext.MinorIndustries.First(f => f.Id == (long)minorGroup.Key.MinorIndustryId).MinorIndustryName,
                                TOBs = minorGroup
                                    .Where(t => t.TOBId != null) // Ensure TOBId is not null
                                    .Select(t => new RegulationTOB
                                    {
                                        TOBId = t.TOBId!.Value,
                                        TOBName = dbContext.TOBDetails.First(f => f.Id == t.TOBId.Value).TOBName,
                                    })
                                    .ToList()
                            })
                            .ToList()
                    })
                    .ToList();
                result.LegalEntityType = dbContext.RegulationSetupLegalEntityTypeHistory
                                            .Where(k => k.RegulationSetupId == result.HistoryId)
                                            .Select(k => new RegulationSetupLegalEntityTypeDto
                                            {
                                                RegulationSetupId = result.Id,
                                                ComplianceId = k.ComplianceId,
                                                LegalEntityType = k.LegalEntityType,
                                                EntityName = dbContext.EntityType
    .Where(v => v.Id == k.LegalEntityType)
    .Select(v => v.EntityType)
    .FirstOrDefault(),
                                                CreatedOn = k.CreatedOn,
                                                CreatedBy = k.CreatedBy,
                                                ModifiedBy = k.ModifiedBy,
                                                ModifiedOn = k.ModifiedOn,
                                                UID = k.UID
                                            })
                                            .ToList();
            }
            return result;
        }

        public async Task<RegulationStupDetails> ApproveRegulationSetup(AccessModel access)
        {
            long regulationid = 0;
            var regulation = await this.GetHistoryRegulationSetup(access.UID);
            //regulationid = regulation
            regulation.ModifiedBy = access.CreatedBy;

            await AddUpdateRegulationSetupAsync(regulation, true);
            //var Adminrequest = new RegulationSetupCompliance()
            //{
            //    RegulationSetupId = compliance.RegulationSetupId,
            //    ComplianceName = compliance.ComplianceName,
            //    Description = compliance.Description,
            //    CreatedOn = DateTime.Now,
            //    CreatedBy = compliance.CreatedBy,
            //    Status = 1,
            //};
            //dbContext.RegulationSetupCompliance.Add(Adminrequest);
            //await dbContext.SaveChangesAsync();
            //if (compliance.Parameters != null)
            //{
            //    foreach (var param in compliance.Parameters)
            //    {
            //        var mainparam = new RegulationSetupComplianceParameter();
            //        mainparam.ParameterTypeId = param.ParameterTypeId;
            //        mainparam.ParameterTypeValue = param.ParameterTypeValue;
            //        mainparam.ParameterOperator = param.ParameterOperator;
            //        mainparam.CreatedBy = compliance.CreatedBy;
            //        mainparam.CreatedOn = DateTime.Now;
            //        mainparam.Status = 1;
            //        mainparam.RegulationSetupComplianceId = Adminrequest.Id;
            //        dbContext.RegulationSetupComplianceParameter.Add(mainparam);
            //    }
            //}

            var approval = dbContext.RegulationSetupApproval.Where(f => f.UID == access.UID).FirstOrDefault();

            approval.ApprovalStatus = RefApprovalStatus.Approved;
            approval.ModifiedOn = DateTime.Now;
            approval.ModifiedBy = access.CreatedBy;
            dbContext.RegulationSetupApproval.Update(approval);

            await dbContext.SaveChangesAsync();
            await RegulationSetupApprovalResponseNotification(approval.CreatedBy.Value, RefApprovalStatusU.Approved);
            return new RegulationStupDetails() { ResponseCode = 1, ResponseMessage = "Approved Successfully" };
        }

        private async Task<RegulationStupDetails> AddUpdateRegulationSetupAsync(RegulationStupDetails regulation, bool isSuperAdmin)
        {
            try
            {
                long regulationid = 0;
                long complianceid = 0;
                long tocid = 0;
                long registerationid = 0;
                // Assuming `compliance` is the object containing the data to be saved or updated
                var existingRegulation = await dbContext.RegulationSetupDetails
                    .FirstOrDefaultAsync(c => c.UID == regulation.UID || c.Id == regulation.Id);

                if (existingRegulation != null)
                {
                    // Update existing recordGetRegSetupComplianceHistory
                    existingRegulation.RegulationType = regulation.RegulationType;
                    existingRegulation.RegulationName = regulation.RegulationName;
                    existingRegulation.Description = regulation.Description;
                    existingRegulation.RegulationSetupDetailsReferenceCode = regulation.RegulationSetupDetailsReferenceCode;
                    existingRegulation.Status = isSuperAdmin ? 1 : 0;
                    existingRegulation.ModifiedOn = DateTime.Now;
                    existingRegulation.ModifiedBy = regulation?.ModifiedBy ?? regulation?.ManagerId;
                    existingRegulation.IsConcernedAndAuthorityChecked = regulation?.IsConcernedAndAuthorityChecked;

                    // Delete existing parameters
                    var existingParams = dbContext.RegulationSetupParameter
                        .Where(p => p.RegulationSetupId == existingRegulation.Id);
                    dbContext.RegulationSetupParameter.RemoveRange(existingParams);

                    // Add new parameters
                    if (regulation.RegulationSetupParameters != null)
                    {
                        foreach (var param in regulation.RegulationSetupParameters)
                        {
                            var newParam = new RegulationSetupParameter
                            {
                                ParameterTypeId = param.ParameterTypeId,
                                ParameterTypeValue = param.ParameterTypeValue,
                                ParameterOperator = param.ParameterOperator,
                                CreatedBy = regulation.CreatedBy,
                                CreatedOn = DateTime.Now,
                                ModifiedOn = DateTime.Now,
                                ModifiedBy = regulation?.ModifiedBy ?? regulation?.ManagerId,
                                Status = isSuperAdmin ? 1 : 0,
                                RegulationSetupId = existingRegulation.Id
                            };
                            dbContext.RegulationSetupParameter.Add(newParam);
                        }
                    }
                    regulationid = (long)existingRegulation.Id;

                    var existingCAAMapping = await dbContext.RegulationSetupCAAMapping
                        .FirstOrDefaultAsync(c => c.RegulationSetupId == regulationid);
                    if ((bool)regulation.IsConcernedAndAuthorityChecked)
                    {
                        // Find all related CAA mappings
                        var relatedCAAMappings = dbContext.RegulationSetupCAAMapping
                            .Where(m =>
                                (regulationid != null && m.RegulationSetupId == regulationid) ||
                                (complianceid != null && m.ComplianceId == complianceid) ||
                                (tocid != null && m.TocId == tocid)
                            ).ToList();

                        foreach (var mapping in relatedCAAMappings)
                        {
                            mapping.ConcernedMinistryId = regulation.ConcernedMinistryId;
                            mapping.RegulatoryAuthorityId = regulation.RegulatoryAuthorityId;
                            dbContext.RegulationSetupCAAMapping.Update(mapping);
                        }
                    }
                    else
                    {
                        if (existingCAAMapping != null)
                        {
                            existingCAAMapping.ConcernedMinistryId = regulation.ConcernedMinistryId;
                            existingCAAMapping.RegulatoryAuthorityId = regulation.RegulatoryAuthorityId;
                            dbContext.RegulationSetupCAAMapping.Update(existingCAAMapping);
                        }
                        else
                        {
                            var caaMapping = new RegulationSetupCAAMapping
                            {
                                RegulationSetupId = regulationid,
                                ConcernedMinistryId = regulation.ConcernedMinistryId,
                                RegulatoryAuthorityId = regulation.RegulatoryAuthorityId
                            };
                            dbContext.RegulationSetupCAAMapping.Add(caaMapping);
                        }
                    }
                }
                else
                {
                    // Add new record
                    var Adminrequest = new RegulationSetupDetails
                    {
                        CountryId = regulation.CountryId,
                        StateId = regulation.StateId,
                        RegulationGroupId = regulation.RegulationGroupId,
                        MajorIndustryId = regulation.MajorIndustryId,
                        MinorIndustryId = regulation.MinorIndustryId,
                        EntityTypeId = regulation.EntityTypeId,
                        RegulationType = regulation.RegulationType,
                        RegulationName = regulation.RegulationName,
                        Description = regulation.Description,
                        RegulationSetupDetailsReferenceCode = regulation.RegulationSetupDetailsReferenceCode,
                        CreatedOn = DateTime.Now,
                        CreatedBy = regulation?.CreatedBy,
                        Status = isSuperAdmin ? 1 : 0,
                        ModifiedBy = regulation?.ModifiedBy ?? regulation?.ManagerId,
                        ModifiedOn = DateTime.Now,
                        IsParameterChecked = regulation.isParameterChecked,
                        IsConcernedAndAuthorityChecked = regulation.IsConcernedAndAuthorityChecked,
                        ConcernedMinistryId = regulation.ConcernedMinistryId,
                        RegulatoryAuthorityId = regulation.RegulatoryAuthorityId
                    };

                    dbContext.RegulationSetupDetails.Add(Adminrequest);
                    await dbContext.SaveChangesAsync();
                    var caaMapping = new RegulationSetupCAAMapping
                    {
                        RegulationSetupId = Adminrequest.Id,
                        ConcernedMinistryId = regulation.ConcernedMinistryId,
                        RegulatoryAuthorityId = regulation.RegulatoryAuthorityId
                    };
                    dbContext.RegulationSetupCAAMapping.Add(caaMapping);
                    // Add new parameters
                    if (regulation.RegulationSetupParameters != null)
                    {
                        foreach (var param in regulation.RegulationSetupParameters)
                        {
                            var newParam = new RegulationSetupParameter
                            {
                                ParameterTypeId = param.ParameterTypeId,
                                ParameterTypeValue = param.ParameterTypeValue,
                                ParameterOperator = param.ParameterOperator,
                                CreatedBy = regulation.CreatedBy,
                                CreatedOn = DateTime.Now,
                                Status = isSuperAdmin ? 1 : 0,
                                ModifiedBy = regulation?.ModifiedBy ?? regulation?.ManagerId,
                                ModifiedOn = DateTime.Now,
                                RegulationSetupId = Adminrequest.Id
                            };
                            dbContext.RegulationSetupParameter.Add(newParam);
                        }
                    }
                    regulationid = (long)Adminrequest.Id;
                }

                //var tob = dbContext.RegulationSetupIndustryTOB.Where(f => f.RegulationSetupId == regulation.Id);
                //if (tob.Count() > 0)
                //    dbContext.RegulationSetupIndustryTOB.RemoveRange(tob);
                // Find existing mapping for this regulation and compliance

                foreach (var MJ in regulation.Industry)
                {
                    foreach (var MI in MJ.MinorIndustries)
                    {
                        // Check if there are no TOBs
                        if (MI.TOBs == null || !MI.TOBs.Any())
                        {
                            // Insert a row with TOBId set to null
                            var InTob = new RegulationSetupIndustryTOB
                            {
                                RegulationSetupId = regulationid,
                                MajorIndustryId = MJ.MajorIndustryId,
                                MinorIndustryId = MI.MinorIndustryId,
                                TOBId = null, // Set TOBId to null
                                CreatedBy = regulation?.CreatedBy,
                                CreatedOn = DateTime.Now,
                                ModifiedBy = regulation?.ModifiedBy ?? regulation?.ManagerId,
                                ModifiedOn = DateTime.Now,
                            };
                            dbContext.RegulationSetupIndustryTOB.Add(InTob);
                        }
                        else
                        {
                            // Iterate through each TOB and insert rows
                            foreach (var tobr in MI.TOBs)
                            {
                                var InTob = new RegulationSetupIndustryTOB
                                {
                                    RegulationSetupId = regulationid,
                                    MajorIndustryId = MJ.MajorIndustryId,
                                    MinorIndustryId = MI.MinorIndustryId,
                                    TOBId = tobr.TOBId,
                                    CreatedBy = regulation?.CreatedBy,
                                    CreatedOn = DateTime.Now,
                                    ModifiedBy = regulation?.ModifiedBy ?? regulation?.ManagerId,
                                    ModifiedOn = DateTime.Now,
                                };
                                dbContext.RegulationSetupIndustryTOB.Add(InTob);
                            }
                        }
                    }
                }

                //Legal entity adding

                var item = new RegulationSetupLegalEntityType();
                // Step 1: Get existing records for this RegulationSetupId
                var existingEntities = await dbContext.RegulationSetupLegalEntityType
                    .Where(x => x.RegulationSetupId == regulationid)
                    .ToListAsync();

                // Step 2: Delete existing records if found
                if (existingEntities.Any())
                {
                    dbContext.RegulationSetupLegalEntityType.RemoveRange(existingEntities);
                }

                //var addEntity = new RegulationSetupLegalEntityType();
                foreach (var entity in regulation.LegalEntityType)
                {
                    var addEntity = new RegulationSetupLegalEntityType
                    {
                        RegulationSetupId = regulationid,
                        LegalEntityType = entity.LegalEntityType,
                        CreatedOn = DateTime.Now,
                        UID = Guid.NewGuid(),
                        CreatedBy = regulation?.CreatedBy,
                        ModifiedBy = regulation?.ModifiedBy ?? regulation?.ManagerId,
                        ModifiedOn = DateTime.Now
                    };
                    dbContext.RegulationSetupLegalEntityType.Add(addEntity);
                }
                //var addEntity = new RegulationSetupLegalEntityType();
                //// Step 3: Add new records
                //foreach (var entity in regulation.LegalEntityType)
                //{
                //    addEntity.RegulationSetupId = regulationid;
                //    addEntity.LegalEntityType = entity.LegalEntityType;
                //    addEntity.CreatedOn = DateTime.Now;
                //    addEntity.UID = Guid.NewGuid();
                //    addEntity.CreatedBy = regulation?.CreatedBy;
                //    addEntity.ModifiedBy = regulation?.ModifiedBy ?? regulation?.ManagerId;
                //    addEntity.ModifiedOn = DateTime.Now;
                //    dbContext.RegulationSetupLegalEntityType.Add(addEntity);
                //}

                await dbContext.SaveChangesAsync();

                return regulation;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<RegulationStupDetails> RejectRegulationSetup(AccessModel access)
        {
            var approval = dbContext.RegulationSetupApproval.Where(f => f.UID == access.UID).FirstOrDefault();
            approval.ApprovalStatus = RefApprovalStatus.Rejected;
            approval.ModifiedOn = DateTime.Now;
            approval.ModifiedBy = access.ManagerId;
            dbContext.RegulationSetupApproval.Update(approval);

            await dbContext.SaveChangesAsync();
            await RegulationSetupApprovalResponseNotification(approval.CreatedBy.Value, RefApprovalStatusU.Rejected);
            return new RegulationStupDetails() { ResponseCode = 1, ResponseMessage = "Rejected Successfully" };
        }

        public async Task<List<MinorIndustry>> GetMinorIndustrybyMajorID(long majorIndustoryId)
        {
            var result = dbContext.MinorIndustries.Where(x => x.MajorIndustryId == majorIndustoryId && x.Status == '1').ToList();
            return result;
            //var result = dbContext.IndustryrMapping.Join(
            //              dbContext.MinorIndustries,
            //              mapping => mapping.MinorIndustryId,
            //              minor => minor.Id,
            //              (m, s) => new IndustryrMapping
            //              {
            //                  MajorIndustryId = s.MajorIndustryId,
            //                  MinorIndustryId = s.Id,
            //                  MinorIndustryName = s.MinorIndustryName,
            //                  CountryId = m.CountryId,
            //              }
            //           ).Where(w => w.MajorIndustryId == majorIndustoryId).GroupBy(x => x.MinorIndustryId).ToList();

            //return result;
        }

        public async Task<List<IndustryrMapping>> GetMinorIndustrybyMajorIDMap(List<long> majorIndustoryId, long countryId)
        {
            var result = await dbContext.IndustryrMapping
                        .Join(
                            dbContext.MinorIndustries,
                            mapping => mapping.MinorIndustryId,
                            minor => minor.Id,
                            (mapping, minor) => new { mapping, minor }
                        )
                        .Join(
                            dbContext.MajorIndustries,
                            combined => combined.minor.MajorIndustryId,
                            major => major.Id,
                            (combined, major) => new IndustryrMapping
                            {
                                MajorIndustryId = major.Id,
                                MinorIndustryId = combined.minor.Id,
                                MinorIndustryName = combined.minor.MinorIndustryName,
                                CountryId = combined.mapping.CountryId,
                                MajorIndustryName = major.MajorIndustryName // Include the MajorIndustryName
                            }
                        ).ToListAsync();
            result = result.Where(w => majorIndustoryId!.Contains((long)w.MajorIndustryId) && w.CountryId == countryId)
                         .ToList();

            return result;
        }

        public async Task<List<IndustryrMapping>> GetTOBMinorIndustrybyMajorIDMap(
     List<long> majorIndustryIds,
     List<long> minorIndustryIds,
     long countryId)
        {
            var result = await (
                from mapping in dbContext.TOBMapping
                where minorIndustryIds.Contains((long)mapping.MinorIndustryId) && mapping.CountryId == (int)countryId

                join minor in dbContext.MinorIndustries
                on mapping.MinorIndustryId equals minor.Id

                join major in dbContext.MajorIndustries
                on mapping.MajorIndustryId equals major.Id

                join tob in dbContext.TOBDetails
                on (int)mapping.TOBId equals (int)tob.Id

                where majorIndustryIds.Contains((long)major.Id) && mapping.Status == 1 // Only active entries

                select new IndustryrMapping
                {
                    MajorIndustryId = major.Id,
                    MajorIndustryName = major.MajorIndustryName,
                    MinorIndustryId = minor.Id,
                    MinorIndustryName = minor.MinorIndustryName,
                    CountryId = mapping.CountryId,
                    TOBId = tob.Id,
                    TOBName = tob.TOBName,
                    Status = mapping.Status
                }
            ).ToListAsync();

            return result;
        }

        public async Task<List<IndustryMappings>> GetIndustryMapping(long countryId)
        {
            var result = dbContext.IndustryrMapping.Where(x => x.CountryId == countryId && x.Status == '1').ToList();
            return result;
            //var result = dbContext.IndustryrMapping.Join(
            //              dbContext.MinorIndustries,
            //              mapping => mapping.MinorIndustryId,
            //              minor => minor.Id,
            //              (m, s) => new IndustryrMapping
            //              {
            //                  MajorIndustryId = s.MajorIndustryId,
            //                  MinorIndustryId = s.Id,
            //                  MinorIndustryName = s.MinorIndustryName,
            //                  CountryId = m.CountryId,
            //              }
            //           ).Where(w => w.MajorIndustryId == majorIndustoryId).GroupBy(x => x.MinorIndustryId).ToList();

            //return result;
        }

        //---------------------------------------------------------------------------------------------------

        public async Task<RegSetupComplianceModel> GetRegulationSetupCompliance(Guid? complianceuid)
        {
            var result = await dbContext.RegulationSetupCompliance
                                .Where(f => f.UID == complianceuid)
                                .SelectMany(compliance =>
                                    dbContext.RegulationSetupDetails
                                        .Where(details => compliance.RegulationSetupId != null
                                                          ? details.Id == compliance.RegulationSetupId
                                                          : dbContext.RegulationSetupCompliance
                                                              .Where(parent => parent.Id == compliance.ParentComplianceId)
                                                              .Select(parent => parent.RegulationSetupId)
                                                              .Contains(details.Id))
                                        .DefaultIfEmpty(),
                                    (compliance, details) => new { compliance, details })
                                .Select(s => new RegSetupComplianceModel
                                {
                                    Id = s.compliance.Id,
                                    UID = s.compliance.UID,
                                    ComplianceName = s.compliance.ComplianceName,
                                    CreatedBy = s.compliance.CreatedBy,
                                    CreatedOn = s.compliance.CreatedOn,
                                    Description = s.compliance.Description,
                                    Status = s.compliance.Status,
                                    Parameters = dbContext.RegulationSetupComplianceParameter
                                        .Where(p => p.RegulationSetupComplianceId == s.compliance.Id)
                                        .Select(k => new RegSetupComplianceParameterHistory
                                        {
                                            ParameterOperator = k.ParameterOperator,
                                            ParameterTypeId = k.ParameterTypeId,
                                            ParameterTypeValue = k.ParameterTypeValue,
                                            Id = k.Id,
                                            RegulationSetupComplianceId = k.RegulationSetupComplianceId,
                                            UID = k.UID,
                                            Sequence = k.Sequence
                                        }).ToList(),
                                    RegulationName = s.details.RegulationName,
                                    RegulationSetupId = s.details.Id,
                                    IsParameterChecked = s.details.IsParameterChecked,
                                    RegulationSetupUID = s.details.UID,
                                    RegulationSetupComplianceReferenceCode = s.compliance.RegulationSetupComplianceReferenceCode,
                                    SectionName = s.compliance.SectionName
                                })
                                .FirstOrDefaultAsync();
            if (result != null)
            {
                //var industry = dbContext.RegulationSetupIndustryTOB.Where(f => f.RegulationSetupId == result.RegulationSetupId);

                //result.TOBList = dbContext.RegulationSetupIndustryTOB
                //        .Where(r => r.ComplianceId == result.Id)
                //        .Select(t => new RegulationTOBList
                //        {
                //            TOBId = t.TOBId!.Value,
                //            TOBName = dbContext.TOBDetails.First(f => f.Id == t.TOBId.Value).TOBName,
                //        }).ToList();
                // Step 1: Load all RegulationSetupIndustryTOB records for this RegulationSetupId
                var setupIndustries = dbContext.RegulationSetupIndustryTOB
                    .Where(r => r.ComplianceId == result.Id && r.TOCRuleType == null)
                    .ToList();

                // Step 2: Get all needed Ids
                var majorIds = setupIndustries.Select(r => r.MajorIndustryId).Distinct().ToList();
                var minorIds = setupIndustries.Select(r => r.MinorIndustryId).Distinct().ToList();
                var tobIds = setupIndustries.Where(r => r.TOBId != null).Select(r => r.TOBId!.Value).Distinct().ToList();
                var regCompliance = await dbContext.RegulationSetupCAAMapping
                                        .FirstOrDefaultAsync(p => p.ComplianceId == result.Id);

                result.ConcernedMinistryId = regCompliance?.ConcernedMinistryId ?? 0;
                result.RegulatoryAuthorityId = regCompliance?.RegulatoryAuthorityId ?? 0;

                result.ConcernedMinistry = await dbContext.ConcernedMinistries
                    .Where(c => c.Id == result.ConcernedMinistryId)
                    .Select(c => new Models.ConcernedMinistry
                    {
                        Id = c.Id,
                        ConcernedMinistryCode = c.ConcernedMinistryCode,
                        ConcernedMinistryName = c.ConcernedMinistryName,
                        ConcernedMinistryReferenceCode = c.ConcernedMinistryReferenceCode
                    })
                    .FirstOrDefaultAsync();
                result.RegulatoryAuthorities = await dbContext.RegulatoryAuthority
                    .Where(c => c.Id == result.RegulatoryAuthorityId)
                    .Select(c => new Models.RegulatoryAuthorities
                    {
                        Id = c.Id,
                        RegulatoryAuthorityCode = c.RegulatoryAuthorityCode,
                        RegulatoryAuthorityName = c.RegulatoryAuthorityName,
                        RegulatoryAuthorityReferenceCode = c.RegulatoryAuthorityReferenceCode
                        // Add other properties if needed
                    })
                    .FirstOrDefaultAsync();

                // Step 3: Preload lookup dictionaries to avoid N+1 queries
                var majorDict = dbContext.MajorIndustries
                    .Where(m => majorIds.Contains(m.Id))
                    .ToDictionary(m => m.Id, m => m.MajorIndustryName);

                var minorDict = dbContext.MinorIndustries
                    .Where(m => minorIds.Contains(m.Id))
                    .ToDictionary(m => m.Id, m => m.MinorIndustryName);

                var tobDict = dbContext.TOBDetails
                    .Where(t => tobIds.Contains(t.Id))
                    .ToDictionary(t => t.Id, t => t.TOBName);

                result.Industry = setupIndustries
                     .GroupBy(r => r.MajorIndustryId)
                     .Where(majorGroup => majorGroup.Key != null && majorDict.ContainsKey(majorGroup.Key))
                     .Select(majorGroup => new RegulationMajorIndustry
                     {
                         MajorIndustryId = majorGroup.Key,
                         MajorIndustryName = majorDict[majorGroup.Key],
                         MinorIndustries = majorGroup
                             .GroupBy(m => m.MinorIndustryId)
                             .Where(minorGroup => minorGroup.Key != null && minorDict.ContainsKey(minorGroup.Key))
                             .Select(minorGroup => new RegulationMinorIndustry
                             {
                                 MinorIndustryId = minorGroup.Key,
                                 MinorIndustryName = minorDict[minorGroup.Key],
                                 TOBs = minorGroup
                                     .Where(t => t.TOBId != null && tobDict.ContainsKey(t.TOBId.Value) && t.MinorIndustryId == minorGroup.Key)
                                     .Select(t => new RegulationTOB
                                     {
                                         TOBId = t.TOBId!.Value,
                                         TOBName = tobDict[t.TOBId.Value]
                                     })
                                     .DistinctBy(t => t.TOBId)
                                     .ToList()
                             })
                             .ToList()
                     })
                     .ToList();

                result.LegalEntityType = await GetLegalEntityTypesByComplianceId(result.Id ?? 0);
            }

            return result;
        }

        public async Task<RegSetupComplianceModel> AddRegSetupComplianceAsync(RegSetupComplianceModel compliance)
        {
            try
            {
                var isSuperAdmin = helperRepository.IsSuperAdmin((Int64)compliance.CreatedBy);

                if (isSuperAdmin)
                {
                    await AddUpdateRegSetupComplianceAsync(compliance, isSuperAdmin);
                }

                //History Code
                var request = new RegulationSetupComplianceHistory()
                {
                    ComplianceId = compliance.Id,
                    ParentComplianceId = compliance.ParentComplianceId,
                    RegulationSetupId = compliance.RegulationSetupId,
                    ComplianceName = compliance.ComplianceName,
                    Description = compliance.Description,
                    CreatedOn = DateTime.Now,
                    CreatedBy = compliance.CreatedBy,
                    Status = isSuperAdmin ? 1 : 0,
                    SectionName = compliance.SectionName,
                    RegulationSetupComplianceReferenceCode = compliance.RegulationSetupComplianceReferenceCode
                };
                dbContext.RegulationSetupComplianceHistory.Add(request);
                await dbContext.SaveChangesAsync();

                if (compliance.Parameters != null)
                {
                    foreach (var param in compliance.Parameters)
                    {
                        param.CreatedBy = compliance.CreatedBy;
                        param.CreatedOn = DateTime.Now;
                        param.Status = isSuperAdmin ? 1 : 0;
                        param.ParameterTypeId = param.ParameterTypeId;
                        param.ParameterTypeValue = param.ParameterTypeValue;
                        param.ParameterOperator = param.ParameterOperator;
                        param.RegulationSetupComplianceId = request.HistoryId;
                        dbContext.RegSetupComplianceParameterHistory.Add(param);
                    }

                    await dbContext.SaveChangesAsync();
                }
                // await AddOrUpdateRegulationSetupIndustryTOB(compliance, compliance.Id ?? 0);

                if (compliance != null && compliance.Industry != null && compliance.Industry.Count != 0)
                {
                    foreach (var MJ in compliance.Industry)
                    {
                        foreach (var MI in MJ.MinorIndustries!)
                        {
                            var alreadyExist = await dbContext.RegulationSetupIndustryTOBHistory
                                .FirstOrDefaultAsync(c => c.ComplianceId == request.HistoryId && c.MajorIndustryId == MJ.MajorIndustryId && c.MinorIndustryId == MI.MinorIndustryId);

                            // Check if there are no TOBs
                            if (MI.TOBs == null || !MI.TOBs.Any())
                            {
                                if (alreadyExist == null)
                                {
                                    // Insert a row with TOBId set to null
                                    var InTob = new RegulationSetupIndustryTOBHistory
                                    {
                                        ComplianceId = request.HistoryId,
                                        MajorIndustryId = MJ.MajorIndustryId,
                                        MinorIndustryId = MI.MinorIndustryId,
                                        TOBId = null, // Set TOBId to null
                                        CreatedBy = compliance.CreatedBy,
                                        CreatedOn = DateTime.Now
                                    };
                                    dbContext.RegulationSetupIndustryTOBHistory.Add(InTob);
                                }
                            }
                            else
                            {
                                // Iterate through each TOB and insert rows
                                foreach (var tobr in MI!.TOBs.Select(v => v.TOBId))
                                {
                                    if (alreadyExist == null || (alreadyExist != null && alreadyExist.TOBId != tobr))
                                    {
                                        var InTob = new RegulationSetupIndustryTOBHistory
                                        {
                                            RegulationSetupId = request.HistoryId,
                                            TOBId = tobr,
                                            ComplianceId = request.HistoryId,
                                            CreatedBy = compliance.CreatedBy,
                                            MajorIndustryId = MJ.MajorIndustryId,
                                            MinorIndustryId = MI.MinorIndustryId,
                                            CreatedOn = DateTime.Now
                                        };
                                        dbContext.RegulationSetupIndustryTOBHistory.Add(InTob);
                                    }
                                }

                                await dbContext.SaveChangesAsync();
                            }
                        }
                    }
                }

                if (compliance != null && compliance.LegalEntityType != null && compliance.LegalEntityType.Count > 0)
                {
                    var item = new RegulationSetupLegalEntityTypeHistory();
                    foreach (var entity in compliance.LegalEntityType)
                    {
                        var existingEntities = await dbContext.RegulationSetupLegalEntityTypeHistory
                            .FirstOrDefaultAsync(x => x.ComplianceId == request.HistoryId && x.LegalEntityType == entity.LegalEntityType);
                        if (existingEntities == null)
                        {
                            item.ComplianceId = request.HistoryId;
                            item.LegalEntityType = entity.LegalEntityType;
                            item.CreatedOn = DateTime.Now;
                            item.UID = Guid.NewGuid();
                            item.CreatedBy = compliance.CreatedBy;
                            item.ModifiedBy = entity.ModifiedBy;
                            item.ModifiedOn = entity.ModifiedOn;
                            dbContext.RegulationSetupLegalEntityTypeHistory.Add(item);
                        }
                    }
                }
                if (compliance != null)
                {
                    var caaMapping = new RegulationSetupCAAMappingHistory
                    {
                        RegulationSetupId = compliance.RegulationSetupId,
                        ComplianceId = compliance.HistoryId,
                        ConcernedMinistryId = compliance.ConcernedMinistryId,
                        RegulatoryAuthorityId = compliance.RegulatoryAuthorityId
                    };
                    dbContext.RegulationSetupCAAMappingHistory.Add(caaMapping);
                    await dbContext.SaveChangesAsync();
                }
                await dbContext.SaveChangesAsync();

                return new RegSetupComplianceModel() { HistoryId = request.HistoryId, ResponseCode = 1, ResponseMessage = isSuperAdmin ? "Saved Successfully" : "Request sent for Approval" };
            }
            catch (Exception ex)
            {
                return new RegSetupComplianceModel() { HistoryId = 0, ResponseCode = 1, ResponseMessage = " Not Saved Successfully" };
            }
        }

        private async Task<RegSetupComplianceModel> AddUpdateRegSetupComplianceAsync(RegSetupComplianceModel compliance, bool isSuperAdmin)
        {
            long complianceid = 0;
            // Assuming `compliance` is the object containing the data to be saved or updated
            var existingCompliance = await dbContext.RegulationSetupCompliance
                .FirstOrDefaultAsync(c => c.UID == compliance.UID || c.Id == compliance.Id);
            if (existingCompliance != null)
            {
                // Update existing recordGetRegSetupComplianceHistory
                existingCompliance.RegulationSetupId = compliance.RegulationSetupId;
                existingCompliance.ParentComplianceId = compliance.ParentComplianceId;
                existingCompliance.ComplianceName = compliance.ComplianceName;
                existingCompliance.Description = compliance.Description;
                existingCompliance.Status = isSuperAdmin ? 1 : 0;
                existingCompliance.ModifiedOn = DateTime.Now;
                existingCompliance.ModifiedBy = compliance.ModifiedBy;
                existingCompliance.RegulationSetupComplianceReferenceCode = compliance.RegulationSetupComplianceReferenceCode;
                existingCompliance.SectionName = compliance.SectionName;
                // Delete existing parameters
                var existingParams = dbContext.RegulationSetupComplianceParameter
                    .Where(p => p.RegulationSetupComplianceId == existingCompliance.Id);
                dbContext.RegulationSetupComplianceParameter.RemoveRange(existingParams);

                // Add new parameters
                if (compliance.Parameters != null)
                {
                    foreach (var param in compliance.Parameters)
                    {
                        var newParam = new RegulationSetupComplianceParameter
                        {
                            ParameterTypeId = param.ParameterTypeId,
                            ParameterTypeValue = param.ParameterTypeValue,
                            ParameterOperator = param.ParameterOperator,
                            CreatedBy = compliance.CreatedBy,
                            CreatedOn = DateTime.Now,
                            Status = isSuperAdmin ? 1 : 0,
                            RegulationSetupComplianceId = existingCompliance.Id
                        };
                        dbContext.RegulationSetupComplianceParameter.Add(newParam);
                    }
                }

                complianceid = existingCompliance.Id;
            }
            else
            {
                // Add new record
                var Adminrequest = new RegulationSetupCompliance
                {
                    RegulationSetupId = compliance.RegulationSetupId,
                    ParentComplianceId = compliance.ParentComplianceId,
                    ComplianceName = compliance.ComplianceName,
                    Description = compliance.Description,
                    CreatedOn = DateTime.Now,
                    CreatedBy = compliance.CreatedBy,
                    Status = isSuperAdmin ? 1 : 0,
                    RegulationSetupComplianceReferenceCode = compliance.RegulationSetupComplianceReferenceCode,
                    SectionName = compliance.SectionName
                };
                dbContext.RegulationSetupCompliance.Add(Adminrequest);
                await dbContext.SaveChangesAsync();

                if (compliance.Parameters != null)
                {
                    foreach (var param in compliance.Parameters)
                    {
                        var mainparam = new RegulationSetupComplianceParameter
                        {
                            ParameterTypeId = param.ParameterTypeId,
                            ParameterTypeValue = param.ParameterTypeValue,
                            ParameterOperator = param.ParameterOperator,
                            CreatedBy = compliance.CreatedBy,
                            CreatedOn = DateTime.Now,
                            Status = isSuperAdmin ? 1 : 0,
                            RegulationSetupComplianceId = Adminrequest.Id
                        };
                        dbContext.RegulationSetupComplianceParameter.Add(mainparam);
                    }
                }

                complianceid = Adminrequest.Id;
                compliance.Id = Adminrequest.Id;
            }
            var tob = dbContext.RegulationSetupIndustryTOB.Where(f => f.ComplianceId == compliance.Id);
            if (tob.Count() > 0)
                dbContext.RegulationSetupIndustryTOB.RemoveRange(tob);
            await dbContext.SaveChangesAsync();
            if (compliance != null)
            {
                var caaMapping = new RegulationSetupCAAMapping
                {
                    RegulationSetupId = compliance.RegulationSetupId,
                    ComplianceId = complianceid,
                    ConcernedMinistryId = compliance.ConcernedMinistryId,
                    RegulatoryAuthorityId = compliance.RegulatoryAuthorityId
                };
                dbContext.RegulationSetupCAAMapping.Add(caaMapping);
                await dbContext.SaveChangesAsync();
            }
            //if (compliance.TOBs != null)
            //{
            //    foreach (var tobr in compliance.TOBs)
            //    {
            //        var InTob = new RegulationSetupIndustryTOB
            //        {
            //            //RegulationSetupId = compliance.RegulationSetupId,
            //            TOBId = tobr,
            //            ComplianceId = complianceid,
            //            CreatedBy = compliance.CreatedBy,
            //            CreatedOn = DateTime.Now
            //        };
            //        dbContext.RegulationSetupIndustryTOB.Add(InTob);
            //    }
            //}
            //else if (compliance.TOBList != null)
            //{
            //    foreach (var tobr in compliance.TOBList)
            //    {
            //        var InTob = new RegulationSetupIndustryTOB
            //        {
            //            //RegulationSetupId = compliance.RegulationSetupId,
            //            TOBId = tobr.TOBId,
            //            ComplianceId = complianceid,
            //            CreatedBy = compliance.CreatedBy,
            //            CreatedOn = DateTime.Now
            //        };
            //        dbContext.RegulationSetupIndustryTOB.Add(InTob);
            //    }
            //}
            await AddOrUpdateRegulationSetupIndustryTOB(compliance, complianceid);
            await AddOrUpdateLegalEntityType(compliance, complianceid);

            await dbContext.SaveChangesAsync();

            return compliance;
        }

        public async Task<bool> PostUpdateRegComplianceApproval(AccessModel access)
        {
            var isAdmin = helperRepository.IsSuperAdmin((int)access.CreatedBy);

            var approvalStatuses = dbContext.RefApprovalStatus
                                    .Where(s => new[] { "Approved", "Reviewed", "Pending", "Forward" }.Contains(s.Status))
                                    .ToDictionary(s => s.Status, s => s.Id);

            var approvedStatusId = approvalStatuses.GetValueOrDefault("Approved");
            //var reviewerStatusId = approvalStatuses.GetValueOrDefault("Reviewed");
            var pendingApprovalId = approvalStatuses.GetValueOrDefault("Pending");
            var forwardId = approvalStatuses.GetValueOrDefault("Forward");

            var approvalStatusId = dbContext.RefApprovalStatus.Where(s => s.Status == RefApprovalStatusU.Pending.ToString())
                .Select(s => s.Id)
                .FirstOrDefault();

            var newManagerId = access.ManagerId ?? 0;

            if (isAdmin)
            {
                if (approvalStatusId == approvedStatusId || approvalStatusId == pendingApprovalId)
                {
                    approvalStatusId = approvedStatusId;
                }
            }

            if (dbContext.RegulationSetupComplianceApproval.Any(ua => ua.Id == access.Id && ua.ApprovalStatus == pendingApprovalId && ua.CreatedBy == access.CreatedBy))
            {
                return false;
            }

            if (!dbContext.RegulationSetupComplianceApproval.Any(ua => ua.UID == access.UID))
            {
                dbContext.RegulationSetupComplianceApproval.Add(new RegulationSetupComplianceApproval
                {
                    HistoryId = (long)access.HistoryId,
                    ManagerId = access.ManagerId,
                    ApprovalStatus = approvalStatusId,
                    CreatedBy = access.CreatedBy,
                    CreatedOn = DateTime.Now,
                });
            }
            else
            {
                var RegSetupComplianceAppr = dbContext.RegulationSetupComplianceApproval.FirstOrDefault(ua => ua.UID == access.UID);
                if (RegSetupComplianceAppr != null)
                {
                    RegSetupComplianceAppr.ApprovalStatus = approvalStatusId;
                    RegSetupComplianceAppr.ModifiedBy = access.CreatedBy;
                    RegSetupComplianceAppr.ModifiedOn = DateTime.Now;
                }
            }

            if (approvalStatusId == approvedStatusId || ((approvalStatusId == forwardId || approvalStatusId == approvedStatusId) && isAdmin))
            {
                var user = dbContext.RegulationSetupCompliance.FirstOrDefault(u => u.Id == access.HistoryId);
                if (user != null)
                {
                    user.Status = 1;
                    user.ModifiedBy = access.CreatedBy;
                    user.ModifiedOn = DateTime.Now;
                }
            }

            dbContext.SaveChanges();
            return true;
        }

        public async Task<RegSetupComplianceModel> GetRegSetupComplianceHistory(Guid? Uid)
        {
            var reg = new RegSetupComplianceModel();
            try
            {
                var approval = await dbContext.RegulationSetupComplianceApproval.Where(f => f.UID == Uid).FirstOrDefaultAsync();
                var result = await dbContext.RegulationSetupComplianceHistory.Where(g => g.HistoryId == approval!.HistoryId).Select(k => new RegSetupComplianceModel()
                {
                    HistoryId = k!.HistoryId,
                    Id = k.ComplianceId,
                    ComplianceName = k.ComplianceName,
                    Description = k.Description,
                    UID = k.UID,
                    RegulationSetupId = k.RegulationSetupId,
                    RegulationName = dbContext.RegulationSetupDetails.Where(j => j.Id == k.RegulationSetupId).FirstOrDefault()!.RegulationName,
                    MajorIndustryId = dbContext.RegulationSetupDetails.Where(m => m.Id == k.RegulationSetupId).FirstOrDefault()!.MajorIndustryId,
                    RegulationSetupComplianceReferenceCode = k.RegulationSetupComplianceReferenceCode,
                    SectionName = k.SectionName,
                    Parameters = dbContext.RegSetupComplianceParameterHistory.Where(p => p.RegulationSetupComplianceId == k.HistoryId).Select(h => new RegSetupComplianceParameterHistory()
                    {
                        HistoryId = h.HistoryId,
                        RegulationSetupComplianceId = h.RegulationSetupComplianceId,
                        ParameterTypeId = h.ParameterTypeId,
                        ParameterTypeValue = h.ParameterTypeValue,
                        ParameterOperator = h.ParameterOperator,
                        Sequence = h.Sequence
                    }).ToList()
                }).FirstOrDefaultAsync();
                if (result == null || result.ComplianceId == 0)
                {
                    reg.ResponseCode = 0;
                    reg.ResponseMessage = "No Compliance data found";
                    return reg;
                }

                if (result != null)
                {
                    var industry = dbContext.RegulationSetupIndustryTOBHistory.Where(f => f.ComplianceId == result.HistoryId).ToList();
                    var tobData = dbContext.RegulationSetupIndustryTOBHistory.Where(f => f.ComplianceId == result.HistoryId).ToList();

                    var majorIds = industry.Select(r => r.MajorIndustryId).Distinct().ToList();
                    var minorIds = industry.Select(r => r.MinorIndustryId).Distinct().ToList();
                    var tobIds = tobData.Where(r => r.TOBId != null).Select(r => r.TOBId!.Value).Distinct().ToList();

                    // Step 3: Preload lookup dictionaries to avoid N+1 queries
                    var majorDict = dbContext.MajorIndustries
                        .Where(m => majorIds.Contains(m.Id))
                        .ToDictionary(m => m.Id, m => m.MajorIndustryName);

                    var minorDict = dbContext.MinorIndustries
                        .Where(m => minorIds.Contains(m.Id))
                        .ToDictionary(m => m.Id, m => m.MinorIndustryName);

                    var tobDict = dbContext.TOBDetails
                        .Where(t => tobIds.Contains(t.Id))
                        .ToDictionary(t => t.Id, t => t.TOBName);
                    var regCompliance = await dbContext.RegulationSetupCAAMappingHistory
                                        .FirstOrDefaultAsync(p => p.ComplianceId == result.Id);
                    result.ConcernedMinistryId = regCompliance?.ConcernedMinistryId;
                    result.RegulatoryAuthorityId = regCompliance?.RegulatoryAuthorityId;

                    var cmData = await dbContext.ConcernedMinistries
.FirstOrDefaultAsync(m => m.Id == result.ConcernedMinistryId);

                    var RaData = await dbContext.RegulatoryAuthority
                        .FirstOrDefaultAsync(a => a.Id == result.RegulatoryAuthorityId);

                    result.RegulatoryAuthorities = new Models.RegulatoryAuthorities
                    {
                        Id = RaData.Id,
                        RegulatoryAuthorityCode = RaData.RegulatoryAuthorityCode,
                        RegulatoryAuthorityReferenceCode = RaData.RegulatoryAuthorityReferenceCode,
                        RegulatoryAuthorityName = RaData.RegulatoryAuthorityName
                    };

                    result.ConcernedMinistry = new ComplianceAPI.Models.ConcernedMinistry
                    {
                        Id = cmData.Id,
                        ConcernedMinistryCode = cmData.ConcernedMinistryCode,
                        ConcernedMinistryName = cmData.ConcernedMinistryName,
                        ConcernedMinistryReferenceCode = cmData.ConcernedMinistryReferenceCode,
                        CreatedBy = cmData.CreatedBy,
                        CreatedOn = cmData.CreatedOn,
                        Status = cmData.Status,
                        ModifiedBy = cmData.ModifiedBy?.ToString(),
                        ModifiedOn = cmData.ModifiedOn,
                        UID = cmData.UID
                    };

                    // Step 4: Build the hierarchy

                    result.Industry = industry
                        .GroupBy(r => r.MajorIndustryId)
                        .Select(majorGroup => new RegulationMajorIndustry
                        {
                            MajorIndustryId = majorGroup.Key,
                            MajorIndustryName = majorGroup.Key != null && majorDict.ContainsKey(majorGroup.Key)
    ? majorDict[majorGroup.Key]
    : null,

                            MinorIndustries = majorGroup
                                .GroupBy(m => m.MinorIndustryId)
                                .Select(minorGroup => new RegulationMinorIndustry
                                {
                                    MinorIndustryId = minorGroup.Key,
                                    MinorIndustryName =
                                    minorGroup.Key != null && minorDict.ContainsKey(minorGroup.Key)
    ? minorDict[minorGroup.Key]
    : null,

                                    TOBs = tobData
                                        .Where(t => t.TOBId != null)
                                        .Select(t => new RegulationTOB
                                        {
                                            TOBId = t.TOBId!.Value,
                                            TOBName = tobDict[t.TOBId.Value]
                                        })
                                        .DistinctBy(t => t.TOBId)
                                        .ToList()
                                })
                                .ToList()
                        })
                        .ToList();
                    result.LegalEntityType = await (
                from legalEntity in dbContext.RegulationSetupLegalEntityTypeHistory
                join entity in dbContext.EntityType on legalEntity.LegalEntityType equals entity.Id into entityJoin
                from entity in entityJoin.DefaultIfEmpty()
                where legalEntity.ComplianceId == result.HistoryId
                select new RegulationSetupLegalEntityTypeDto
                {
                    RegulationSetupId = legalEntity.RegulationSetupId,
                    LegalEntityType = legalEntity.LegalEntityType,
                    EntityName = entity.EntityType,
                    UID = legalEntity.UID,
                    Id = legalEntity.HistoryId,
                    CreatedBy = legalEntity.CreatedBy
                }
            ).ToListAsync();
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<RegSetupComplianceModel> ApproveRegSetupCompliance(AccessModel access)
        {
            var compliance = await this.GetRegSetupComplianceHistory(access.UID);
            await AddUpdateRegSetupComplianceAsync(compliance, true);
            //var Adminrequest = new RegulationSetupCompliance()
            //{
            //    RegulationSetupId = compliance.RegulationSetupId,
            //    ComplianceName = compliance.ComplianceName,
            //    Description = compliance.Description,
            //    CreatedOn = DateTime.Now,
            //    CreatedBy = compliance.CreatedBy,
            //    Status = 1,
            //};
            //dbContext.RegulationSetupCompliance.Add(Adminrequest);
            //await dbContext.SaveChangesAsync();
            //if (compliance.Parameters != null)
            //{
            //    foreach (var param in compliance.Parameters)
            //    {
            //        var mainparam = new RegulationSetupComplianceParameter();
            //        mainparam.ParameterTypeId = param.ParameterTypeId;
            //        mainparam.ParameterTypeValue = param.ParameterTypeValue;
            //        mainparam.ParameterOperator = param.ParameterOperator;
            //        mainparam.CreatedBy = compliance.CreatedBy;
            //        mainparam.CreatedOn = DateTime.Now;
            //        mainparam.Status = 1;
            //        mainparam.RegulationSetupComplianceId = Adminrequest.Id;
            //        dbContext.RegulationSetupComplianceParameter.Add(mainparam);
            //    }
            //}

            var approval = dbContext.RegulationSetupComplianceApproval.Where(f => f.UID == access.UID).FirstOrDefault();

            approval.ApprovalStatus = RefApprovalStatus.Approved;
            approval.ModifiedOn = DateTime.Now;
            approval.ModifiedBy = access.CreatedBy;
            dbContext.RegulationSetupComplianceApproval.Update(approval);

            await dbContext.SaveChangesAsync();

            return new RegSetupComplianceModel() { ResponseCode = 1, ResponseMessage = "Approved Successfully" };
        }

        public async Task<RegSetupComplianceModel> RejectRegSetupCompliance(AccessModel access)
        {
            var approval = dbContext.RegulationSetupComplianceApproval.Where(f => f.UID == access.UID).FirstOrDefault();
            approval.ApprovalStatus = RefApprovalStatus.Rejected;
            approval.ModifiedOn = DateTime.Now;
            approval.ModifiedBy = access.CreatedBy;
            dbContext.RegulationSetupComplianceApproval.Update(approval);

            await dbContext.SaveChangesAsync();

            return new RegSetupComplianceModel() { ResponseCode = 1, ResponseMessage = "Rejected Successfully" };
        }

        //public async Task<TOCModel> AddTOCSetupDetails(TOCModel TOC)
        //{
        //    var JsonResult = JsonHelper.SerializeToJson<TOCModel>(TOC);
        //    var isSuperAdmin = helperRepository.IsSuperAdmin((long)TOC.TOCRegistration.CreatedBy!);
        //    if (isSuperAdmin)
        //    {
        //        await SaveTOCRegistrationAsync(TOC);
        //        await SaveTOCRulesAsync(TOC.TOCRules, TOC.ComplianceId);
        //    }
        //    throw new NotImplementedException();
        //}

        //public async Task<TOCRegistrationModel> GetTOCRegistration(long tocRegId, long? complianceId, long? regulationid, string ruleType)
        //{
        //    var filter = complianceId == 0 || complianceId == null ? await dbContext.TOCRegistration.Where(r => r.RegulationSetupId == regulationid).ToListAsync() : await dbContext.TOCRegistration.Where(r => r.ComplianceId == complianceId).ToListAsync();
        //    if (filter != null && filter.Count() > 0)
        //    {
        //        var response = await filter.Select(async s => new TOCRegistrationModel
        //        {
        //            Id = s.Id,
        //            RegistrationName = s.RegistrationName,
        //            Description = s.Description,
        //            CreatedOn = s.CreatedOn,
        //            CreatedBy = s.CreatedBy,
        //            ModifiedBy = s.ModifiedBy,
        //            ModifiedOn = s.ModifiedOn,
        //            UID = s.UID,
        //            RegulationSetupId = s.RegulationSetupId,
        //            SectionNameOfRegister = s.SectionNameOfRegister,
        //            RegulationSetupTypeOfComplianceRegisterRC = s.RegulationSetupTypeOfComplianceRegisterRC,
        //            TOCDocument = dbContext.TOCDocuments.Where(d => d.TOCRegistrationId == s.Id).Select(d => new TOCDocuments
        //            {
        //                Id = d.Id,
        //                TOCRegistrationId = d.TOCRegistrationId,
        //                DocumentName = d.DocumentName,
        //                Status = d.Status,
        //                CreatedOn = d.CreatedOn,
        //                CreatedBy = d.CreatedBy,
        //                ModifiedBy = d.ModifiedBy,
        //                ModifiedOn = d.ModifiedOn,
        //                UID = d.UID
        //            }).ToList(),
        //            TOCParameter = await getParameters()
        //        }).FirstOrDefault();

        //        if (response != null)
        //        {
        //            //var industry = dbContext.RegulationSetupIndustryTOB.Where(f => f.RegulationSetupId == result.RegulationSetupId);

        //            response.TOBList = dbContext.RegulationSetupIndustryTOB
        //                    .Where(r => (r.RegulationSetupId == regulationid || r.ComplianceId == complianceId) && r.TOCRuleType == ruleType)
        //                    .Select(t => new RegulationTOBList
        //                    {
        //                        TOBId = t.TOBId!.Value,
        //                        TOBName = dbContext.TOBDetails.First(f => f.Id == t.TOBId.Value).TOBName,
        //                    }).ToList();
        //        }
        //        return response;
        //    }
        //    async Task<List<TOCParameter>> getParameters()
        //    {
        //        var parameters = (complianceId == 0 || complianceId == null) ? await dbContext.TOCParameter.Where(d => d.RegulationSetupId == regulationid && d.TOCRuleType == "Registration").ToListAsync() : await dbContext.TOCParameter.Where(d => (d.ComplianceId == (complianceId == 0 ? null : complianceId) && d.TOCRuleType == "Registration")).ToListAsync();
        //        if (parameters != null)
        //        {
        //            var result = parameters.Select(p => new TOCParameter
        //            {
        //                Id = p.Id,
        //                ComplianceId = p.ComplianceId,
        //                ParameterTypeId = p.ParameterTypeId,
        //                ParameterTypeValue = p.ParameterTypeValue,
        //                ParameterOperator = p.ParameterOperator,
        //                Sequence = p.Sequence,
        //                Status = p.Status,
        //                CreatedOn = p.CreatedOn,
        //                CreatedBy = p.CreatedBy,
        //                ModifiedBy = p.ModifiedBy,
        //                UID = p.UID
        //            }).ToList();
        //            return result;
        //        }
        //        return null;
        //    }
        //    return null;
        //}
        public async Task<TOCRegistrationModel> GetTOCRegistration(long tocRegId, long? complianceId, long? regulationid, string ruleType)
        {
            try
            {
                var filter = complianceId == 0 || complianceId == null
                    ? await dbContext.TOCRegistration.Where(r => r.RegulationSetupId == regulationid).ToListAsync()
                    : await dbContext.TOCRegistration.Where(r => r.ComplianceId == complianceId).ToListAsync();

                if (filter != null && filter.Count() > 0)
                {
                    var s = filter.FirstOrDefault();
                    if (s == null) return null;

                    var response = new TOCRegistrationModel
                    {
                        Id = s.Id,
                        RegistrationName = s.RegistrationName,
                        Description = s.Description,
                        CreatedOn = s.CreatedOn,
                        CreatedBy = s.CreatedBy,
                        ModifiedBy = s.ModifiedBy,
                        ModifiedOn = s.ModifiedOn,
                        UID = s.UID,
                        RegulationSetupId = s.RegulationSetupId,
                        SectionNameOfRegister = s.SectionNameOfRegister,
                        RegulationSetupTypeOfComplianceRegisterRC = s.RegulationSetupTypeOfComplianceRegisterRC,
                        TOCDocument = dbContext.TOCDocuments.Where(d => d.TOCRegistrationId == s.Id).Select(d => new TOCDocuments
                        {
                            Id = d.Id,
                            TOCRegistrationId = d.TOCRegistrationId,
                            DocumentName = d.DocumentName,
                            Status = d.Status,
                            CreatedOn = d.CreatedOn,
                            CreatedBy = d.CreatedBy,
                            ModifiedBy = d.ModifiedBy,
                            ModifiedOn = d.ModifiedOn,
                            UID = d.UID
                        }).ToList(),
                        TOCParameter = await getParameters()
                    };

                    // Fetch Industry hierarchy
                    var setupIndustries = dbContext.RegulationSetupIndustryTOB
                        .Where(r => (r.RegulationSetupId == regulationid || r.ComplianceId == complianceId) && r.TOCRuleType == ruleType)
                        .ToList();

                    var majorIds = setupIndustries.Select(r => r.MajorIndustryId).Distinct().ToList();
                    var minorIds = setupIndustries.Select(r => r.MinorIndustryId).Distinct().ToList();
                    var tobIds = setupIndustries.Where(r => r.TOBId != null).Select(r => r.TOBId!.Value).Distinct().ToList();

                    var majorDict = dbContext.MajorIndustries
                        .Where(m => majorIds.Contains(m.Id))
                        .ToDictionary(m => m.Id, m => m.MajorIndustryName);

                    var minorDict = dbContext.MinorIndustries
                        .Where(m => minorIds.Contains(m.Id))
                        .ToDictionary(m => m.Id, m => m.MinorIndustryName);

                    var tobDict = dbContext.TOBDetails
                        .Where(t => tobIds.Contains(t.Id))
                        .ToDictionary(t => t.Id, t => t.TOBName);

                    response.Industry = setupIndustries
                        .GroupBy(r => r.MajorIndustryId)
                        .Where(majorGroup => majorGroup.Key != null && majorDict.ContainsKey(majorGroup.Key))
                        .Select(majorGroup => new RegulationMajorIndustry
                        {
                            MajorIndustryId = majorGroup.Key,
                            MajorIndustryName = majorDict[majorGroup.Key],
                            MinorIndustries = majorGroup
                                .GroupBy(m => m.MinorIndustryId)
                                .Where(minorGroup => minorGroup.Key != null && minorDict.ContainsKey(minorGroup.Key))
                                .Select(minorGroup => new RegulationMinorIndustry
                                {
                                    MinorIndustryId = minorGroup.Key,
                                    MinorIndustryName = minorDict[minorGroup.Key],
                                    TOBs = minorGroup
                                        .Where(t => t.TOBId != null && tobDict.ContainsKey(t.TOBId.Value))
                                        .Select(t => new RegulationTOB
                                        {
                                            TOBId = t.TOBId!.Value,
                                            TOBName = tobDict[t.TOBId.Value]
                                        })
                                        .ToList()
                                })
                                .ToList()
                        })
                        .ToList();

                    var regCompliance = await dbContext.RegulationSetupCAAMapping
                    .FirstOrDefaultAsync(p => p.TocId == tocRegId);
                    if (regCompliance != null)
                    {
                        response.ConcernedMinistryId = regCompliance?.ConcernedMinistryId ?? 0;
                        response.RegulatoryAuthorityId = regCompliance?.RegulatoryAuthorityId ?? 0;
                    }

                    var list = await dbContext.RegulationSetupLegalEntityType
                        .Where(x =>
                            (x.RegulationSetupId == s.RegulationSetupId || x.ComplianceId == s.ComplianceId)
                            && x.TocRegisterationId == tocRegId)
                           .Select(x => new RegulationSetupLegalEntityTypeDto
                           {
                               RegulationSetupId = x.RegulationSetupId,
                               ComplianceId = x.ComplianceId,
                               LegalEntityType = x.LegalEntityType,
                               EntityName = dbContext.EntityType.Where(e => e.Id == x.LegalEntityType).Select(e => e.EntityType).FirstOrDefault(),
                               UID = x.UID,
                               Id = x.Id,
                               CreatedBy = x.CreatedBy
                           })
                           .GroupBy(x => new { x.RegulationSetupId, x.ComplianceId, x.LegalEntityType, x.EntityName })
                           .Select(g => g.First())
                           .ToListAsync();
                    response.LegalEntityType = list;
                    return response;
                }

                async Task<List<TOCParameter>> getParameters()
                {
                    var parameters = (complianceId == 0 || complianceId == null)
                        ? await dbContext.TOCParameter.Where(d => d.RegulationSetupId == regulationid && d.TOCRuleType == "Registration").ToListAsync()
                        : await dbContext.TOCParameter.Where(d => (d.ComplianceId == (complianceId == 0 ? null : complianceId) && d.TOCRuleType == "Registration")).ToListAsync();
                    if (parameters != null)
                    {
                        var result = parameters.Select(p => new TOCParameter
                        {
                            Id = p.Id,
                            ComplianceId = p.ComplianceId,
                            ParameterTypeId = p.ParameterTypeId,
                            ParameterTypeValue = p.ParameterTypeValue,
                            ParameterOperator = p.ParameterOperator,
                            Sequence = p.Sequence,
                            Status = p.Status,
                            CreatedOn = p.CreatedOn,
                            CreatedBy = p.CreatedBy,
                            ModifiedBy = p.ModifiedBy,
                            UID = p.UID
                        }).ToList();
                        return result;
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
            }

            return null;
        }

        public async Task<TOCRegistrationModel> SaveTOCRegistrationAsync(TOCRegistrationModel dto)
        {
            string message = "Saved Successfull and Sent for Review";
            try
            {
                var JsonResult = JsonHelper.SerializeToJson<TOCRegistrationModel>(dto);
                var isSuperAdmin = helperRepository.IsSuperAdmin((long)dto.CreatedBy!);
                if (isSuperAdmin)
                {
                    await addTOCRegistrationAsync(dto);
                    message = "Saved Successfull";
                }

                var history = new TOCHistory
                {
                    CreatedBy = dto.CreatedBy,
                    CreatedOn = DateTime.Now,
                    ComplianceId = dto.ComplianceId,
                    RegulationSetupId = dto.RegulationSetupId,
                    TOCRuleType = dto.RuleType,
                    TOCHistoryJson = JsonResult
                };
                dbContext.TOCHistory.Add(history);
                await dbContext.SaveChangesAsync();
                var tob = dbContext.RegulationSetupIndustryTOBHistory.Where(f => f.ComplianceId == dto.ComplianceId && f.RegulationSetupId == dto.RegulationSetupId && f.TOCRuleType == dto.RuleType);
                if (tob.Count() > 0)
                    dbContext.RegulationSetupIndustryTOBHistory.RemoveRange(tob);

                if (dto.Industry != null && dto.Industry.Count > 0)
                {
                    foreach (var MJ in dto.Industry)
                    {
                        foreach (var MI in MJ.MinorIndustries)
                        {
                            if (MI.TOBs == null || !MI.TOBs.Any())
                            {
                                var InTob = new RegulationSetupIndustryTOBHistory
                                {
                                    RegulationSetupId = dto.RegulationSetupId,
                                    ComplianceId = dto.ComplianceId,
                                    TOCId = history.HistoryId,
                                    MajorIndustryId = MJ.MajorIndustryId,
                                    MinorIndustryId = MI.MinorIndustryId,
                                    TOBId = null,
                                    TOCRuleType = dto.RuleType,
                                    CreatedBy = dto.CreatedBy,
                                    CreatedOn = DateTime.Now
                                };
                                dbContext.RegulationSetupIndustryTOBHistory.Add(InTob);
                            }
                            else
                            {
                                foreach (var tobr in MI.TOBs)
                                {
                                    var InTob = new RegulationSetupIndustryTOBHistory
                                    {
                                        RegulationSetupId = dto.RegulationSetupId,
                                        ComplianceId = dto.ComplianceId,
                                        TOCId = history.HistoryId,
                                        MajorIndustryId = MJ.MajorIndustryId,
                                        MinorIndustryId = MI.MinorIndustryId,
                                        TOBId = tobr.TOBId,
                                        TOCRuleType = dto.RuleType,
                                        CreatedBy = dto.CreatedBy,
                                        CreatedOn = DateTime.Now
                                    };
                                    dbContext.RegulationSetupIndustryTOBHistory.Add(InTob);
                                }
                            }
                        }
                    }
                }
                else if (dto.TOBs != null && dto.TOBs.Count > 0)
                {
                    // Fallback: flat TOB list (legacy)
                    foreach (var tobr in dto.TOBs)
                    {
                        var InTob = new RegulationSetupIndustryTOBHistory
                        {
                            RegulationSetupId = dto.RegulationSetupId,
                            ComplianceId = dto.ComplianceId,
                            TOBId = tobr,
                            TOCRuleType = dto.RuleType,
                            CreatedBy = dto.CreatedBy,
                            CreatedOn = DateTime.Now,
                            TOCId = history.HistoryId
                        };
                        dbContext.RegulationSetupIndustryTOBHistory.Add(InTob);
                    }
                }

                // Remove existing legal entity type history for this TOC registration
                var existingEntities = dbContext.RegulationSetupLegalEntityTypeHistory
                    .Where(x => x.RegulationSetupId == dto.RegulationSetupId && x.ComplianceId == dto.ComplianceId);
                if (existingEntities.Any())
                {
                    dbContext.RegulationSetupLegalEntityTypeHistory.RemoveRange(existingEntities);
                }

                // Save legal entity type history
                if (dto.LegalEntityType != null && dto.LegalEntityType.Count > 0)
                {
                    foreach (var entity in dto.LegalEntityType)
                    {
                        var historyEntity = new RegulationSetupLegalEntityTypeHistory
                        {
                            RegulationSetupId = dto.RegulationSetupId,
                            ComplianceId = dto.ComplianceId,
                            TocId = history.HistoryId,
                            LegalEntityType = entity.LegalEntityType,
                            CreatedOn = DateTime.Now,
                            CreatedBy = dto.CreatedBy,
                            ModifiedBy = entity.ModifiedBy,
                            ModifiedOn = entity.ModifiedOn,
                            UID = Guid.NewGuid()
                        };
                        dbContext.RegulationSetupLegalEntityTypeHistory.Add(historyEntity);
                    }
                }

                if ((dto.ConcernedMinistryId.HasValue && dto.ConcernedMinistryId.Value > 0) || (dto.RegulatoryAuthorityId.HasValue && dto.RegulatoryAuthorityId.Value > 0))
                {
                    var caaMapping = new RegulationSetupCAAMappingHistory
                    {
                        RegulationSetupId = dto.RegulationSetupId,
                        ComplianceId = dto.ComplianceId,
                        TocId = history.HistoryId,
                        TOCRuleType = dto.RuleType,
                        ConcernedMinistryId = dto.ConcernedMinistryId,
                        RegulatoryAuthorityId = dto.RegulatoryAuthorityId
                    };
                    dbContext.RegulationSetupCAAMappingHistory.Add(caaMapping);
                }

                await dbContext.SaveChangesAsync();

                dto.HistoryId = history.HistoryId;
                dto.ResponseCode = 1;
                dto.ResponseMessage = message;

                return dto;
            }
            catch (Exception ex)
            {
                dto.ResponseCode = 0;
                dto.ResponseMessage = "Error occured";
                return dto;
            }
        }

        private async Task addTOCRegistrationAsync(TOCRegistrationModel dto)
        {
            if (dto.Id > 0)
            {
                // Update existing TOCRegistration
                var existingRegistration = await dbContext.TOCRegistration.FindAsync(dto.Id);

                if (existingRegistration != null)
                {
                    existingRegistration.RegistrationName = dto.RegistrationName;
                    existingRegistration.Description = dto.Description;
                    existingRegistration.ModifiedBy = dto.CreatedBy;
                    existingRegistration.ModifiedOn = DateTime.Now;
                    existingRegistration.SectionNameOfRegister = dto.SectionNameOfRegister;
                    dbContext.TOCRegistration.Update(existingRegistration);
                    await dbContext.SaveChangesAsync();
                    dto.Id = existingRegistration.Id;
                }
            }
            else
            {
                var registration = new TOCRegistration
                {
                    RegistrationName = dto.RegistrationName,
                    ComplianceId = dto.ComplianceId,
                    RegulationSetupId = dto.RegulationSetupId,
                    Description = dto.Description,
                    CreatedBy = dto.CreatedBy,
                    CreatedOn = DateTime.Now,
                    UID = Guid.NewGuid(),
                    RegulationSetupTypeOfComplianceRegisterRC = dto.RegulationSetupTypeOfComplianceRegisterRC,
                    SectionNameOfRegister = dto.SectionNameOfRegister
                };

                dbContext.TOCRegistration.Add(registration);
                await dbContext.SaveChangesAsync();

                dto.Id = registration.Id;
            }

            // Remove existing industry records for this TOC registration
            var tob = dbContext.RegulationSetupIndustryTOB
                .Where(f => f.ComplianceId == dto.ComplianceId && f.RegulationSetupId == dto.RegulationSetupId && f.TOCRuleType == dto.RuleType);
            if (tob.Any())
                dbContext.RegulationSetupIndustryTOB.RemoveRange(tob);

            // Map full industry hierarchy: major, minor, TOB
            if (dto.Industry != null && dto.Industry.Count > 0)
            {
                foreach (var major in dto.Industry)
                {
                    foreach (var minor in major.MinorIndustries)
                    {
                        if (minor.TOBs == null || !minor.TOBs.Any())
                        {
                            var inTob = new RegulationSetupIndustryTOB
                            {
                                RegulationSetupId = dto.RegulationSetupId,
                                ComplianceId = dto.ComplianceId,
                                TOCId = dto.Id,
                                TocRegisterationId = dto.Id,
                                MajorIndustryId = major.MajorIndustryId,
                                MinorIndustryId = minor.MinorIndustryId,
                                TOBId = null,
                                TOCRuleType = dto.RuleType,
                                CreatedBy = dto.CreatedBy,
                                CreatedOn = DateTime.Now
                            };
                            dbContext.RegulationSetupIndustryTOB.Add(inTob);
                        }
                        else
                        {
                            foreach (var tobItem in minor.TOBs)
                            {
                                var inTob = new RegulationSetupIndustryTOB
                                {
                                    RegulationSetupId = dto.RegulationSetupId,
                                    ComplianceId = dto.ComplianceId,
                                    TOCId = dto.Id,
                                    TocRegisterationId = dto.Id,
                                    MajorIndustryId = major.MajorIndustryId,
                                    MinorIndustryId = minor.MinorIndustryId,
                                    TOBId = tobItem.TOBId,
                                    TOCRuleType = dto.RuleType,
                                    CreatedBy = dto.CreatedBy,
                                    CreatedOn = DateTime.Now
                                };
                                dbContext.RegulationSetupIndustryTOB.Add(inTob);
                            }
                            await dbContext.SaveChangesAsync();
                        }
                    }
                }
            }
            else if (dto.TOBs != null && dto.TOBs.Count > 0)
            {
                foreach (var tobr in dto.TOBs)
                {
                    var inTob = new RegulationSetupIndustryTOB
                    {
                        RegulationSetupId = dto.RegulationSetupId,
                        ComplianceId = dto.ComplianceId,
                        TOBId = tobr,
                        TOCRuleType = dto.RuleType,
                        CreatedBy = dto.CreatedBy,
                        CreatedOn = DateTime.Now,
                        TOCId = dto.Id,
                        TocRegisterationId = dto.Id
                    };
                    dbContext.RegulationSetupIndustryTOB.Add(inTob);
                }
            }

            // Remove existing legal entity types for this TOC registration
            var existingEntities = dbContext.RegulationSetupLegalEntityType
                .Where(x => x.TocRegisterationId == dto.Id);
            if (existingEntities.Any())
            {
                dbContext.RegulationSetupLegalEntityType.RemoveRange(existingEntities);
            }

            // Save new legal entity types for this TOC registration
            if (dto.LegalEntityType != null && dto.LegalEntityType.Count > 0)
            {
                foreach (var entity in dto.LegalEntityType)
                {
                    var newEntity = new RegulationSetupLegalEntityType
                    {
                        TocRegisterationId = dto.Id,
                        RegulationSetupId = dto.RegulationSetupId,
                        ComplianceId = dto.ComplianceId,
                        LegalEntityType = entity.LegalEntityType,
                        CreatedOn = DateTime.Now,
                        CreatedBy = dto.CreatedBy,
                        ModifiedBy = entity.ModifiedBy,
                        ModifiedOn = entity.ModifiedOn,
                        UID = Guid.NewGuid()
                    };
                    dbContext.RegulationSetupLegalEntityType.Add(newEntity);
                }
            }

            await dbContext.SaveChangesAsync();

            //var existingCAAMapping = dbContext.RegulationSetupCAAMapping
            //    .Where(x => x.TocId == dto.Id);
            //if (existingCAAMapping.Any())
            //{
            //    dbContext.RegulationSetupCAAMapping.RemoveRange(existingCAAMapping);
            //}

            if ((dto.ConcernedMinistryId.HasValue && dto.ConcernedMinistryId.Value > 0) || (dto.RegulatoryAuthorityId.HasValue && dto.RegulatoryAuthorityId.Value > 0))
            {
                var caaMapping = new RegulationSetupCAAMapping
                {
                    RegulationSetupId = dto.RegulationSetupId,
                    ComplianceId = dto.ComplianceId,
                    TocId = dto.Id,
                    TOCRuleType = dto.RuleType,
                    ConcernedMinistryId = dto.ConcernedMinistryId,
                    RegulatoryAuthorityId = dto.RegulatoryAuthorityId
                };
                dbContext.RegulationSetupCAAMapping.Add(caaMapping);
                await dbContext.SaveChangesAsync();
            }

            await SaveTOCDocumentsAsync(dto.TOCDocument, dto.Id);
            await SaveTOCParametersAsync(dto.TOCParameter, dto.ComplianceId, dto.RuleType, dto.CreatedBy, dto.RegulationSetupId);
        }

        private async Task SaveTOCDocumentsAsync(List<TOCDocuments> documents, long registrationId)
        {
            // Start a new transaction
            using (var transaction = await dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    // Delete existing documents for the given registrationId
                    var existingDocuments = await dbContext.TOCDocuments
                        .Where(d => d.TOCRegistrationId == registrationId)
                        .ToListAsync();
                    if (existingDocuments != null && existingDocuments.Any())
                    {
                        dbContext.TOCDocuments.RemoveRange(existingDocuments);
                    }

                    // Prepare document entities to insert
                    if (documents != null)
                    {
                        var documentEntities = documents.Select(d => new TOCDocuments
                        {
                            TOCRegistrationId = registrationId,
                            DocumentName = d.DocumentName,
                            CreatedBy = 1, // Replace with actual CreatedBy
                            CreatedOn = DateTime.Now,
                            UID = Guid.NewGuid()
                        }).ToList();

                        // Insert new documents
                        dbContext.TOCDocuments.AddRange(documentEntities);
                    }

                    // Save changes
                    await dbContext.SaveChangesAsync();

                    // Commit the transaction if all operations succeeded
                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    // Rollback the transaction if any operation failed
                    await transaction.RollbackAsync();
                    throw; // Rethrow the exception to propagate the error
                }
            }
        }

        public async Task<bool> TOCComplianceApproval(AccessModel access)
        {
            var isAdmin = helperRepository.IsSuperAdmin((int)access.CreatedBy);

            var approvalStatuses = dbContext.RefApprovalStatus
                                    .Where(s => new[] { "Approved", "Reviewed", "Pending", "Forward" }.Contains(s.Status))
                                    .ToDictionary(s => s.Status, s => s.Id);

            var approvedStatusId = approvalStatuses.GetValueOrDefault("Approved");
            //var reviewerStatusId = approvalStatuses.GetValueOrDefault("Reviewed");
            var pendingApprovalId = approvalStatuses.GetValueOrDefault("Pending");
            var forwardId = approvalStatuses.GetValueOrDefault("Forward");

            var approvalStatusId = dbContext.RefApprovalStatus.Where(s => s.Status == RefApprovalStatusU.Pending.ToString())
                .Select(s => s.Id)
                .FirstOrDefault();

            var newManagerId = access.ManagerId ?? 0;

            if (isAdmin)
            {
                if (approvalStatusId == approvedStatusId || approvalStatusId == pendingApprovalId)
                {
                    approvalStatusId = approvedStatusId;
                }
            }

            if (dbContext.TOCApproval.Any(ua => ua.Id == access.Id && ua.ApprovalStatus == pendingApprovalId && ua.CreatedBy == access.CreatedBy))
            {
                return false;
            }

            if (!dbContext.TOCApproval.Any(ua => ua.UID == access.UID))
            {
                dbContext.TOCApproval.Add(new TOCApproval
                {
                    HistoryId = (long)access.HistoryId,
                    ManagerId = access.ManagerId,
                    ApprovalStatus = approvalStatusId,
                    CreatedBy = access.CreatedBy,
                    CreatedOn = DateTime.Now,
                    ComplianceId = access.ComplianceId,
                    TOCRuleType = access.TOCRuleType
                });
            }
            else
            {
                var RegSetupComplianceAppr = dbContext.TOCApproval.FirstOrDefault(ua => ua.UID == access.UID);
                if (RegSetupComplianceAppr != null)
                {
                    RegSetupComplianceAppr.ApprovalStatus = approvalStatusId;
                    RegSetupComplianceAppr.ModifiedBy = access.CreatedBy;
                    RegSetupComplianceAppr.ModifiedOn = DateTime.Now;
                }
            }

            if (approvalStatusId == approvedStatusId || ((approvalStatusId == forwardId || approvalStatusId == approvedStatusId) && isAdmin))
            {
                var user = dbContext.RegulationSetupCompliance.FirstOrDefault(u => u.Id == access.HistoryId);
                if (user != null)
                {
                    user.Status = 1;
                    user.ModifiedBy = access.CreatedBy;
                    user.ModifiedOn = DateTime.Now;
                }
            }

            dbContext.SaveChanges();
            return true;
        }

        public async Task<List<TOCRegistration>> GetAllTocRegister()
        {
            return await dbContext.TOCRegistration.ToListAsync();
        }

        public async Task<TOCRules> GetTOCRulesAsync(long complianceId, long regulationid, string ruleType)
        {
            try
            {
                var dues = await dbContext.TOCDues
            .Where(d => (d.ComplianceId == complianceId || d.RegulationSetupId == regulationid) && d.TOCRuleType == ruleType && d.Status == true)
            .Select(d => new TOCDuesModel
            {
                Id = d.Id,
                ComplianceId = d.ComplianceId,
                Frequency = d.Frequency,
                ForTheMonth = d.ForTheMonth,
                DueDate = d.DueDate,
                CreatedBy = d.CreatedBy,
                CreatedOn = d.CreatedOn,
                ModifiedBy = d.ModifiedBy,
                ModifiedOn = d.ModifiedOn,
                UID = d.UID,
                FrequencyType = d.FrequencyType,
                FromTrunOver = d.FromTrunOver,
                ToTrunOver = d.ToTrunOver,
                Status = d.Status,
                SectionNameofDues = d.SectionNameofDues,
                DuesReferenceCode = d.DuesReferenceCode,
                TOCDueDates = dbContext.TOCDueDates
                    .Where(dd => dd.TOCDuesId == d.Id && dd.Status == true)
                    .Select(dd => new TOCDueDatesModel
                    {
                        Id = dd.Id,
                        TOCDuesId = dd.TOCDuesId,
                        DueDate = dd.DueDate,
                        CreatedBy = dd.CreatedBy,
                        CreatedOn = dd.CreatedOn,
                        ModifiedBy = dd.ModifiedBy,
                        ModifiedOn = dd.ModifiedOn,
                        UID = dd.UID,
                        FrequencyType = dd.FrequencyType,
                        FromTrunOver = dd.FromTrunOver,
                        ToTrunOver = dd.ToTrunOver,
                        ForTheMonth = dd.ForTheMonth,
                        Status = dd.Status,
                        Label = dd.Label, // Assuming Label is a property in TOCDueDatesModel
                        TocDueMonths = dbContext.TOCDueDates
                    .Where(cd => cd.ParentTocDueDateId == dd.Id && cd.Status == true)
                    .Select(dd => new TOCDueDatesModel
                    {
                        Id = dd.Id,
                        TOCDuesId = dd.TOCDuesId,
                        DueDate = dd.DueDate,
                        CreatedBy = dd.CreatedBy,
                        CreatedOn = dd.CreatedOn,
                        ModifiedBy = dd.ModifiedBy,
                        ModifiedOn = dd.ModifiedOn,
                        UID = dd.UID,
                        FrequencyType = dd.FrequencyType,
                        FromTrunOver = dd.FromTrunOver,
                        ToTrunOver = dd.ToTrunOver,
                        ForTheMonth = dd.ForTheMonth,
                        Status = dd.Status,
                        Label = dd.Label, // Assuming Label is a property in TOCDueDatesModel
                    }).ToList()
                    }).ToList()
            }).ToListAsync();

                if (dues == null)
                    return null; // Or handle accordingly based on your application logic

                // Fetch TOCIntrestPenality
                var intrestPenalities = await dbContext.TOCIntrestPenality
                    .Where(p => (p.ComplianceId == complianceId || p.RegulationSetupId == regulationid) && p.TOCRuleType == ruleType)
                    .ToListAsync();

                // Fetch TOCImprisonment
                var imprisonments = await dbContext.TOCImprisonment
                    .Where(i => (i.ComplianceId == complianceId || i.RegulationSetupId == regulationid) && i.TOCRuleType == ruleType)
                    .ToListAsync();

                // Fetch TOCParameter
                var parameters = await dbContext.TOCParameter
                    .Where(p => (p.ComplianceId == complianceId || p.RegulationSetupId == regulationid) && p.TOCRuleType == ruleType)
                    .ToListAsync();

                // Construct TOCRules object
                var tocRules = new TOCRules
                {
                    ComplianceId = complianceId,
                    RuleType = ruleType,
                    TOCDues = dues,
                    TOCIntrestPenality = intrestPenalities,
                    TOCImprisonment = imprisonments,
                    TOCParameter = parameters,
                    //CreatedOn = dues.CreatedOn,
                    //CreatedBy = dues.CreatedBy,
                    //ModifiedBy = dues.ModifiedBy,
                    //ModifiedOn = dues.ModifiedOn
                    // Add other properties as needed
                };
                if (dues != null && dues.Count > 0)
                {
                    tocRules.Id = dues[0].Id;
                }
                if (tocRules != null)
                {
                    //var industry = dbContext.RegulationSetupIndustryTOB.Where(f => f.RegulationSetupId == result.RegulationSetupId);

                    //tocRules.TOBList = dbContext.RegulationSetupIndustryTOB
                    //        .Where(r => (r.RegulationSetupId == regulationid || r.ComplianceId == complianceId) && r.TOCRuleType == ruleType)
                    //        .Select(t => new RegulationTOBList
                    //        {
                    //            TOBId = t.TOBId!.Value,
                    //            TOBName = dbContext.TOBDetails.First(f => f.Id == t.TOBId.Value).TOBName,
                    //        }).ToList();
                    if (tocRules != null)
                    {
                        // Initialize the hierarchical structure
                        // Step 1: Load all RegulationSetupIndustryTOB records for this RegulationSetupId
                        var setupIndustries = dbContext.RegulationSetupIndustryTOB
                            .Where(f => (f.RegulationSetupId == regulationid || f.ComplianceId == complianceId) && f.TOCRuleType == ruleType)
                            .ToList();
                        Console.WriteLine(tocRules.Id);
                        var regCompliance = await dbContext.RegulationSetupCAAMapping
                            .FirstOrDefaultAsync(p => p.TocId == tocRules.Id);
                        tocRules.ConcernedMinistryId = regCompliance?.ConcernedMinistryId;
                        tocRules.RegulatoryAuthorityId = regCompliance?.RegulatoryAuthorityId;
                        // Step 2: Get all needed Ids
                        var majorIds = setupIndustries.Where(f => (f.RegulationSetupId == regulationid || f.ComplianceId == complianceId) && f.TOCRuleType == ruleType).Select(r => r.MajorIndustryId).Distinct().ToList();
                        var minorIds = setupIndustries.Where(f => (f.RegulationSetupId == regulationid || f.ComplianceId == complianceId) && f.TOCRuleType == ruleType && f.MinorIndustryId != null).Select(r => r.MinorIndustryId).Distinct().ToList();
                        var tobIds = setupIndustries.Where(f => (f.RegulationSetupId == regulationid || f.ComplianceId == complianceId) && f.TOCRuleType == ruleType && f.TOBId != null).Select(r => r.TOBId!.Value).Distinct().ToList();

                        // Step 3: Preload lookup dictionaries to avoid N+1 queries
                        var majorDict = dbContext.MajorIndustries
                            .Where(m => majorIds.Contains(m.Id))
                            .ToDictionary(m => m.Id, m => m.MajorIndustryName);

                        var minorDict = dbContext.MinorIndustries
                            .Where(m => minorIds.Contains(m.Id))
                            .ToDictionary(m => m.Id, m => m.MinorIndustryName);

                        var tobDict = dbContext.TOBDetails
                            .Where(t => tobIds.Contains(t.Id))
                            .ToDictionary(t => t.Id, t => t.TOBName);

                        tocRules.Industry = setupIndustries
                        .GroupBy(r => r.MajorIndustryId)
                        .Select(majorGroup => new RegulationMajorIndustry
                        {
                            MajorIndustryId = majorGroup.Key,
                            MajorIndustryName = majorGroup.Key != null ? majorDict.GetValueOrDefault(majorGroup.Key) : null,

                            MinorIndustries = majorGroup
                                .GroupBy(m => m.MinorIndustryId)
                                .Select(minorGroup => new RegulationMinorIndustry
                                {
                                    MinorIndustryId = minorGroup.Key,
                                    MinorIndustryName = minorGroup.Key != null ? minorDict.GetValueOrDefault(minorGroup.Key) : null,
                                    TOBs = minorGroup
                                        .Where(t => t.TOBId != null)
                                        .Select(t => new RegulationTOB
                                        {
                                            TOBId = t.TOBId!.Value,
                                            TOBName = tobDict[t.TOBId.Value]
                                        })
                                        .DistinctBy(t => t.TOBId)
                                        .ToList()
                                })
                                .ToList()
                        })
                        .ToList();

                        tocRules.LegalEntityType = dbContext.RegulationSetupLegalEntityType.Where(k => k.RegulationSetupId == tocRules.Id).ToList();
                    }
                }

                return tocRules;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<TOCRules> SaveTOCRulesAsync(TOCRules rule)
        {
            var JsonResult = JsonHelper.SerializeToJson<TOCRules>(rule);
            var isSuperAdmin = helperRepository.IsSuperAdmin((long)rule.CreatedBy!);
            if (isSuperAdmin)
            {
                await addTOCRulesAsync(rule, rule.HistoryId);
            }

            var history = new TOCHistory
            {
                CreatedBy = rule.CreatedBy,
                CreatedOn = DateTime.Now,
                ComplianceId = rule.ComplianceId,
                RegulationSetupId = rule.RegulationSetupId,
                TOCRuleType = rule.RuleType,
                TOCHistoryJson = JsonResult
            };
            dbContext.TOCHistory.Add(history);
            await dbContext.SaveChangesAsync();

            // Remove existing history records for this compliance/rule
            var tob = dbContext.RegulationSetupIndustryTOBHistory
                .Where(f => f.ComplianceId == rule.ComplianceId && f.RegulationSetupId == rule.RegulationSetupId && f.TOCRuleType == rule.RuleType);
            if (tob.Any())
                dbContext.RegulationSetupIndustryTOBHistory.RemoveRange(tob);

            var existingEntities = dbContext.RegulationSetupLegalEntityTypeHistory
        .Where(x => x.RegulationSetupId == rule.RegulationSetupId && x.ComplianceId == rule.ComplianceId && x.TocId == history.HistoryId);
            if (existingEntities.Any())
            {
                dbContext.RegulationSetupLegalEntityTypeHistory.RemoveRange(existingEntities);
            }

            // Save legal entity type history
            if (rule.LegalEntityType != null && rule.LegalEntityType.Count > 0)
            {
                foreach (var entity in rule.LegalEntityType)
                {
                    var historyEntity = new RegulationSetupLegalEntityTypeHistory
                    {
                        RegulationSetupId = rule.RegulationSetupId,
                        ComplianceId = rule.ComplianceId,
                        TocId = history.HistoryId,
                        LegalEntityType = entity.LegalEntityType,
                        CreatedOn = DateTime.Now,
                        CreatedBy = rule.CreatedBy,
                        ModifiedBy = entity.ModifiedBy,
                        ModifiedOn = entity.ModifiedOn,
                        UID = Guid.NewGuid()
                    };
                    dbContext.RegulationSetupLegalEntityTypeHistory.Add(historyEntity);
                }
            }

            // If NOT super admin, bind all major/minor/TOB combinations from rule.Industry
            if (!isSuperAdmin && rule.Industry != null)
            {
                foreach (var MJ in rule.Industry)
                {
                    foreach (var MI in MJ.MinorIndustries)
                    {
                        if (MI.TOBs == null || !MI.TOBs.Any())
                        {
                            var inTob = new RegulationSetupIndustryTOBHistory
                            {
                                RegulationSetupId = rule.RegulationSetupId,
                                ComplianceId = rule.ComplianceId,
                                TOCId = history.HistoryId,
                                MajorIndustryId = MJ.MajorIndustryId,
                                MinorIndustryId = MI.MinorIndustryId,
                                TOBId = null,
                                TOCRuleType = rule.RuleType,
                                CreatedBy = rule.CreatedBy,
                                CreatedOn = DateTime.Now
                            };
                            dbContext.RegulationSetupIndustryTOBHistory.Add(inTob);
                        }
                        else
                        {
                            foreach (var tobr in MI.TOBs)
                            {
                                var inTob = new RegulationSetupIndustryTOBHistory
                                {
                                    RegulationSetupId = rule.RegulationSetupId,
                                    ComplianceId = rule.ComplianceId,
                                    TOCId = history.HistoryId,
                                    MajorIndustryId = MJ.MajorIndustryId,
                                    MinorIndustryId = MI.MinorIndustryId,
                                    TOBId = tobr.TOBId,
                                    TOCRuleType = rule.RuleType,
                                    CreatedBy = rule.CreatedBy,
                                    CreatedOn = DateTime.Now
                                };
                                dbContext.RegulationSetupIndustryTOBHistory.Add(inTob);
                            }
                        }
                    }
                }
            }
            else
            {
                // Existing logic for super admin: just add TOBs if present
                foreach (var tobr in rule.TOBs)
                {
                    var InTob = new RegulationSetupIndustryTOBHistory
                    {
                        RegulationSetupId = rule.RegulationSetupId,
                        ComplianceId = rule.ComplianceId,
                        TOCId = history.HistoryId,
                        TOBId = tobr,
                        TOCRuleType = rule.RuleType,
                        CreatedBy = rule.CreatedBy,
                        CreatedOn = DateTime.Now
                    };
                    dbContext.RegulationSetupIndustryTOBHistory.Add(InTob);
                }
            }
            if ((rule.ConcernedMinistryId.HasValue && rule.ConcernedMinistryId.Value > 0) || (rule.RegulatoryAuthorityId.HasValue && rule.RegulatoryAuthorityId.Value > 0))
            {
                var caaMapping = new RegulationSetupCAAMappingHistory
                {
                    RegulationSetupId = rule.RegulationSetupId,
                    ComplianceId = rule.ComplianceId,
                    TocId = history.HistoryId,
                    TOCRuleType = rule.RuleType,
                    ConcernedMinistryId = rule.ConcernedMinistryId,
                    RegulatoryAuthorityId = rule.RegulatoryAuthorityId
                };
                dbContext.RegulationSetupCAAMappingHistory.Add(caaMapping);
            }
            await dbContext.SaveChangesAsync();

            rule.HistoryId = history.HistoryId;
            rule.ResponseCode = 1;
            rule.ResponseMessage = "Saved Successfull";
            return rule;
        }

        //public async Task<TOCRules> SaveTOCRulesAsync(TOCRules rule)
        //{
        //    var JsonResult = JsonHelper.SerializeToJson<TOCRules>(rule);
        //    var isSuperAdmin = helperRepository.IsSuperAdmin((long)rule.CreatedBy!);
        //    if (isSuperAdmin)
        //    {
        //        await addTOCRulesAsync(rule, rule.HistoryId);
        //    }

        //    var history = new TOCHistory
        //    {
        //        CreatedBy = rule.CreatedBy,
        //        CreatedOn = DateTime.Now,
        //        ComplianceId = rule.ComplianceId,
        //        RegulationSetupId = rule.RegulationSetupId,
        //        TOCRuleType = rule.RuleType,
        //        TOCHistoryJson = JsonResult
        //    };
        //    dbContext.TOCHistory.Add(history);
        //    await dbContext.SaveChangesAsync();
        //    var tob = dbContext.RegulationSetupIndustryTOBHistory.Where(f => f.ComplianceId == rule.ComplianceId && f.RegulationSetupId == rule.RegulationSetupId && f.TOCRuleType == rule.RuleType);
        //    if (tob.Count() > 0)
        //        dbContext.RegulationSetupIndustryTOBHistory.RemoveRange(tob);
        //    foreach (var tobr in rule.TOBs)
        //    {
        //        var InTob = new RegulationSetupIndustryTOBHistory
        //        {
        //            //RegulationSetupId = rule.RegulationSetupId,
        //            ComplianceId = rule.ComplianceId,
        //            RegulationSetupId = rule.RegulationSetupId,
        //            TOBId = tobr,
        //            TOCRuleType = rule.RuleType,

        //            //ComplianceId = history.HistoryId,
        //            CreatedBy = rule.CreatedBy,
        //            CreatedOn = DateTime.Now
        //        };
        //        dbContext.RegulationSetupIndustryTOBHistory.Add(InTob);
        //    }

        //    rule.HistoryId = history.HistoryId;
        //    rule.ResponseCode = 1;
        //    rule.ResponseMessage = "Saved Successfull";
        //    return rule;
        //}

        private async Task<TOCRules> addTOCRulesAsync(TOCRules rule, long? historyId)
        {
            long duesId = 0;
            if (rule.TOCDues != null)
            {
                duesId = await SaveOrUpdateTOCDuesAsync(rule, rule.ComplianceId);
                if (rule.TOCDues.Count() > 0)
                {
                    await SaveTOCDueDatesAsync(rule, duesId);
                }
            }

            if (rule.TOCIntrestPenality != null && rule.TOCIntrestPenality.Count() > 0)
            {
                await SaveOrUpdateTOCIntrestPenalitiesAsync(rule, rule.ComplianceId);
            }

            if (rule.TOCImprisonment != null && rule.TOCImprisonment.Count() > 0)
            {
                await SaveOrUpdateTOCImprisonmentsAsync(rule, rule.ComplianceId);
            }

            if (rule.TOCParameter != null && rule.TOCParameter.Count() > 0)
            {
                await SaveTOCParametersAsync(rule.TOCParameter, rule.ComplianceId, rule.RuleType, rule.CreatedBy, rule.RegulationSetupId);
            }

            var tob = dbContext.RegulationSetupIndustryTOB.Where(f => f.ComplianceId == rule.ComplianceId && f.RegulationSetupId == rule.RegulationSetupId && f.TOCRuleType == rule.RuleType);
            if (tob.Count() > 0)
                dbContext.RegulationSetupIndustryTOB.RemoveRange(tob);
            long? tocDuesId = rule.TOCDues != null && rule.TOCDues.Count > 0 ? rule.TOCDues[0].Id : null;

            var existingEntities = dbContext.RegulationSetupLegalEntityType
                .Where(x => x.RegulationSetupId == rule.RegulationSetupId && x.ComplianceId == rule.ComplianceId && x.TocId == tocDuesId);
            if (existingEntities.Any())
            {
                dbContext.RegulationSetupLegalEntityType.RemoveRange(existingEntities);
            }

            // Save new legal entity types for this rule
            if (rule.LegalEntityType != null && rule.LegalEntityType.Count > 0)
            {
                foreach (var entity in rule.LegalEntityType)
                {
                    var newEntity = new RegulationSetupLegalEntityType
                    {
                        RegulationSetupId = rule.RegulationSetupId,
                        ComplianceId = rule.ComplianceId,
                        TocId = tocDuesId,
                        LegalEntityType = entity.LegalEntityType,
                        CreatedOn = DateTime.Now,
                        CreatedBy = rule.CreatedBy,
                        ModifiedBy = entity.ModifiedBy,
                        ModifiedOn = entity.ModifiedOn,
                        UID = Guid.NewGuid()
                    };
                    dbContext.RegulationSetupLegalEntityType.Add(newEntity);
                }
            }
            //foreach (var tobr in rule.TOBs)
            //{
            //    var InTob = new RegulationSetupIndustryTOB
            //    {
            //        TOBId = tobr,
            //        ComplianceId = rule.ComplianceId,
            //        RegulationSetupId = rule.RegulationSetupId,
            //        CreatedBy = rule.CreatedBy,
            //        CreatedOn = DateTime.Now,
            //        TOCRuleType = rule.RuleType,
            //    };
            //    dbContext.RegulationSetupIndustryTOB.Add(InTob);
            //}
            foreach (var MJ in rule.Industry)
            {
                foreach (var MI in MJ.MinorIndustries)
                {
                    // Check if there are no TOBs
                    if (MI.TOBs == null || !MI.TOBs.Any())
                    {
                        // Insert a row with TOBId set to null
                        var InTob = new RegulationSetupIndustryTOB
                        {
                            TOCId = duesId,
                            MajorIndustryId = MJ.MajorIndustryId,
                            MinorIndustryId = MI.MinorIndustryId,
                            TOBId = null, // Set TOBId to null
                            CreatedBy = rule.CreatedBy,
                            CreatedOn = DateTime.Now,
                            ComplianceId = rule.ComplianceId,
                            RegulationSetupId = rule.RegulationSetupId,
                            TOCRuleType = rule.RuleType,
                        };
                        dbContext.RegulationSetupIndustryTOB.Add(InTob);
                    }
                    else
                    {
                        // Iterate through each TOB and insert rows
                        foreach (var tobr in MI.TOBs)
                        {
                            var InTob = new RegulationSetupIndustryTOB
                            {
                                TOCId = duesId,
                                MajorIndustryId = MJ.MajorIndustryId,
                                MinorIndustryId = MI.MinorIndustryId,
                                TOBId = tobr.TOBId,
                                CreatedBy = rule.CreatedBy,
                                CreatedOn = DateTime.Now,
                                ComplianceId = rule.ComplianceId,
                                RegulationSetupId = rule.RegulationSetupId,
                                TOCRuleType = rule.RuleType,
                            };
                            dbContext.RegulationSetupIndustryTOB.Add(InTob);
                        }
                    }
                }
            }

            if ((rule.ConcernedMinistryId.HasValue && rule.ConcernedMinistryId.Value > 0) || (rule.RegulatoryAuthorityId.HasValue && rule.RegulatoryAuthorityId.Value > 0))
            {
                var caaMapping = new RegulationSetupCAAMapping
                {
                    RegulationSetupId = rule.RegulationSetupId,
                    ComplianceId = rule.ComplianceId,
                    TocId = duesId,
                    TOCRuleType = rule.RuleType,
                    ConcernedMinistryId = rule.ConcernedMinistryId,
                    RegulatoryAuthorityId = rule.RegulatoryAuthorityId
                };
                dbContext.RegulationSetupCAAMapping.Add(caaMapping);
                await dbContext.SaveChangesAsync();
            }

            return rule;
        }

        private async Task<long> SaveOrUpdateTOCDuesAsync(TOCRules rule, long? complianceId)
        {
            try
            {
                if (rule.TOCDues.Count() > 0)
                {
                    foreach (var due in rule.TOCDues)
                    {
                        if (due.Id > 0)
                        {
                            // Update existing TOCDues
                            var existingDues = await dbContext.TOCDues.FindAsync(due.Id);

                            if (existingDues != null)
                            {
                                existingDues.ComplianceId = complianceId;
                                existingDues.RegulationSetupId = rule.RegulationSetupId;
                                existingDues.TOCRuleType = rule.RuleType;
                                existingDues.Frequency = due.Frequency;
                                existingDues.ForTheMonth = due.ForTheMonth;
                                existingDues.DueDate = due.DueDate;
                                existingDues.ModifiedBy = rule.CreatedBy; // Replace with actual ModifiedBy
                                existingDues.ModifiedOn = DateTime.Now;
                                existingDues.FrequencyType = due.FrequencyType;
                                existingDues.FromTrunOver = due.FromTrunOver;
                                existingDues.ToTrunOver = due.ToTrunOver;
                                existingDues.Status = due.Status;
                                existingDues.SectionNameofDues = due.SectionNameofDues;
                                existingDues.DuesReferenceCode = due.DuesReferenceCode;
                                dbContext.TOCDues.Update(existingDues);
                                await dbContext.SaveChangesAsync();
                                due.Id = existingDues.Id;
                                // return existingDues.Id;

                                return due.Id;
                            }
                            else
                            {
                                throw new Exception($"TOCDues with Id {due.Id} not found.");
                            }
                        }
                        else
                        {
                            // Add new TOCDues if Id is null or 0
                            var duesEntity = new TOCDues
                            {
                                ComplianceId = complianceId,
                                RegulationSetupId = rule.RegulationSetupId,
                                TOCRuleType = rule.RuleType,
                                Frequency = due.Frequency,
                                ForTheMonth = due.ForTheMonth,
                                DueDate = due.DueDate,
                                CreatedBy = rule.CreatedBy, // Replace with actual CreatedBy
                                CreatedOn = DateTime.Now,
                                UID = Guid.NewGuid(),
                                FrequencyType = due.FrequencyType,
                                FromTrunOver = due.FromTrunOver,
                                ToTrunOver = due.ToTrunOver,
                                Status = due.Status,
                                SectionNameofDues = due.SectionNameofDues,
                                DuesReferenceCode = due.DuesReferenceCode
                            };

                            dbContext.TOCDues.Add(duesEntity);

                            foreach (var tobr in rule.TOBs)
                            {
                                var InTob = new RegulationSetupIndustryTOB
                                {
                                    TOCId = duesEntity.Id,
                                    TOCRuleType = rule.RuleType,
                                    TOBId = tobr,
                                    //ComplianceId = rule.Id,
                                    CreatedBy = rule.CreatedBy,
                                    CreatedOn = DateTime.Now
                                };
                                dbContext.RegulationSetupIndustryTOB.Add(InTob);
                            }
                            await dbContext.SaveChangesAsync();
                            due.Id = duesEntity.Id;
                            // return duesEntity.Id;
                            return due.Id;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            //throw new Exception("TOCDues object in TOCRules is null.");

            return 0;
        }

        //private async Task SaveTOCDueDatesAsync(TOCRules rule, long duesId)
        //{
        //    // Start a new transaction
        //    using (var transaction = await dbContext.Database.BeginTransactionAsync())
        //    {
        //        try
        //        {
        //            // Delete existing due dates for the given duesId
        //            //var existingDueDates = await dbContext.TOCDueDates
        //            //    .Where(dd => dd.TOCDuesId == duesId)
        //            //    .ToListAsync();

        //            //if (existingDueDates != null && existingDueDates.Any())
        //            //{
        //            //    dbContext.TOCDueDates.RemoveRange(existingDueDates);
        //            //}

        //            foreach (var due in rule.TOCDues)
        //            {
        //                //foreach (var item in due.TOCDueDates)
        //                //{
        //                    // Prepare due date entities to insert
        //                    if (rule.TOCDues != null && due.TOCDueDates != null)
        //                    {
        //                        var dueDateEntities = due.TOCDueDates.Where(d=>d.Id == 0).Select(dd => new TOCDueDates
        //                        {
        //                            Id = dd.Id,
        //                            TOCDuesId = due.Id,
        //                            DueDate = dd.DueDate,
        //                            Label = dd.Label,
        //                            CreatedBy = rule.CreatedBy, // Replace with actual CreatedBy
        //                            CreatedOn = DateTime.Now,
        //                            UID = Guid.NewGuid(),
        //                            FrequencyType = dd.FrequencyType,
        //                            FromTrunOver = dd.FromTrunOver,
        //                            ToTrunOver = dd.ToTrunOver,
        //                            ForTheMonth = dd.ForTheMonth,
        //                            Status= dd.Status
        //                        }).ToList();

        //                        // Insert new due dates
        //                        if (dueDateEntities.Count() > 0)
        //                        {
        //                            dbContext.TOCDueDates.AddRange(dueDateEntities);
        //                        }

        //                        var updatedueDateEntities = due.TOCDueDates.Where(d => d.Id > 0).Select(dd => new TOCDueDates
        //                        {
        //                            Id= dd.Id,
        //                            TOCDuesId = due.Id,
        //                            DueDate = dd.DueDate,
        //                            Label = dd.Label,
        //                            CreatedBy = rule.CreatedBy, // Replace with actual CreatedBy
        //                            CreatedOn = DateTime.Now,
        //                            UID = Guid.NewGuid(),
        //                            FrequencyType = dd.FrequencyType,
        //                            FromTrunOver = dd.FromTrunOver,
        //                            ToTrunOver = dd.ToTrunOver,
        //                            ForTheMonth = dd.ForTheMonth,
        //                            Status = dd.Status
        //                        }).ToList();

        //                        // Insert new due dates
        //                        if (updatedueDateEntities.Count() > 0)
        //                        {
        //                            dbContext.TOCDueDates.UpdateRange(updatedueDateEntities);
        //                        }

        //                    }

        //               // }
        //            }

        //            // Save changes
        //            await dbContext.SaveChangesAsync();

        //            // Commit the transaction if all operations succeeded
        //            await transaction.CommitAsync();
        //        }
        //        catch (Exception)
        //        {
        //            // Rollback the transaction if any operation failed
        //            await transaction.RollbackAsync();
        //            throw; // Rethrow the exception to propagate the error
        //        }
        //    }
        //}

        private async Task SaveTOCDueDatesAsync(TOCRules rule, long duesId)
        {
            using (var transaction = await dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    foreach (var due in rule.TOCDues)
                    {
                        if (rule.TOCDues != null && due.TOCDueDates != null)
                        {
                            // === INSERT NEW DUE DATES ===
                            var newDueDateEntities = due.TOCDueDates
                                .Where(d => d.Id == 0)
                                .Select(dd => new TOCDueDates
                                {
                                    TOCDuesId = due.Id,
                                    DueDate = dd.DueDate,
                                    Label = dd.Label,
                                    CreatedBy = rule.CreatedBy,
                                    CreatedOn = DateTime.Now,
                                    UID = Guid.NewGuid(),
                                    FrequencyType = dd.FrequencyType,
                                    FromTrunOver = dd.FromTrunOver,
                                    ToTrunOver = dd.ToTrunOver,
                                    ForTheMonth = dd.ForTheMonth,
                                    Status = dd.Status
                                }).ToList();

                            if (newDueDateEntities.Any())
                            {
                                await dbContext.TOCDueDates.AddRangeAsync(newDueDateEntities);
                                await dbContext.SaveChangesAsync(); // So new IDs are generated
                            }

                            // === UPDATE EXISTING DUE DATES ===
                            var updateDueDateEntities = due.TOCDueDates
                                .Where(d => d.Id > 0)
                                .Select(dd => new TOCDueDates
                                {
                                    Id = dd.Id,
                                    TOCDuesId = due.Id,
                                    DueDate = dd.DueDate,
                                    Label = dd.Label,
                                    CreatedBy = rule.CreatedBy,
                                    CreatedOn = DateTime.Now,
                                    UID = Guid.NewGuid(),
                                    FrequencyType = dd.FrequencyType,
                                    FromTrunOver = dd.FromTrunOver,
                                    ToTrunOver = dd.ToTrunOver,
                                    ForTheMonth = dd.ForTheMonth,
                                    Status = dd.Status
                                }).ToList();

                            if (updateDueDateEntities.Any())
                            {
                                dbContext.TOCDueDates.UpdateRange(updateDueDateEntities);
                                await dbContext.SaveChangesAsync();
                            }

                            // === HANDLE TOC DUE MONTHS ===
                            foreach (var dd in due.TOCDueDates)
                            {
                                long tocDueDateId = dd.Id;

                                // If this is a new due date just added above, find its new Id
                                if (tocDueDateId == 0)
                                {
                                    var matched = newDueDateEntities.FirstOrDefault(e =>
                                        e.FrequencyType == dd.FrequencyType &&
                                        e.FromTrunOver == dd.FromTrunOver &&
                                        e.ToTrunOver == dd.ToTrunOver &&
                                        e.ForTheMonth == dd.ForTheMonth &&
                                        e.DueDate == dd.DueDate
                                    );
                                    if (matched != null)
                                        tocDueDateId = matched.Id;
                                }

                                if (dd.TocDueMonths != null && tocDueDateId > 0)
                                {
                                    var newMonthEntities = dd.TocDueMonths
                                        .Where(m => m.Id == 0)
                                        .Select(m => new TOCDueDates
                                        {
                                            ParentTocDueDateId = tocDueDateId,
                                            FrequencyType = m.FrequencyType,
                                            FromTrunOver = m.FromTrunOver,
                                            ToTrunOver = m.ToTrunOver,
                                            ForTheMonth = m.ForTheMonth,
                                            DueDate = m.DueDate,
                                            Label = m.Label,
                                            Status = m.Status,
                                            CreatedBy = rule.CreatedBy,
                                            CreatedOn = DateTime.Now,
                                            UID = Guid.NewGuid()
                                        }).ToList();

                                    if (newMonthEntities.Any())
                                    {
                                        await dbContext.TOCDueDates.AddRangeAsync(newMonthEntities);
                                    }

                                    var updateMonthEntities = dd.TocDueMonths
                                        .Where(m => m.Id > 0)
                                        .Select(m => new TOCDueDates
                                        {
                                            Id = m.Id,
                                            ParentTocDueDateId = tocDueDateId,
                                            FrequencyType = m.FrequencyType,
                                            FromTrunOver = m.FromTrunOver,
                                            ToTrunOver = m.ToTrunOver,
                                            ForTheMonth = m.ForTheMonth,
                                            DueDate = m.DueDate,
                                            Label = m.Label,
                                            Status = m.Status,
                                            CreatedBy = rule.CreatedBy,
                                            CreatedOn = DateTime.Now,
                                            UID = Guid.NewGuid()
                                        }).ToList();

                                    if (updateMonthEntities.Any())
                                    {
                                        dbContext.TOCDueDates.UpdateRange(updateMonthEntities);
                                    }
                                }
                            }
                        }
                    }

                    await dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        private async Task SaveOrUpdateTOCIntrestPenalitiesAsync(TOCRules rule, long? complianceId)
        {
            if (rule.TOCIntrestPenality != null && rule.TOCIntrestPenality.Any())
            {
                foreach (var penality in rule.TOCIntrestPenality)
                {
                    if (penality.Id > 0)
                    {
                        // Update existing TOCIntrestPenality
                        var existingPenality = await dbContext.TOCIntrestPenality.FindAsync(penality.Id);

                        if (existingPenality != null)
                        {
                            existingPenality.ComplianceId = complianceId;
                            existingPenality.RegulationSetupId = rule.RegulationSetupId;
                            existingPenality.TOCRuleType = rule.RuleType;
                            existingPenality.SectionName = penality.SectionName;
                            existingPenality.IntrestType = penality.IntrestType;
                            existingPenality.IntrestRate = penality.IntrestRate;
                            existingPenality.IntrestRateFrequency = penality.IntrestRateFrequency;
                            existingPenality.Period = penality.Period;
                            existingPenality.PenalityRate = penality.PenalityRate;
                            existingPenality.PenalityRateFrequency = penality.PenalityRateFrequency;
                            existingPenality.ModifiedBy = rule.CreatedBy; // Replace with actual ModifiedBy
                            existingPenality.ModifiedOn = DateTime.Now;

                            dbContext.TOCIntrestPenality.Update(existingPenality);
                        }
                        else
                        {
                            throw new Exception($"TOCIntrestPenality with Id {penality.Id} not found.");
                        }
                    }
                    else
                    {
                        // Add new TOCIntrestPenality if Id is null or 0
                        var newPenality = new TOCIntrestPenality
                        {
                            ComplianceId = complianceId,
                            RegulationSetupId = rule.RegulationSetupId,
                            TOCRuleType = rule.RuleType,
                            SectionName = penality.SectionName,
                            IntrestType = penality.IntrestType,
                            IntrestRate = penality.IntrestRate,
                            IntrestRateFrequency = penality.IntrestRateFrequency,
                            Period = penality.Period,
                            PenalityRate = penality.PenalityRate,
                            PenalityRateFrequency = penality.PenalityRateFrequency,
                            CreatedBy = rule.CreatedBy, // Replace with actual CreatedBy
                            CreatedOn = DateTime.Now,
                            UID = Guid.NewGuid()
                        };

                        dbContext.TOCIntrestPenality.Add(newPenality);
                    }
                }

                await dbContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception("TOCIntrestPenality list in TOCRules is null or empty.");
            }
        }

        private async Task SaveOrUpdateTOCImprisonmentsAsync(TOCRules rule, long? complianceId)
        {
            if (rule.TOCImprisonment != null && rule.TOCImprisonment.Any())
            {
                foreach (var imprisonment in rule.TOCImprisonment)
                {
                    if (imprisonment.Id > 0)
                    {
                        // Update existing TOCImprisonment
                        var existingImprisonment = await dbContext.TOCImprisonment.FindAsync(imprisonment.Id);

                        if (existingImprisonment != null)
                        {
                            existingImprisonment.ComplianceId = complianceId;
                            existingImprisonment.RegulationSetupId = rule.RegulationSetupId;
                            existingImprisonment.TOCRuleType = rule.RuleType;
                            existingImprisonment.SectionName = imprisonment.SectionName;
                            existingImprisonment.DurationFrom = imprisonment.DurationFrom;
                            existingImprisonment.DurationTo = imprisonment.DurationTo;
                            existingImprisonment.DurationType = imprisonment.DurationType;
                            existingImprisonment.ForWhome = imprisonment.ForWhome;
                            existingImprisonment.ModifiedBy = rule.CreatedBy; // Replace with actual ModifiedBy
                            existingImprisonment.ModifiedOn = DateTime.Now;

                            dbContext.TOCImprisonment.Update(existingImprisonment);
                        }
                        else
                        {
                            throw new Exception($"TOCImprisonment with Id {imprisonment.Id} not found.");
                        }
                    }
                    else
                    {
                        // Add new TOCImprisonment if Id is null or 0
                        var newImprisonment = new TOCImprisonment
                        {
                            ComplianceId = complianceId,
                            RegulationSetupId = rule.RegulationSetupId,
                            TOCRuleType = rule.RuleType,
                            SectionName = imprisonment.SectionName,
                            DurationFrom = imprisonment.DurationFrom,
                            DurationTo = imprisonment.DurationTo,
                            DurationType = imprisonment.DurationType,
                            ForWhome = imprisonment.ForWhome,
                            CreatedBy = rule.CreatedBy, // Replace with actual CreatedBy
                            CreatedOn = DateTime.Now,
                            UID = Guid.NewGuid()
                        };

                        dbContext.TOCImprisonment.Add(newImprisonment);
                    }
                }

                await dbContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception("TOCImprisonment list in TOCRules is null or empty.");
            }
        }

        private async Task SaveTOCParametersAsync(List<TOCParameter> parameters, long? complianceId, string TOCRuleType, long? createdBy, long? RegulationSetupId)
        {
            // Start a new transaction
            using (var transaction = await dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    // Fetch existing parameters based on ComplianceId and TOCRuleType
                    var existingParameters = dbContext.TOCParameter
                        .Where(p => p.ComplianceId == complianceId && p.TOCRuleType == TOCRuleType);

                    // Delete existing parameters if any
                    if (existingParameters != null && existingParameters.Any())
                    {
                        dbContext.TOCParameter.RemoveRange(existingParameters);
                    }

                    // Prepare new parameters to insert
                    var parameterEntities = parameters.Select(p => new TOCParameter
                    {
                        ComplianceId = complianceId,
                        RegulationSetupId = RegulationSetupId,
                        TOCRuleType = TOCRuleType,
                        ParameterTypeId = p.ParameterTypeId,
                        ParameterTypeValue = p.ParameterTypeValue,
                        ParameterOperator = p.ParameterOperator,
                        CreatedBy = createdBy,
                        CreatedOn = DateTime.Now,
                        UID = Guid.NewGuid()
                    }).ToList();

                    // Insert new parameters
                    dbContext.TOCParameter.AddRange(parameterEntities);

                    // Save changes
                    await dbContext.SaveChangesAsync();

                    // Commit the transaction if all operations succeeded
                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    // Rollback the transaction if any operation failed
                    await transaction.RollbackAsync();
                    throw; // Rethrow the exception to propagate the error
                }
            }
        }

        public async Task<object> GetTOCHistory(long historyId)
        {
            var history = await dbContext.TOCHistory.FirstOrDefaultAsync(f => f.HistoryId == historyId);
            if (history == null)
                return null;

            // Get CAA mapping for this TOC history
            var caaMapping = await dbContext.RegulationSetupCAAMappingHistory
                .FirstOrDefaultAsync(x => x.TocId == historyId);

            ConcernedMinistry concernedMinistry = null!;
            RegulatoryAuthorities regulatoryAuthority = null!;

            if (caaMapping?.ConcernedMinistryId != null)
            {
                var cm = await dbContext.ConcernedMinistries
                    .FirstOrDefaultAsync(m => m.Id == caaMapping.ConcernedMinistryId);
                if (cm != null)
                {
                    concernedMinistry = new ConcernedMinistry
                    {
                        Id = cm.Id,
                        ConcernedMinistryCode = cm.ConcernedMinistryCode,
                        ConcernedMinistryName = cm.ConcernedMinistryName,
                        ConcernedMinistryReferenceCode = cm.ConcernedMinistryReferenceCode,
                        CreatedBy = cm.CreatedBy,
                        CreatedOn = cm.CreatedOn,
                        Status = cm.Status,
                        ModifiedOn = cm.ModifiedOn,
                        UID = cm.UID
                    };
                }
            }
            if (caaMapping?.RegulatoryAuthorityId != null)
            {
                var ra = await dbContext.RegulatoryAuthority
                    .FirstOrDefaultAsync(a => a.Id == caaMapping.RegulatoryAuthorityId);
                if (ra != null)
                {
                    regulatoryAuthority = new ComplianceAPI.Models.DataModels.RegulatoryAuthorities
                    {
                        Id = ra.Id,
                        RegulatoryAuthorityCode = ra.RegulatoryAuthorityCode,
                        RegulatoryAuthorityReferenceCode = ra.RegulatoryAuthorityReferenceCode,
                        RegulatoryAuthorityName = ra.RegulatoryAuthorityName
                    };
                }
            }

            // Load all RegulationSetupIndustryTOBHistory records for this TOCId
            var industry = dbContext.RegulationSetupIndustryTOBHistory
                .Where(f => f.TOCId == history.HistoryId)
                .ToList();

            var majorIds = industry.Select(r => r.MajorIndustryId).Distinct().ToList();
            var minorIds = industry.Select(r => r.MinorIndustryId).Distinct().ToList();
            var tobIds = industry.Where(r => r.TOBId != null).Select(r => r.TOBId!.Value).Distinct().ToList();

            // Preload lookup dictionaries
            var majorDict = dbContext.MajorIndustries
                .Where(m => majorIds.Contains(m.Id))
                .ToDictionary(m => m.Id, m => m.MajorIndustryName);

            var minorDict = dbContext.MinorIndustries
                .Where(m => minorIds.Contains(m.Id))
                .ToDictionary(m => m.Id, m => m.MinorIndustryName);

            var tobDict = dbContext.TOBDetails
                .Where(t => tobIds.Contains(t.Id))
                .ToDictionary(t => t.Id, t => t.TOBName);

            // Build the hierarchy
            var industryHierarchy = industry
                .GroupBy(r => r.MajorIndustryId)
                .Select(majorGroup => new
                {
                    MajorIndustryId = majorGroup.Key,
                    MajorIndustryName = majorGroup.Key != null && majorDict.ContainsKey(majorGroup.Key)
                        ? majorDict[majorGroup.Key]
                        : null,
                    MinorIndustries = majorGroup
                        .GroupBy(m => m.MinorIndustryId)
                        .Select(minorGroup => new
                        {
                            MinorIndustryId = minorGroup.Key,
                            MinorIndustryName = minorGroup.Key != null && minorDict.ContainsKey(minorGroup.Key)
                                ? minorDict[minorGroup.Key]
                                : null,
                            TOBs = minorGroup
                                .Where(t => t.TOBId != null)
                                .Select(t => new
                                {
                                    TOBId = t.TOBId!.Value,
                                    TOBName = tobDict.ContainsKey(t.TOBId.Value) ? tobDict[t.TOBId.Value] : null
                                })
                                .Distinct()
                                .ToList()
                        })
                        .ToList()
                })
                .ToList();
            var legalEntityTypes = await (
            from legalEntity in dbContext.RegulationSetupLegalEntityTypeHistory
            join entity in dbContext.EntityType on legalEntity.LegalEntityType equals entity.Id into entityJoin
            from entity in entityJoin.DefaultIfEmpty()
            where legalEntity.TocId == history.HistoryId
            select new RegulationSetupLegalEntityTypeDto
            {
                RegulationSetupId = legalEntity.RegulationSetupId,
                ComplianceId = legalEntity.ComplianceId,
                LegalEntityType = legalEntity.LegalEntityType,
                EntityName = entity.EntityType,
                UID = legalEntity.UID,
                Id = legalEntity.HistoryId,
                CreatedBy = legalEntity.CreatedBy,
            }
        ).ToListAsync();

            return new
            {
                History = history,
                Industry = industryHierarchy,
                LegalEntityType = legalEntityTypes,
                ConcernedMinistry = concernedMinistry,
                RegulatoryAuthority = regulatoryAuthority
            };
        }

        public async Task<Response> ApproveTOCRegistration(AccessModel access)
        {
            try
            {
                if (access != null)
                {
                    var accessresult = await dbContext.TOCApproval.Where(a => a.UID == access.UID).FirstOrDefaultAsync();

                    var history = await dbContext.TOCHistory.Where(h => h.HistoryId == accessresult.HistoryId).FirstOrDefaultAsync();

                    var JsonResult = JsonHelper.DeserializeFromJson<TOCRegistrationModel>(history.TOCHistoryJson);
                    await addTOCRegistrationAsync(JsonResult);
                    accessresult.ApprovalStatus = RefApprovalStatus.Approved;
                    accessresult.ModifiedBy = access.CreatedBy;
                    accessresult.ModifiedOn = DateTime.Now;
                    dbContext.TOCApproval.Update(accessresult);
                    dbContext.SaveChanges();
                }
                return new Response() { ResponseCode = 1, ResponseMessage = "Approved Successfull" }; ;
            }
            catch (Exception ex)
            {
                return new Response() { ResponseCode = 0, ResponseMessage = "Failed to Approved" };
            }
        }

        public async Task<Response> RejectTOC(AccessModel access)
        {
            try
            {
                if (access != null)
                {
                    var accessresult = await dbContext.TOCApproval.Where(a => a.UID == access.UID).FirstOrDefaultAsync();

                    accessresult.ApprovalStatus = RefApprovalStatus.Rejected;
                    accessresult.ModifiedBy = access.CreatedBy;
                    accessresult.ModifiedOn = DateTime.Now;
                    dbContext.TOCApproval.Update(accessresult);
                }
                return new Response() { ResponseCode = 1, ResponseMessage = "Rejected Successfull" };
            }
            catch (Exception ex)
            {
                return new Response() { ResponseCode = 0, ResponseMessage = "Failed to Rejected" }; ;
            }
        }

        public async Task<Response> ApproveTOCRule(AccessModel access)
        {
            try
            {
                if (access != null)
                {
                    var accessresult = await dbContext.TOCApproval.Where(a => a.UID == access.UID).FirstOrDefaultAsync();

                    var history = await dbContext.TOCHistory.Where(h => h.HistoryId == accessresult.HistoryId).FirstOrDefaultAsync();
                    var JsonResult = JsonHelper.DeserializeFromJson<TOCRules>(history.TOCHistoryJson);
                    await addTOCRulesAsync(JsonResult, history.HistoryId);
                    accessresult.ApprovalStatus = RefApprovalStatus.Approved;
                    accessresult.ModifiedBy = access.CreatedBy;
                    accessresult.ModifiedOn = DateTime.Now;
                    dbContext.TOCApproval.Update(accessresult);
                    dbContext.SaveChanges();
                }
                return new Response() { ResponseCode = 1, ResponseMessage = "Approved Successfull" }; ;
            }
            catch (Exception ex)
            {
                return new Response() { ResponseCode = 0, ResponseMessage = "Failed to Approved" };
            }
        }

        public async Task<bool> AddRegulationSetupApprovalNotification(long createdBy, string regulationSetupName)
        {
            try
            {
                var createdByUserDetails = await helperRepository.GetUserAndManagerInfoAsync(createdBy);
                if (createdByUserDetails != null)
                {
                    var notification = new Notification
                    {
                        NotificationId = Guid.NewGuid().ToString(),
                        SenderUserId = createdByUserDetails.UserId,
                        SenderUserName = createdByUserDetails.UserName,
                        RecipientUserId = createdByUserDetails.ManagerId,
                        RecipientUserName = createdByUserDetails.ManagerName,
                        CreatedDate = DateTime.UtcNow,
                        ReadDate = null,
                        MarkAsRead = false,
                        ModuleType = Models.Enums.ModuleType.RegulationSetup
                    };
                    if (createdByUserDetails.UserRoleId == 1)
                    {
                        notification.NotificationTitle = string.Format(ApiConstants.NewRegulationSetupBySuperAdminNotificationTitleTemplate, regulationSetupName);
                        notification.NotificationMessage = string.Format(ApiConstants.NewRegulationSetupBySuperAdminNotificationMessageTemplate, regulationSetupName);
                        notification.Status = Models.Enums.RefApprovalStatus.Approved;
                    }
                    else
                    {
                        notification.NotificationTitle = string.Format(ApiConstants.NewRegulationSetupNotificationTitleTemplate, createdByUserDetails.UserName, regulationSetupName);
                        notification.NotificationMessage = string.Format(ApiConstants.NewRegulationSetupNotificationMessageTemplate, createdByUserDetails.UserName, regulationSetupName);
                        notification.Status = Models.Enums.RefApprovalStatus.Pending;
                    }
                    await dbContext.AddAsync(notification);
                    await dbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> RegulationSetupApprovalResponseNotification(long createdBy, string approvalStatus)
        {
            try
            {
                var userInfo = await helperRepository.GetUserAndManagerInfoAsync(createdBy);
                if (userInfo != null)
                {
                    var notification = new Notification
                    {
                        NotificationId = Guid.NewGuid().ToString(),
                        SenderUserId = userInfo.ManagerId,
                        SenderUserName = userInfo.ManagerName,
                        RecipientUserId = userInfo.UserId,
                        RecipientUserName = userInfo.UserName,
                        CreatedDate = DateTime.UtcNow,
                        ReadDate = null,
                        MarkAsRead = false,
                        ModuleType = Models.Enums.ModuleType.RegulationGroup
                    };
                    if (approvalStatus == Models.Enums.RefApprovalStatus.Approved.ToString())
                    {
                        notification.Status = Models.Enums.RefApprovalStatus.Approved;
                        notification.NotificationTitle = $"<b>{userInfo.ManagerName}</b> approved a <b>New Regulation Setup</b>";
                        notification.NotificationMessage = $"<b>{userInfo.ManagerName}</b> approved a <b>New Regulation Setup</b>";
                    }
                    else if (approvalStatus == Models.Enums.RefApprovalStatus.Rejected.ToString())
                    {
                        notification.Status = Models.Enums.RefApprovalStatus.Rejected;
                        notification.NotificationTitle = $"<b>{userInfo.ManagerName}</b> rejected the <b>New Regulation Setup</b>";
                        notification.NotificationMessage = $"<b>{userInfo.ManagerName}</b> rejected the <b>New Regulation Setup</b>";
                    }
                    await dbContext.AddAsync(notification);
                    await dbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<List<RegulationListModel>> GetRegulationsByCountryIds(List<long> countryIds)
        {
            try
            {
                var regulations = await dbContext.RegulationSetupDetails
                .Where(r => r.CountryId != null && countryIds.Contains(r.CountryId.Value))
                .Select(r => new RegulationListModel
                {
                    Id = r.Id,
                    RegulationName = r.RegulationName,
                    RuleType = r.RegulationType
                })
                .ToListAsync();
                return regulations;
            }
            catch (Exception ex)
            {
                return new List<RegulationListModel>();
            }
        }

        public async Task<string> GetNextRegulationSetupCode()
        {
            using (var connection = unitOfWork.ConnectionFactory())
            {
                var count = await connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM [product_owner].[RegulationSetupDetails]");
                int nextNumber = count + 1;

                return nextNumber.ToString("D3");
            }
        }

        public async Task<string> GetNextRegulationSetupComplianceCode()
        {
            using (var connection = unitOfWork.ConnectionFactory())
            {
                var maxId = await connection.ExecuteScalarAsync<long>(
                    "SELECT ISNULL(MAX(ID), 0) FROM [product_owner].[RegulationSetupCompliance]");
                long nextNumber = maxId + 1;
                return nextNumber.ToString("D3");
            }
        }

        public async Task<List<RegulationSetupLegalEntityTypeDto>> GetLegalEntityTypesByRegulationSetupId(long regulationSetupId)
        {
            var response = await (
                from legalEntity in dbContext.RegulationSetupLegalEntityType
                join entity in dbContext.EntityType on legalEntity.LegalEntityType equals entity.Id into entityJoin
                from entity in entityJoin.DefaultIfEmpty()
                where legalEntity.RegulationSetupId == regulationSetupId && legalEntity.ComplianceId == null && legalEntity.TocId == null && legalEntity.TocRegisterationId == null
                select new RegulationSetupLegalEntityTypeDto
                {
                    RegulationSetupId = legalEntity.RegulationSetupId,
                    LegalEntityType = legalEntity.LegalEntityType,
                    EntityName = entity.EntityType,
                    UID = legalEntity.UID,
                    Id = legalEntity.Id,
                    CreatedBy = legalEntity.CreatedBy
                }
            ).ToListAsync();

            return response;
        }

        public async Task<List<RegulationSetupLegalEntityTypeDto>> GetLegalEntityTypesByComplianceId(long complianceId)
        {
            var response = await (
                from legalEntity in dbContext.RegulationSetupLegalEntityType
                join entity in dbContext.EntityType on legalEntity.LegalEntityType equals entity.Id into entityJoin
                from entity in entityJoin.DefaultIfEmpty()
                where legalEntity.ComplianceId == complianceId
                select new RegulationSetupLegalEntityTypeDto
                {
                    RegulationSetupId = legalEntity.RegulationSetupId,
                    LegalEntityType = legalEntity.LegalEntityType,
                    EntityName = entity.EntityType,
                    UID = legalEntity.UID,
                    Id = legalEntity.Id,
                    CreatedBy = legalEntity.CreatedBy
                }
            ).ToListAsync();

            return response;
        }

        private async Task AddOrUpdateRegulationSetupIndustryTOB(RegSetupComplianceModel compliance, long complianceid)
        {
            if (compliance != null && compliance.Industry != null && compliance.Industry.Count != 0)
            {
                foreach (var MJ in compliance.Industry)
                {
                    foreach (var MI in MJ.MinorIndustries!)
                    {
                        var alreadyExist = await dbContext.RegulationSetupIndustryTOB
                            .FirstOrDefaultAsync(c => c.ComplianceId == complianceid && c.MajorIndustryId == MJ.MajorIndustryId && c.MinorIndustryId == MI.MinorIndustryId);

                        // Check if there are no TOBs
                        if ((MI.TOBs == null || !MI.TOBs.Any()))
                        {
                            if (alreadyExist == null)
                            {
                                // Insert a row with TOBId set to null
                                var InTob = new RegulationSetupIndustryTOB
                                {
                                    ComplianceId = complianceid,
                                    MajorIndustryId = MJ.MajorIndustryId,
                                    MinorIndustryId = MI.MinorIndustryId,
                                    TOBId = null, // Set TOBId to null
                                    CreatedBy = compliance.CreatedBy,
                                    CreatedOn = DateTime.Now
                                };
                                dbContext.RegulationSetupIndustryTOB.Add(InTob);
                            }
                        }
                        else
                        {
                            // Iterate through each TOB and insert rows
                            foreach (var tobr in MI!.TOBs.Select(c => c.TOBId))
                            {
                                if (alreadyExist == null || (alreadyExist != null && alreadyExist.TOBId != tobr))
                                {
                                    var InTob = new RegulationSetupIndustryTOB
                                    {
                                        ComplianceId = complianceid,
                                        MajorIndustryId = MJ.MajorIndustryId,
                                        MinorIndustryId = MI.MinorIndustryId,
                                        TOBId = tobr,
                                        CreatedBy = compliance.CreatedBy,
                                        CreatedOn = DateTime.Now
                                    };
                                    dbContext.RegulationSetupIndustryTOB.Add(InTob);
                                }
                            }

                            await dbContext.SaveChangesAsync();
                        }
                    }
                }
            }
        }

        private async Task AddOrUpdateLegalEntityType(RegSetupComplianceModel compliance, long complianceid)
        {
            if (compliance != null && compliance.LegalEntityType != null && compliance.LegalEntityType.Count > 0)
            {
                var item = new RegulationSetupLegalEntityType();
                foreach (var entity in compliance.LegalEntityType)
                {
                    var existingEntities = await dbContext.RegulationSetupLegalEntityType
                        .FirstOrDefaultAsync(x => x.ComplianceId == complianceid && x.LegalEntityType == entity.LegalEntityType);
                    if (existingEntities == null)
                    {
                        item.ComplianceId = complianceid;
                        item.LegalEntityType = entity.LegalEntityType;
                        item.CreatedOn = DateTime.Now;
                        item.UID = Guid.NewGuid();
                        item.CreatedBy = compliance.CreatedBy;
                        item.ModifiedBy = entity.ModifiedBy;
                        item.ModifiedOn = entity.ModifiedOn;
                        dbContext.RegulationSetupLegalEntityType.Add(item);
                    }
                }
            }
        }

        public async Task<string> GetNextRegulationSetupTypeOfComplianceRegisterCode()
        {
            using (var connection = unitOfWork.ConnectionFactory())
            {
                var maxId = await connection.ExecuteScalarAsync<long>(
                    "SELECT ISNULL(MAX(ID), 0) FROM [product_owner].[TOCRegistration]");
                long nextNumber = maxId + 1;
                return nextNumber.ToString("D3");
            }
        }

        public async Task<string> GetNextRegulationSetupTypeOfComplianceDuesCode()
        {
            using (var connection = unitOfWork.ConnectionFactory())
            {
                var maxId = await connection.ExecuteScalarAsync<long>(
                    "SELECT ISNULL(MAX(ID), 0) FROM [product_owner].[TOCDues]");
                long nextNumber = maxId + 1;
                return nextNumber.ToString("D3");
            }
        }

        ///<see cref="IRegulationSetupRepository.GetTypeOfCompliances()/>
        public async Task<List<TypeOfCompliance>> GetTypeOfCompliances()
        {
            using (var connection = unitOfWork.ConnectionFactory())
            {
                var res = await connection.QueryAsync<TypeOfCompliance>(
                    @"SELECT
                        t.Id,
                        t.RegulationSetupId,
                        t.ComplianceId,
                        CASE
                            WHEN t.RegulationSetupId IS NOT NULL
                                THEN r.RegulationName + '-' + t.TOCRuleType + '-' + t.Frequency + '-' + CAST(t.FromTrunOver AS VARCHAR) + '-' + CAST(t.ToTrunOver AS VARCHAR)
                            WHEN t.ComplianceId IS NOT NULL
                                THEN c.ComplianceName + '-' + t.TOCRuleType + '-' + t.Frequency + '-' + CAST(t.FromTrunOver AS VARCHAR) + '-' + CAST(t.ToTrunOver AS VARCHAR)
                            ELSE NULL
                        END AS TypeOfComplianceName
                    FROM [ComplianceNew].[product_owner].[TOCDues] t
                    LEFT JOIN [ComplianceNew].[product_owner].[RegulationSetupDetails] r
                        ON t.RegulationSetupId = r.Id
                    LEFT JOIN [ComplianceNew].[product_owner].[RegulationSetupCompliance] c
                        ON t.ComplianceId = c.Id
                    ");

                return res.ToList();
            }
        }

        public async Task<List<TOCDues>> GetAllTOCDuesAsync()
        {
            return await dbContext.TOCDues.ToListAsync();
        }
    }
}