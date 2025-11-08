using ComplianceAPI.Helpers;
using ComplianceAPI.Models;
using ComplianceAPI.Repository;

namespace ComplianceAPI.Services
{
    public interface IEntityService
    {
        Task<List<EntityApprovalList>> GetEntityApprovalList(int userId);
        Task<List<Entity>> GetAllEntitiesByOrgId(int orgId);
        Task<Entity> GetEntityDetails(int entityId);
        Task<PostEntity> PostEntity(PostEntity enity);

        Task<Models.Entity> GetEntityView(int entityId);
        Task<bool> PostEntityApprove(AccessModel access);
        Task<bool> PostEntityReject(AccessModel access);
        Task<bool> PostEntityForward(AccessModel access);
        Task<FinancialMonth> GetStartAndEndMonthsByCountryId(int countryId);
        Task<List<Entity>> GetEntitiesByOrganizationId(long organizationId);
        Task<List<Entity>> GetEntitiesByOrganizationAndCountryId(long organizationId,long countryId);
        Task<List<ComplianceAPI.Models.Entity>> GetClientEntitiesLocations(int organizationId);
    }
    public class EntityService : IEntityService
    {
        private readonly IEntityRepository _entityRepository;
        public EntityService(IEntityRepository entityRepository)
        {
            _entityRepository = entityRepository;
        }

        public async Task<List<EntityApprovalList>> GetEntityApprovalList(int userId)
        {
            return await _entityRepository.GetEntityApprovalList(userId);
        }
        public async Task<List<Entity>> GetAllEntitiesByOrgId(int orgId)
        {
            return await _entityRepository.GetAllEntitiesByOrgId(orgId);
        }

        public async Task<Entity> GetEntityDetails(int entityId)
        {
            return await _entityRepository.GetEntityDetails(entityId);
        }

        public async Task<Models.Entity> GetEntityView(int entityId)
        {
            return await _entityRepository.GetEntityView(entityId);
        }

        public async Task<PostEntity> PostEntity(PostEntity entity)
        {
            PostEntity result = new PostEntity();
            result = await _entityRepository.PostEntity(entity);
            return result;
        }

        public async Task<bool> PostEntityApprove(AccessModel access)
        {
            return await _entityRepository.UpdateEntityApproval(access, RefApprovalStatusU.Approved);
        }

        public async Task<bool> PostEntityReject(AccessModel access)
        {
            return await _entityRepository.UpdateEntityApproval(access, RefApprovalStatusU.Rejected);
        }

        public async Task<bool> PostEntityForward(AccessModel access)
        {
            return await _entityRepository.UpdateEntityApproval(access, RefApprovalStatusU.Forward);
        }

        public async Task<FinancialMonth> GetStartAndEndMonthsByCountryId(int countryId)
        {
            return await _entityRepository.GetStartAndEndMonthsByCountryId(countryId);
        }

        public async Task<List<Entity>> GetEntitiesByOrganizationId(long organizationId)
        {
            var entities = await _entityRepository.GetEntitiesByOrganizationId(organizationId);
            return entities;
        }
        public async Task<List<Entity>> GetEntitiesByOrganizationAndCountryId(long organizationId,long countryId)
        {
            var entities = await _entityRepository.GetEntitiesByOrganizationAndCountryId(organizationId, countryId);
            return entities;
        }

        public async Task<List<ComplianceAPI.Models.Entity>> GetClientEntitiesLocations(int organizationid)
        {
            return await _entityRepository.GetClientEntitiesLocations(organizationid);
        }
    }
}
