using ComplianceAPI.Models;
using ComplianceAPI.Models.DataModels;
using ComplianceAPI.Repository;
using MinorIndustry = ComplianceAPI.Models.DataModels.MinorIndustry;
using RefApprovalType = ComplianceAPI.Helpers.RefApprovalType;

namespace ComplianceAPI.Services
{
    public interface IRegulationSetupService
    {
        Task<RegulationStupDetails> GetRegulationSetupDetails(Guid? regSetupuid);
        Task<List<RegulationSetupDetails>> GetAllRegulationSetupDetails();

        Task<List<ReuglationListModle>> GetRegSetupHistory();

        Task<RegulationStupDetails> GetHistoryRegulationSetup(Guid? UID);

        Task<List<RegulationSetupParameter>> GetAllRegulationSetupParameter();

        Task<RegulationStupDetails> AddRegulationSetupDetails(RegulationStupDetails regulationStupDetails);

        Task<RegulationStupDetails> ApproveRegulationSetup(AccessModel access);

        Task<RegulationStupDetails> RejectRegulationSetup(AccessModel access);

        Task<List<MinorIndustry>> GetMinorIndustrybyMajorID(long majorIndustoryId);

        Task<List<IndustryrMapping>> GetMinorIndustrybyMajorIDMap(List<long> majorIndustoryId, long countryId);

        Task<List<IndustryrMapping>> GetTOBMinorIndustrybyMajorIDMap(List<long> majorIndustoryId, List<long> minorIndustoryId, long countryId);

        Task<List<IndustryMappings>> GetIndustryMapping(long countryId);

        Task<bool> PostParameterReviewAccess(AccessModel access);

        Task<bool> PostParameterApproveAccess(AccessModel access);

        Task<bool> PostParameterRejectAccess(AccessModel access);

        Task<List<TOCDues>> GetAllTOCDuesAsync();

        Task<List<TOCRegistration>> GetTOCRegisteration();
        //Task<RegulationSetupParameters> AddRegulationSetupParameters(AddRegulationStupParameters regulationStupParameters, Guid? accessUID);

        Task<RegSetupComplianceModel> GetRegulationSetupCompliance(Guid? complianceuid);

        Task<RegSetupComplianceModel> AddRegSetupComplianceAsync(RegSetupComplianceModel compliance);

        Task<GetRegulationBasicAndParameterByCountryID> GetAllRegulationBasicAndParameterByCountryID(int countryId);

        Task<List<PendingApproval>> GetPendingRegulationSetupApproval(Guid? UserUID);

        Task<RegSetupComplianceModel> GetRegSetupComplianceHistory(Guid? Uid);

        Task<RegSetupComplianceModel> ApproveRegSetupCompliance(AccessModel access);

        Task<RegSetupComplianceModel> RejectRegSetupCompliance(AccessModel access);

        Task<TOCRegistrationModel> GetTOCRegistration(long tocRegId, long? complianceId, long? regulationid, string ruleType);

        Task<TOCRegistrationModel> SaveTOCRegistrationAsync(TOCRegistrationModel TOC);

        Task<TOCRules> GetTOCRulesAsync(long complianceId, long regulationid, string ruleType);

        Task<TOCRules> PostTOCRules(TOCRules TOC);

        Task<object> GetTOCHistory(long historyId);

        Task<Response> ApproveTOCRegistration(AccessModel access);

        Task<Response> RejectTOC(AccessModel access);

        Task<Response> ApproveTOCRule(AccessModel access);

        Task<List<RegulationListModel>> GetRegulationsByCountryIds(string countryIds);

        Task<string> GetNextRegulationSetupCode();

        Task<string> GetNextRegulationSetupComplianceCode();

        Task<string> GetNextRegulationSetupTypeOfComplianceRegisterCode();

        Task<string> GetNextRegulationSetupTypeOfComplianceDuesCode();

