using ComplianceAPI.Helpers;
using ComplianceAPI.Models;
using ComplianceAPI.Repository;
using RefApprovalType = ComplianceAPI.Helpers.RefApprovalType;
using User = ComplianceAPI.Models.User;

namespace ComplianceAPI.Services
{
    public interface IUserServices
    {
        Task<User> login(Login login);
        Task<Roles> Updateroles(UpdateRoles roles);
        Task<User> PostUser(PostUser user, Guid? accessUID);
        Task<List<User>> GetAllUsers();
        Task<User> GetUsers(Guid uid);
        Task<User> GetHistoryUsers(long HistoryId);
        Task<User> DeleteUsers(Guid uid, int status);
        Task<User> ForgotPassword(ForgotPassword forgotPassword);
        Task<List<Roles>> GetAllRoles();
        Task<Roles> GetRoles(Guid uid);

        Task<Roles> GetHistoryRoles(long HistoryId);
        Task<List<PendingApproval>> GetPendingRoleApproval(Guid? UserUID);
        Task<bool> UpdateRoleApproveAccess(AccessModel access);
        Task<bool> UpdateRoleRejectAccess(AccessModel access);
        Task<List<Products>> GetAllProducts();

        Task<List<PendingApproval>> GetPendingUserApproval(Guid? UserUID);

        Task<bool> PostUserApproveAccess(AccessModel access);
        Task<bool> PostUserRejectAccess(AccessModel access);

        Task<bool> PostUserReviewAccess(AccessModel access);
        Task<bool> PostUserForwardAccess(AccessModel access);
        // Task<bool> PostProductAccess(Products product);

    }
    public class UserServices : IUserServices
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IConfiguration configuration;
        private readonly IEmail email;

        public UserServices(IUsersRepository usersRepository, IConfiguration configuration, IEmail email)
        {
            _usersRepository = usersRepository;
            this.configuration = configuration;
            this.email = email;
        }

        public async Task<User> login(Login login)
        {
            return await _usersRepository.login(login);
        }
        public async Task<Roles> Updateroles(UpdateRoles roles)
        {
            if (_usersRepository.IsSuperAdmin((int)roles.CreatedBy))
            {
                await _usersRepository.updateRoleAdmin(roles);
            }
            var result = await _usersRepository.updateRole(roles);
            AccessModel access = new AccessModel() { ApprovalTypeId = RefApprovalType.Role, CreatedBy = roles.CreatedBy, ManagerId = roles.ManagerId == 0 ? 1 : roles.ManagerId, Status = 0, UserId = result.CreatedBy, HistoryId = result.Id };
            await _usersRepository.PostUpdateRoleApproval(access);
            await _usersRepository.AddUserRoleUpdationApprovalNotification(roles.CreatedBy.Value, roles.RoleDisplayName);
            return result;
        }

        public async Task<User> PostUser(PostUser user, Guid? accessUID)
        {
            User response = null;
            if (_usersRepository.IsSuperAdmin((int)user.CreatedBy))
            {
                response = new User();
                response = await _usersRepository.AdminPostUser(user);
                email.Sendemail(response.Email, response.FullName, response.Email, response.Password);
                response.Password = "";
                if (response.ResponseCode == 1)
                {
                    var result = await _usersRepository.PostUser(user);
                    if (accessUID == null)
                    {
                        AccessModel access = new AccessModel() { ApprovalTypeId = RefApprovalType.User, CreatedBy = user.CreatedBy, ManagerId = user.ApprovalManagerId == 0 ? 1 : user.ApprovalManagerId, Status = 0, UserId = result.Id, HistoryId = result.HistoryId };
                        await _usersRepository.PostUpdateUserApproval(access);
                    }
                }
                await _usersRepository.AddUserApprovalNotification(response.CreatedBy.Value, response.FullName ?? string.Empty, true);
                return response;
            }
            else
            {
                var result = await _usersRepository.PostUser(user);
                if (accessUID == null)
                {
                    AccessModel access = new AccessModel() { ApprovalTypeId = RefApprovalType.User, CreatedBy = user.CreatedBy, ManagerId = user.ApprovalManagerId == 0 ? 1 : user.ApprovalManagerId, Status = 0, UserId = result.Id, HistoryId = result.HistoryId };
                    await _usersRepository.PostUpdateUserApproval(access);
                }
                await _usersRepository.AddUserApprovalNotification(result.CreatedBy.Value, result.FullName ?? string.Empty, false);
                return result;
            }
        }

        public async Task<List<User>> GetAllUsers()
        {
            // new Email(configuration).Sendemail("gulam.asif2911@gmail.com", "Asif", "gulam.asif2911@gmail.com", "123");
            return await _usersRepository.GetAllUsers();
        }
        public async Task<User> GetUsers(Guid uid)
        {
            return await _usersRepository.GetUsers(uid);
        }

        public async Task<User> GetHistoryUsers(long HistoryId)
        {
            return await _usersRepository.GetHistoryUsers(HistoryId);
        }

        public async Task<Roles> GetHistoryRoles(long HistoryId)
        {
            return await _usersRepository.GetHistoryRoles(HistoryId);
        }

        public async Task<User> DeleteUsers(Guid uid, int status)
        {
            return await _usersRepository.DeleteUsers(uid, status);
        }

        public async Task<List<Roles>> GetAllRoles()
        {
            return await _usersRepository.GetAllRoles();
        }
        public async Task<Roles> GetRoles(Guid uid)
        {
            return await _usersRepository.GetRoles(uid);
        }
        public async Task<List<PendingApproval>> GetPendingRoleApproval(Guid? UserUID)
        {
            return await _usersRepository.GetPendingRolesApproval(UserUID);
        }
        public async Task<bool> UpdateRoleApproveAccess(AccessModel access)
        {
            return await _usersRepository.UpdateRoleApproveAccess(access);
        }

        public async Task<bool> UpdateRoleRejectAccess(AccessModel access)
        {
            return await _usersRepository.UpdateRoleRejectAccess(access);
        }
        public async Task<List<Products>> GetAllProducts()
        {
            return await _usersRepository.GetAllProducts();
        }

        //public async Task<bool> PostProductAccess(Products products)
        //{
        //    return await _usersRepository.PostProductAccess(products);
        //}
        public async Task<User> ForgotPassword(ForgotPassword forgotPassword)
        {
            return await _usersRepository.ForgotPassword(forgotPassword);
        }
        public async Task<List<PendingApproval>> GetPendingUserApproval(Guid? UserUID)
        {
            return await _usersRepository.GetPendingUserApproval(UserUID);
        }

        public async Task<bool> PostUserApproveAccess(AccessModel access)
        {
            return await _usersRepository.PostUserApproveAccess(access);
        }

        public async Task<bool> PostUserRejectAccess(AccessModel access)
        {
            return await _usersRepository.PostUserRejectAccess(access);
        }

        public async Task<bool> PostUserReviewAccess(AccessModel access)
        {
            return await _usersRepository.PostUserReviewAccess(access);
        }

        public async Task<bool> PostUserForwardAccess(AccessModel access)
        {
            return await _usersRepository.PostUserForwardAccess(access);
        }
    }
}
