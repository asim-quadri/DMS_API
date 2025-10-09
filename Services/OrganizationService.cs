using Dms_Api.Models;
using Dms_Api.Repository;

namespace Dms_Api.Services
{
    // Services/IOrganisationService.cs
    public interface IOrganizationService
    {
        Task<Organization> AddOrganization(Organization organisation);
        Task<List<Organization>> GetAllOrganizations();
        Task<bool> DeleteOrganization(int id);
    }

    // Services/OrganisationService.cs
    public class OrganisationService : IOrganizationService
    {
        private readonly IOrganizationRepository _organisationRepository;

        public OrganisationService(IOrganizationRepository organizationRepository)
        {
            _organisationRepository = organizationRepository;
        }

        public async Task<Organization> AddOrganization(Organization organisation)
        {
            return await _organisationRepository.AddOrganization(organisation);
        }

       
        public async Task<List<Organization>> GetAllOrganizations()
        {
            return await _organisationRepository.GetAllOrganizations();
        }

        public async Task<bool> DeleteOrganisation(int id)
        {
            return await _organisationRepository.DeleteOrganization(id);
        }

        public Task<bool> DeleteOrganization(int id)
        {
            throw new NotImplementedException();
        }
    }

}
