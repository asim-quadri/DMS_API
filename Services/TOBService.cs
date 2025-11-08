using ComplianceAPI.Helpers;
using ComplianceAPI.Models;
using ComplianceAPI.Models.DataModels;
using ComplianceAPI.Repository;
using RefApprovalType = ComplianceAPI.Helpers.RefApprovalType;

namespace ComplianceAPI.Services
{
    public interface ITOBService
    {
        Task<TOB> GetTOBHistory(long HistoryId);
        Task<List<TOBDetails>> GetTOBList();
        Task<List<TOBMapping>> GetTOBMinorIndusryMapping(int tobId);
        Task<List<PendingApproval>> GetPendingTOBApproval(Guid? UserUID);
        Task<List<TOBApprovalList>> GetTOBMappingApprovaList(Guid UserUID);
        Task<List<TOBMappingList>> GetTOBMappingList();
        Task<List<TOBMappingList>> GetTOBMappingByCountry(long? countryId);
        Task<List<TOBMappingList>> GetTOBMappingByMajor(long? majorInudustryId, long? countryId);
        Task<List<TOBMappingList>> GetTOBMappingByMinor(long? minorInudustryId, long? majorInudustryId, int? countryId);
        Task<TOB> GetHistoryTOB(Guid? UID);
        Task<TOBMappingList> PostTOBMapping(TOBMappingList tobMapping);
        //Task<List<TOBApprovalList>> GetTOBApprovalList(Guid UserUID);
        Task<TOB> AddTOBDetails(TOB tobDetails);
        Task<bool> PostTOBMappingApprove(AccessModel access);
        Task<bool> PostTOBMappingReject(AccessModel access);
        Task<TOB> ApproveTOB(AccessModel access);
        Task<TOB> RejectTOB(AccessModel access);
        Task<string> GetNextTOBCode();
    }
    public class TOBService: ITOBService
    {
        private readonly ITOBRepository _tobRepository;
        private readonly IHelperRepository helperRepository;
        public TOBService(ITOBRepository tobRepository, IHelperRepository helperRepository)
        {
            _tobRepository = tobRepository;
            this.helperRepository = helperRepository;
        }
        public async Task<TOB> GetTOBHistory(long HistoryId)
        {
            return await _tobRepository.GetTOBHistory(HistoryId);
        }
        public async Task<List<TOBDetails>> GetTOBList()
        {
            return await _tobRepository.GetAllTOB();
        }
        public async Task<List<TOBMapping>> GetTOBMinorIndusryMapping(int tobId)
        {
            return await _tobRepository.GetTOBMinorIndusryMapping(tobId);
        }
        public async Task<TOB> GetHistoryTOB(Guid? UID)
        {
            return await _tobRepository.GetHistoryTOB(UID);
        }
        public async Task<List<TOBMappingList>> GetTOBMappingList()
        {
            return await _tobRepository.GetTOBMappingList();
        }
        public async Task<List<TOBMappingList>> GetTOBMappingByCountry(long? countryId)
        {
            return await _tobRepository.GetTOBMappingByCountry(countryId);
        }
        public async Task<List<TOBMappingList>> GetTOBMappingByMajor(long? majorInudustryId, long? countryId)
        {
            return await _tobRepository.GetTOBMappingByMajor(majorInudustryId, countryId);
        }
        public async Task<List<TOBMappingList>> GetTOBMappingByMinor(long? minorInudustryId, long? majorInudustryId, int? countryId)
        {
            return await _tobRepository.GetTOBMappingByMinor(minorInudustryId, majorInudustryId, countryId);
        }
        public async Task<List<TOBApprovalList>> GetTOBMappingApprovaList(Guid UserUID)
        {
            return await _tobRepository.GetTOBMappingApprovaList(UserUID);
        }
        public async Task<List<PendingApproval>> GetPendingTOBApproval(Guid? UserUID)
        {
            return await _tobRepository.GetPendingTOBApproval(UserUID);
        }
        //public async Task<List<TOBApprovalList>> GetTOBApprovalList(Guid UserUID)
        //{
        //    var tob = await _tobRepository.GetMajorApprovaList(UserUID);
        //    return tob.Concat(tob).ToList();
        //}
        public async Task<TOBMappingList> PostTOBMapping(TOBMappingList tobMapping)
        {
            var result = await _tobRepository.PostTOBMapping(tobMapping);
            if (result.ResponseCode != 0)
            {
                AccessModel access = new AccessModel() { CreatedBy = tobMapping.CreatedBy, ManagerId = tobMapping.ManagerId, Status = 0, TOBMappingId = result.Id };
                await _tobRepository.PostTOBApprovalMapping(access, RefApprovalStatusU.Pending);
            }
            await _tobRepository.AddTOBMappingApprovalNotification(tobMapping.CreatedBy.Value, tobMapping.TOBId);
            return result;
        }
        public async Task<TOB> AddTOBDetails(TOB tobDetails)
        {
            var result = await _tobRepository.AddTOBDetails(tobDetails);
            AccessModel access = new AccessModel() { ApprovalTypeId = RefApprovalType.User, CreatedBy = tobDetails.CreatedBy, ManagerId = tobDetails.ApprovalManagerId == null ? 0 : tobDetails.ApprovalManagerId, Status = 0, UserId = result.Id, HistoryId = result.HistoryId };
            await _tobRepository.PostUpdateTOBApproval(access);
            await _tobRepository.AddTOBApprovalNotification(tobDetails.CreatedBy.Value, tobDetails.TOBName);
            return result;
        }
        public async Task<bool> PostTOBApproveAccess(AccessModel access)
        {
            return await _tobRepository.PostTOBApproveAccess(access);
        }
        public async Task<bool> PostTOBMappingApprove(AccessModel access)
        {
            return await _tobRepository.PostTOBApprovalMapping(access, RefApprovalStatusU.Approved);
        }
        public async Task<bool> PostTOBMappingReject(AccessModel access)
        {
            return await _tobRepository.PostTOBApprovalMapping(access, RefApprovalStatusU.Rejected);
        }
        public async Task<TOB> ApproveTOB(AccessModel access)
        {
            return await _tobRepository.ApproveTOB(access);
        }

        public async Task<TOB> RejectTOB(AccessModel access)
        {
            return await _tobRepository.RejectTOB(access);
        }

        public async Task<string> GetNextTOBCode()
        {
            return await _tobRepository.GetNextTOBCode();
        }

    }
}
