using ComplianceAPI.Helpers;
using ComplianceAPI.Models;
using ComplianceAPI.Repository;

namespace ComplianceAPI.Services
{
    public interface IRegulationGroupService
    {
        Task<List<RegulationGroupModel>> GetAllRegulationGroups();

        Task<RegulationGroupModel> GetRegulationGroupByUID(Guid uid);

        Task<RegulationGroupModel> PostRegulationGroup(RegulationGroupModel country);

        Task<bool> PostRegulationGroupApprove(AccessModel access);

        Task<bool> PostRegulationGroupForward(AccessModel access);

        Task<bool> PostRegulationGroupReject(AccessModel access);

        Task<List<RegulationGroupModel>> GetCountryRegulationGroupMapping();

        Task<RegulationGroupModel> PostCountryRegulationGroupMapping(RegulationGroupModel country);

        Task<List<RegulationGroupModel>> GetRegulationGroupApprovalList(Guid UserUID);

        Task<List<RegulationGroupModel>> GetAllRegulationGroupApprovalList();

        Task<List<RegulationGroupModel>> GetAllCountryRegulationGroupMappingApproval();

        Task<List<RegulationGroupModel>> GetCountryRegulationGroupMappingApproval(Guid UserUID);

        Task<bool> PostCountryRegulationGroupMappingApprove(AccessModel access);

        Task<bool> PostCountryRegulationGroupMappingReject(AccessModel access);

        Task<bool> PostCountryRegulationGroupMappingForward(AccessModel access);
        Task<string> GetNextRegulationGroupCode();

    }

    public class RegulationGroupService : IRegulationGroupService
    {
        private readonly IRegulationGroupRepository _countryRepository;

        public RegulationGroupService(IRegulationGroupRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public async Task<List<RegulationGroupModel>> GetAllRegulationGroups()
        {
            return await _countryRepository.GetAllRegulationGroups();
        }

        public async Task<RegulationGroupModel> GetRegulationGroupByUID(Guid uid)
        {
            return await _countryRepository.GetRegulationGroupByUID(uid);
        }

        public async Task<RegulationGroupModel> PostRegulationGroup(RegulationGroupModel regulation)
        {
            var result = await _countryRepository.PostRegulationGroup(regulation);
            if (result != null && result.ResponseCode != 0)
            {
                AccessModel access = new AccessModel() { CreatedBy = regulation.CreatedBy, ManagerId = regulation.ManagerId == 0 ? 1 : regulation.ManagerId, Status = 0, RegulationGroupId = result.Id , ReferenceCode = regulation.RegulationGroupReferenceCode };
                await _countryRepository.PostRegulationGroupApprove(access, RefApprovalStatusU.Pending);
            }
            await _countryRepository.AddNewRegulationGroupApprovalNotification(regulation.CreatedBy.Value, regulation.RegulationGroupName);
            return result;
        }

        public async Task<bool> PostRegulationGroupApprove(AccessModel access)
        {
            return await _countryRepository.PostRegulationGroupApprove(access, RefApprovalStatusU.Approved);
        }

        public async Task<bool> PostRegulationGroupForward(AccessModel access)
        {
            return await _countryRepository.PostRegulationGroupApprove(access, RefApprovalStatusU.Forward);
        }

        public async Task<bool> PostRegulationGroupReject(AccessModel access)
        {
            return await _countryRepository.PostRegulationGroupApprove(access, RefApprovalStatusU.Rejected);
        }

        public async Task<List<RegulationGroupModel>> GetCountryRegulationGroupMapping()
        {
            return await _countryRepository.GetCountryRegulationGroupMapping();
        }

        public async Task<RegulationGroupModel> PostCountryRegulationGroupMapping(RegulationGroupModel regulation)
        {
            var result = await _countryRepository.PostCountryRegulationGroupMapping(regulation);
            if (result != null && result.ResponseCode != 0)
            {
                AccessModel access = new AccessModel() { CreatedBy = regulation.CreatedBy, ManagerId = regulation.ManagerId, Status = 0, CountryRegulationGroupMappingId = result.Id };
                await _countryRepository.PostCountryRegulationGroupMappingApprove(access, RefApprovalStatusU.Pending);
            }
            await _countryRepository.AddNewRegulationGroupMappingApprovalNotification(regulation.CreatedBy.Value, regulation.RegulationGroupId);
            return result;
        }

        public async Task<bool> PostCountryRegulationGroupMappingApprove(AccessModel access)
        {
            return await _countryRepository.PostCountryRegulationGroupMappingApprove(access, RefApprovalStatusU.Approved);
        }

        public async Task<bool> PostCountryRegulationGroupMappingReject(AccessModel access)
        {
            return await _countryRepository.PostCountryRegulationGroupMappingApprove(access, RefApprovalStatusU.Rejected);
        }

        public async Task<bool> PostCountryRegulationGroupMappingForward(AccessModel access)
        {
            return await _countryRepository.PostCountryRegulationGroupMappingApprove(access, RefApprovalStatusU.Forward
                );
        }

        public async Task<List<RegulationGroupModel>> GetRegulationGroupApprovalList(Guid UserUID)
        {
            var regulation = await _countryRepository.GetRegulationGroupApprovalList(UserUID);
            return regulation.ToList();
        }

        public async Task<List<RegulationGroupModel>> GetAllRegulationGroupApprovalList()
        {
            var regulation = await _countryRepository.GetAllRegulationGroupApprovalList();
            return regulation.ToList();
        }

        public async Task<List<RegulationGroupModel>> GetAllCountryRegulationGroupMappingApproval()
        {
            return await _countryRepository.GetAllCountryRegulationGroupMappingApproval();
        }

        public async Task<List<RegulationGroupModel>> GetCountryRegulationGroupMappingApproval(Guid UserUID)
        {
            return await _countryRepository.GetCountryRegulationGroupMappingApproval(UserUID);
        }
        public async Task<string> GetNextRegulationGroupCode()
        {
            return await _countryRepository.GetNextRegulationGroupCode();
        }

    }
}