        Task<List<TypeOfCompliance>> GetTypeOfCompliances();
    }

    public class RegulationSetupService : IRegulationSetupService
    {
        private readonly IRegulationSetupRepository _regulationsetupRepository;
        private readonly IHelperRepository helperRepository;

        public RegulationSetupService(IRegulationSetupRepository regulationSetupRepository, IHelperRepository helperRepository)
        {
            _regulationsetupRepository = regulationSetupRepository;
            this.helperRepository = helperRepository;
        }

        public async Task<RegulationStupDetails> GetRegulationSetupDetails(Guid? regSetupuid)
        {
            return await _regulationsetupRepository.GetAllRegulationSetupDetails(regSetupuid);
        }
        public async Task<List<RegulationSetupDetails>> GetAllRegulationSetupDetails()
        {
            return await _regulationsetupRepository.GetAllRegulationSetupDetails();
        }

        public async Task<List<ReuglationListModle>> GetRegSetupHistory()
        {
            return await _regulationsetupRepository.GetRegSetupHistory();
        }

        public async Task<RegulationStupDetails> GetHistoryRegulationSetup(Guid? UID)
        {
            return await _regulationsetupRepository.GetHistoryRegulationSetup(UID);
        }

        public async Task<bool> PostParameterReviewAccess(AccessModel access)
        {
            return await _regulationsetupRepository.PostRegulationSetupReviewAccess(access);
        }

        public async Task<bool> PostParameterApproveAccess(AccessModel access)
        {
            return await _regulationsetupRepository.PostRegulationSetupApproveAccess(access);
        }

        public async Task<bool> PostParameterRejectAccess(AccessModel access)
        {
            return await _regulationsetupRepository.PostRegulationSetupRejectAccess(access);
        }

        public async Task<List<RegulationSetupParameter>> GetAllRegulationSetupParameter()
        {
            return await _regulationsetupRepository.GetAllRegulationSetupParameter();
        }

        public async Task<RegulationStupDetails> AddRegulationSetupDetails(RegulationStupDetails regulationStupDetails)

        {
            //if (helperRepository.IsSuperAdmin((int)regulationStupDetails.CreatedBy))
            //{
            //    await _regulationsetupRepository.AdminAddRegulationStupDetails(regulationStupDetails);
            //}
            var result = await _regulationsetupRepository.AddRegulationStupDetails(regulationStupDetails);
            AccessModel access = new AccessModel() { ApprovalTypeId = RefApprovalType.User, CreatedBy = regulationStupDetails.CreatedBy, ManagerId = regulationStupDetails.ApprovalManagerId == null ? 0 : regulationStupDetails.ApprovalManagerId, Status = 0, UserId = result.Id, HistoryId = result.HistoryId };
            await _regulationsetupRepository.PostUpdateRegulationStupApproval(access);
            await _regulationsetupRepository.AddRegulationSetupApprovalNotification(regulationStupDetails.CreatedBy.Value, regulationStupDetails.RegulationName);
            return result;
        }

        //public async Task<RegulationSetupParameters> AddRegulationSetupParameters(AddRegulationStupParameters regulationStupParameters, Guid? accessUID)
        //{
        //    if (helperRepository.IsSuperAdmin((int)regulationStupParameters.CreatedBy))
        //    {
        //        await _regulationsetupRepository.AdminAddRegulationStupParameters(regulationStupParameters);
        //    }
        //    var result = await _regulationsetupRepository.AddRegulationStupParameters(regulationStupParameters);
        //    //if (accessUID == null)
        //    //{
        //    //    AccessModel access = new AccessModel() { ApprovalTypeId = RefApprovalType.User, CreatedBy = parameter.CreatedBy, ManagerId = parameter.ApprovalManagerId == null ? 0 : parameter.ApprovalManagerId, Status = 0, UserId = result.Id, HistoryId = result.HistoryId };
        //    //    await _parameterRepository.PostUpdateParameterApproval(access);
        //    //}

