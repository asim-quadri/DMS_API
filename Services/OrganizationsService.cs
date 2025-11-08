using ComplianceAPI.Helpers;
using ComplianceAPI.Models;
using ComplianceAPI.Repository;

namespace ComplianceAPI.Services
{
    public interface IOrganizationServices
    {
        Task<List<BillingLevel>> GetAllBillingLevel();
        Task<List<ProductType>> GetAllProductType();
        Task<List<OrganizationDetail>> GetAllOrganization(int? user);
        Task<List<OrganizationDetail>> GetAllOrganizations();
        Task<OrganizationDetail> GetOrganizationById(long Id);
        Task<List<OrganizationDetail>> GetAllOrganizationWithoutStatus(int? user);
        Task<BillingLevel> GetBillingLevelById(int id);
        Task<List<OrganizationApprovalList>> GetOrganizationApprovalList(string userUID);
        Task<PostOrganization> PostOrganization(PostOrganization organization);
        Task<bool> PostOrganizationApprove(AccessModel access);
        Task<bool> PostOrganizationReject(AccessModel access);
        Task<bool> PostOrganizationForward(AccessModel access);
        Task<List<OrganizationEntityList>> GetOrgEntityList(long? userId);
        Task<List<OrganizationEntityList>> GetOrgEntityLists();
        Task<List<OrganizationDetail>> GetUserOrganizationsByUserId(long userId);
        Task<List<OrganizationDetail>> GetOrganizationsByCountryId(long countryId);
        Task<List<OrganizationDetail>> GetOrganizationsByUserandCountry(long userId, long countryId);


    }

    public class OrganizationService : IOrganizationServices
    {
        private readonly IOrganizationRepository _organizationRepository;
        public OrganizationService(IOrganizationRepository organizationRepository)
        {
            _organizationRepository = organizationRepository;
        }

        public async Task<List<BillingLevel>> GetAllBillingLevel()
        {
            return await _organizationRepository.GetAllBillingLevel();
        }

        public async Task<List<ProductType>> GetAllProductType()
        {
            return await _organizationRepository.GetAllProductType();
        }

        public async Task<List<OrganizationDetail>> GetAllOrganizations()
        {
            return await _organizationRepository.GetAllOrganizations();
        }

        public async Task<List<OrganizationDetail>> GetAllOrganization(int? userId)
        {
            return await _organizationRepository.GetAllOrganization(userId);
        }

        public async Task<OrganizationDetail> GetOrganizationById(long Id)
        {
            return await _organizationRepository.GetOrganizationById(Id);
        }

        public async Task<List<OrganizationDetail>> GetAllOrganizationWithoutStatus(int? user)
        {
            return await _organizationRepository.GetAllOrganizationWithoutStatus(user);
        }

        public async Task<BillingLevel> GetBillingLevelById(int id)
        {
            return await _organizationRepository.GetBillingLevelById(id);
        }

        public async Task<List<OrganizationApprovalList>> GetOrganizationApprovalList(string userUID)
        {
            return await _organizationRepository.GetOrganizationApprovalList(userUID);
        }

        public async Task<PostOrganization> PostOrganization(PostOrganization organization)
        {
            PostOrganization result = new PostOrganization();
            result = await _organizationRepository.PostOrganization(organization);
            return result;
        }

        public async Task<bool> PostOrganizationApprove(AccessModel access)
        {
            return await _organizationRepository.PostUpdateOrganizationApproval(access, RefApprovalStatusU.Approved);
        }

        public async Task<bool> PostOrganizationReject(AccessModel access)
        {
            return await _organizationRepository.PostUpdateOrganizationApproval(access, RefApprovalStatusU.Rejected);
        }

        public async Task<bool> PostOrganizationForward(AccessModel access)
        {
            return await _organizationRepository.PostUpdateOrganizationApproval(access, RefApprovalStatusU.Forward);
        }

        public async Task<List<OrganizationEntityList>> GetOrgEntityLists()
        {
            return await _organizationRepository.GetOrgEntityLists();
        }

        public async Task<List<OrganizationEntityList>> GetOrgEntityList(long? userId)
        {
            return await _organizationRepository.GetOrgEntityList(userId);
        }

        public async Task<List<OrganizationDetail>> GetUserOrganizationsByUserId(long userId)
        {
            return await _organizationRepository.GetUserOrganizationsByUserId(userId);
        }

        public async Task<List<OrganizationDetail>> GetOrganizationsByCountryId(long countryId)
        {
            return await _organizationRepository.GetOrganizationsByCountryId(countryId);
        }
        public async Task<List<OrganizationDetail>> GetOrganizationsByUserandCountry(long userId, long countryId)
        {
            return await _organizationRepository.GetOrganizationsByUserandCountry(userId, countryId);
        }
    }
}
