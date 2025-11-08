using ComplianceAPI.Helpers;
using ComplianceAPI.Models;
using ComplianceAPI.Repository;

namespace ComplianceAPI.Services
{
    public interface IEntityTypeService
    {
        Task<List<EntityTypeModel>> GetAllEntityTypes();

        Task<EntityTypeModel> GetEntityTypeByUID(Guid uid);

        Task<EntityTypeModel> PostEntityType(EntityTypeModel country);

        Task<bool> PostEntityTypeApprove(AccessModel access);

        Task<bool> PostEntityTypeForward(AccessModel access);

        Task<bool> PostEntityTypeReject(AccessModel access);

        Task<List<EntityTypeModel>> GetCountryEntityTypeMapping();

        Task<EntityTypeModel> PostCountryEntityTypeMapping(EntityTypeModel country);

        Task<List<EntityTypeModel>> GetEntityTypeApprovalList(Guid UserUID);

        Task<List<EntityTypeModel>> GetAllEntityTypeApprovalList();

        Task<List<EntityTypeModel>> GetAllCountryEntityTypeMappingApproval();

        Task<List<EntityTypeModel>> GetCountryEntityTypeMappingApproval(Guid UserUID);

        Task<bool> PostCountryEntityTypeMappingApprove(AccessModel access);

        Task<bool> PostCountryEntityTypeMappingReject(AccessModel access);

        Task<bool> PostCountryEntityTypeMappingForward(AccessModel access);
        Task<string> GetNextEntityTypeReferenceCode();

    }

    public class EntityTypeService : IEntityTypeService
    {
        private readonly IEntityTypeRepository _countryRepository;

        public EntityTypeService(IEntityTypeRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public async Task<List<EntityTypeModel>> GetAllEntityTypes()
        {
            return await _countryRepository.GetAllEntityTypes();
        }

        public async Task<EntityTypeModel> GetEntityTypeByUID(Guid uid)
        {
            return await _countryRepository.GetEntityTypeByUID(uid);
        }

        public async Task<EntityTypeModel> PostEntityType(EntityTypeModel regulation)
        {
            var result = await _countryRepository.PostEntityType(regulation);
            AccessModel access = new AccessModel() { CreatedBy = regulation.CreatedBy, ManagerId = regulation.ManagerId == 0 ? 1 : regulation.ManagerId, Status = 0, EntityTypeId = result.Id, ReferenceCode = regulation.EntityTypeReferenceCode };
            await _countryRepository.PostEntityTypeApprove(access, RefApprovalStatusU.Pending);
            await _countryRepository.AddEntityTypeApprovalNotification(regulation.CreatedBy.Value, regulation.EntityType);
            return result;
        }

        public async Task<bool> PostEntityTypeApprove(AccessModel access)
        {
            var result = await _countryRepository.PostEntityTypeApprove(access, RefApprovalStatusU.Approved);

            return result;
        }

        public async Task<bool> PostEntityTypeForward(AccessModel access)
        {
            var result = await _countryRepository.PostEntityTypeApprove(access, RefApprovalStatusU.Forward);

            return result;
        }

        public async Task<bool> PostEntityTypeReject(AccessModel access)
        {
            var result = await _countryRepository.PostEntityTypeApprove(access, RefApprovalStatusU.Rejected);
            return result;
        }

        public async Task<List<EntityTypeModel>> GetCountryEntityTypeMapping()
        {
            return await _countryRepository.GetCountryEntityTypeMapping();
        }

        public async Task<EntityTypeModel> PostCountryEntityTypeMapping(EntityTypeModel regulation)
        {
            var result = await _countryRepository.PostCountryEntityTypeMapping(regulation);
            if (result.ResponseCode != 0)
            {
                AccessModel access = new AccessModel() { CreatedBy = regulation.CreatedBy, ManagerId = regulation.ManagerId == 0 ? 1 : regulation.ManagerId, Status = 0, CountryEntityTypeMappingId = result.Id };
                await _countryRepository.PostCountryEntityTypeMappingApprove(access, RefApprovalStatusU.Pending);
            }
            await _countryRepository.AddEntityTypeMappingApprovalNotification(regulation.CreatedBy.Value, regulation.EntityTypeId);
            return result;
        }

        public async Task<bool> PostCountryEntityTypeMappingApprove(AccessModel access)
        {
            return await _countryRepository.PostCountryEntityTypeMappingApprove(access, RefApprovalStatusU.Approved);
        }

        public async Task<bool> PostCountryEntityTypeMappingReject(AccessModel access)
        {
            return await _countryRepository.PostCountryEntityTypeMappingApprove(access, RefApprovalStatusU.Rejected);
        }

        public async Task<bool> PostCountryEntityTypeMappingForward(AccessModel access)
        {
            return await _countryRepository.PostCountryEntityTypeMappingApprove(access, RefApprovalStatusU.Forward
                );
        }

        public async Task<List<EntityTypeModel>> GetEntityTypeApprovalList(Guid UserUID)
        {
            var regulation = await _countryRepository.GetEntityTypeApprovalList(UserUID);
            return regulation.ToList();
        }

        public async Task<List<EntityTypeModel>> GetAllEntityTypeApprovalList()
        {
            var regulation = await _countryRepository.GetAllEntityTypeApprovalList();
            return regulation.ToList();
        }

        public async Task<List<EntityTypeModel>> GetAllCountryEntityTypeMappingApproval()
        {
            return await _countryRepository.GetAllCountryEntityTypeMappingApproval();
        }

        public async Task<List<EntityTypeModel>> GetCountryEntityTypeMappingApproval(Guid UserUID)
        {
            return await _countryRepository.GetCountryEntityTypeMappingApproval(UserUID);
        }
        public async Task<string> GetNextEntityTypeReferenceCode()
        {
            return await _countryRepository.GetNextEntityTypeReferenceCode();
        }

        //public async Task<bool> AddEntityTypeApprovalSuccessNotification(long createdBy, string approvalStatus)
        //{
        //    return await _countryRepository.AddEntityTypeApprovalSuccessNotification(createdBy, approvalStatus);
        //}
        //public async Task<bool> AddEntityTypeMappingApprovalSuccessNotification(long createdBy, string approvalStatus)
        //{
        //    return await _countryRepository.AddEntityTypeMappingApprovalSuccessNotification(createdBy, approvalStatus);
        //}
    }
}