        //    return result;
        //}
        public async Task<RegulationStupDetails> ApproveRegulationSetup(AccessModel access)
        {
            return await _regulationsetupRepository.ApproveRegulationSetup(access);
        }

        public async Task<RegulationStupDetails> RejectRegulationSetup(AccessModel access)
        {
            return await _regulationsetupRepository.RejectRegulationSetup(access);
        }

        public async Task<List<MinorIndustry>> GetMinorIndustrybyMajorID(long majorIndustoryId)
        {
            return await _regulationsetupRepository.GetMinorIndustrybyMajorID(majorIndustoryId);
        }

        public async Task<List<IndustryrMapping>> GetMinorIndustrybyMajorIDMap(List<long> majorIndustoryId, long countryId)
        {
            return await _regulationsetupRepository.GetMinorIndustrybyMajorIDMap(majorIndustoryId, countryId);
        }

        public async Task<List<IndustryrMapping>> GetTOBMinorIndustrybyMajorIDMap(List<long> majorIndustoryId, List<long> minorIndustoryId, long countryId)
        {
            return await _regulationsetupRepository.GetTOBMinorIndustrybyMajorIDMap(majorIndustoryId, minorIndustoryId, countryId);
        }

        public async Task<List<IndustryMappings>> GetIndustryMapping(long countryId)
        {
            return await _regulationsetupRepository.GetIndustryMapping(countryId);
        }

        public async Task<RegSetupComplianceModel> GetRegulationSetupCompliance(Guid? complianceuid)
        {
            return await _regulationsetupRepository.GetRegulationSetupCompliance(complianceuid);
        }

        public async Task<RegSetupComplianceModel> AddRegSetupComplianceAsync(RegSetupComplianceModel compliance)
        {
            var result = await _regulationsetupRepository.AddRegSetupComplianceAsync(compliance);
            {
                if (result != null && result.ResponseCode == 1)
                {
                    AccessModel access = new AccessModel() { CreatedBy = compliance.CreatedBy, ManagerId = compliance.ManagerId == 0 ? 1 : compliance.ManagerId, Status = helperRepository.IsSuperAdmin((int)compliance.CreatedBy) ? 1 : 0, UserId = result.CreatedBy, HistoryId = result.HistoryId };
                    await _regulationsetupRepository.PostUpdateRegComplianceApproval(access);
                }
            }
            return result;
        }

        public async Task<GetRegulationBasicAndParameterByCountryID> GetAllRegulationBasicAndParameterByCountryID(int countryId)
        {
            return await _regulationsetupRepository.GetAllRegulationBasicAndParameterByCountryID(countryId);
        }

        public async Task<List<PendingApproval>> GetPendingRegulationSetupApproval(Guid? UserUID)
        {
            return await _regulationsetupRepository.GetPendingRegulationSetupApproval(UserUID);
        }

        public async Task<RegSetupComplianceModel> GetRegSetupComplianceHistory(Guid? Uid)
        {
            return await _regulationsetupRepository.GetRegSetupComplianceHistory(Uid);
        }

        public async Task<RegSetupComplianceModel> ApproveRegSetupCompliance(AccessModel access)
        {
            return await _regulationsetupRepository.ApproveRegSetupCompliance(access);
        }

        public async Task<RegSetupComplianceModel> RejectRegSetupCompliance(AccessModel access)
        {
            return await _regulationsetupRepository.RejectRegSetupCompliance(access);
        }

        public async Task<TOCRegistrationModel> GetTOCRegistration(long tocRegId, long? complianceId, long? regulationid, string ruleType)
        {
            return await _regulationsetupRepository.GetTOCRegistration(tocRegId, complianceId, regulationid, ruleType);
        }

