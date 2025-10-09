using DmsApi.Models;
using DmsApi.Repository;

namespace DmsApi.Services
{
    public interface IAccessServices
    { 
        Task<List<AccessModel>> GetAccessList(Guid? UserUID);
        Task<List<PendingApproval>> GetPendingApproval(Guid? UserUID); 
        Task<bool> PostUserManagement(List<AccessModel> access);
        Task<bool> PostApproveAccess(AccessModel access);
        Task<bool> PostRejectAccess(AccessModel access);

    } 
    public class AccessServices : IAccessServices
    {
        private readonly IAccessRepository _accessRepository;
        public AccessServices(IAccessRepository accessRepository)
        {
            _accessRepository = accessRepository;
        }

        public async Task<List<AccessModel>> GetAccessList(Guid? UserUID)
        {
            return await _accessRepository.GetAccessList(UserUID);
        }
        public async Task<List<PendingApproval>> GetPendingApproval(Guid? UserUID)
        {
            return await _accessRepository.GetPendingApproval(UserUID);
        }

        public async Task<bool> PostUserManagement(List<AccessModel> access)
        {
            return await _accessRepository.PostUserManagement(access);
        }

        public async Task<bool> PostApproveAccess(AccessModel access)
        {
            return await _accessRepository.PostApproveAccess(access);
        }

        public async Task<bool> PostRejectAccess(AccessModel access)
        {
            return await _accessRepository.PostRejectAccess(access);
        }

        
    }
}
