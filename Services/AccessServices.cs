using ComplianceAPI.Models;
using ComplianceAPI.Repository;

namespace ComplianceAPI.Services
{
    public interface IAccessServices
    {
        Task<List<AccessModel>> GetAccessList(Guid? UserUID);

        Task<List<PendingApproval>> GetPendingApproval(Guid? UserUID);

        Task<bool> PostUserManagement(List<AccessModel> access);

        Task<bool> PostApproveAccess(AccessModel access);

        Task<bool> PostRejectAccess(AccessModel access);

        /// <summary>
        /// Get Menu options for role Id
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<MenuOptions>> GetMenuOptions(int roleId, int userId);

        /// <summary>
        /// Get User access for user Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<UserAccess>> GetUserAccess(int userId);

        /// <summary>
        /// Set User access for the user Id
        /// </summary>
        /// <param name="userAccess"></param>
        /// <returns></returns>
        Task<bool> SetUserAccess(List<SetAccessRequest> userAccess);

        /// <summary>
        /// Get Menu options for Parent Id
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<MenuOptions>> GetMenuOptionsForParent(int parentId, int userId);

        /// <summary>
        /// Post User Organization Mapping
        /// </summary>
        /// <param name="userOrganizations"></param>
        /// <returns></returns>
        Task<bool> PostUserOrganizationMapping(List<UsersOrganizations> userOrganizations);

        Task<UserRegulationMappingDetails> GetUserRegulationMappingDetailsByUserIdAsync(long userId);

        Task<bool> AddUserRegulationMappingAsync(PostUserRegulationMapping mapping);
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

        ///<see cref="IAccessServices.GetMenuOptions(int, int)"/>
        public async Task<List<MenuOptions>> GetMenuOptions(int roleId, int userId)
        {
            return await _accessRepository.GetMenuOptions(roleId, userId);
        }

        ///<see cref="IAccessServices.GetUserAccess(int)"/>
        public async Task<List<UserAccess>> GetUserAccess(int userId)
        {
            return await _accessRepository.GetUserAccess(userId);
        }

        ///<see cref="IAccessServices.SetUserAccess(List{SetAccessRequest})"/>
        public async Task<bool> SetUserAccess(List<SetAccessRequest> userAccess)
        {
            return await _accessRepository.SetUserAccess(userAccess);
        }

        ///<see cref="IAccessServices.GetMenuOptionsForParent(int, int)"/>
        public async Task<List<MenuOptions>> GetMenuOptionsForParent(int parentId, int userId)
        {
            var res = await _accessRepository.GetMenuOptionsForParent(parentId, userId);
            if (res != null && res.Any())
            {
                foreach (var item in res)
                {
                    item.Title += " Screen";
                }
            }

            return res;
        }

        ///<see cref="IAccessServices.PostUserOrganizationMapping(List<UsersOrganizations>)"/>
        public async Task<bool> PostUserOrganizationMapping(List<UsersOrganizations> userOrganizations)
        {
            return await _accessRepository.PostUserOrganizationMapping(userOrganizations);
        }

        public async Task<UserRegulationMappingDetails> GetUserRegulationMappingDetailsByUserIdAsync(long userId)
        {
            return await _accessRepository.GetUserRegulationMappingDetailsByUserIdAsync(userId);
        }

        public Task<bool> AddUserRegulationMappingAsync(PostUserRegulationMapping mapping)
        {
            return _accessRepository.AddUserRegulationMappingAsync(mapping);
        }
    }
}