        public async Task<TOCRegistrationModel> SaveTOCRegistrationAsync(TOCRegistrationModel TOC)
        {
            var result = await _regulationsetupRepository.SaveTOCRegistrationAsync(TOC);
            {
                if (result != null && result.ResponseCode == 1)
                {
                    AccessModel access = new AccessModel() { CreatedBy = TOC.CreatedBy, ManagerId = TOC.ManagerId == 0 ? 1 : TOC.ManagerId, Status = helperRepository.IsSuperAdmin((int)TOC.CreatedBy) ? 1 : 0, UserId = result.CreatedBy, HistoryId = result.HistoryId, TOCRuleType = TOC.RuleType, ComplianceId = TOC.ComplianceId };
                    await _regulationsetupRepository.TOCComplianceApproval(access);
                }
            }
            return result;
        }

        public async Task<TOCRules> GetTOCRulesAsync(long complianceId, long regulationid, string ruleType)
        {
            return await _regulationsetupRepository.GetTOCRulesAsync(complianceId, regulationid, ruleType);
        }

        public async Task<TOCRules> PostTOCRules(TOCRules TOC)
        {
            var result = await _regulationsetupRepository.SaveTOCRulesAsync(TOC);
            {
                if (result != null && result.ResponseCode == 1)
                {
                    AccessModel access = new AccessModel() { CreatedBy = TOC.CreatedBy, ManagerId = TOC.ManagerId == 0 ? 1 : TOC.ManagerId, Status = helperRepository.IsSuperAdmin((int)TOC.CreatedBy) ? 1 : 0, UserId = result.CreatedBy, HistoryId = result.HistoryId, TOCRuleType = TOC.RuleType, ComplianceId = TOC.ComplianceId };
                    await _regulationsetupRepository.TOCComplianceApproval(access);
                }
            }
            return result;
        }

        public async Task<object> GetTOCHistory(long historyId)
        {
            return await _regulationsetupRepository.GetTOCHistory(historyId);
        }

        public async Task<Response> ApproveTOCRegistration(AccessModel access)
        {
            return await _regulationsetupRepository.ApproveTOCRegistration(access);
        }

        public async Task<Response> RejectTOC(AccessModel access)
        {
            return await _regulationsetupRepository.RejectTOC(access);
        }

        public async Task<List<TOCRegistration>> GetTOCRegisteration() 
        {
            return await _regulationsetupRepository.GetAllTocRegister();
        }
        public async Task<Response> ApproveTOCRule(AccessModel access)
        {
            return await _regulationsetupRepository.ApproveTOCRule(access);
        }

        // In RegulationSetupService.cs
        public async Task<List<RegulationListModel>> GetRegulationsByCountryIds(string countryIds)
        {
            var ids = countryIds.Split(',').Select(id => long.Parse(id.Trim())).ToList();
            return await _regulationsetupRepository.GetRegulationsByCountryIds(ids);
        }

        public async Task<string> GetNextRegulationSetupCode()
        {
            return await _regulationsetupRepository.GetNextRegulationSetupCode();
        }

        public async Task<string> GetNextRegulationSetupComplianceCode()
        {
            return await _regulationsetupRepository.GetNextRegulationSetupComplianceCode();
        }

        public async Task<string> GetNextRegulationSetupTypeOfComplianceRegisterCode()
        {
            return await _regulationsetupRepository.GetNextRegulationSetupTypeOfComplianceRegisterCode();
        }

        public async Task<string> GetNextRegulationSetupTypeOfComplianceDuesCode()
        {
            return await _regulationsetupRepository.GetNextRegulationSetupTypeOfComplianceDuesCode();
        }

        ///<see cref="IRegulationSetupService.GetTypeOfCompliances()"/>
        public async Task<List<TypeOfCompliance>> GetTypeOfCompliances()
        {
            return await _regulationsetupRepository.GetTypeOfCompliances();
        }

        public async Task<List<TOCDues>> GetAllTOCDuesAsync()
        {
            return await _regulationsetupRepository.GetAllTOCDuesAsync();
        }
    }
}