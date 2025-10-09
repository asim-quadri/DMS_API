using DmsApi.Helpers;
using DmsApi.Models;
using DmsApi.Repository;
using Microsoft.AspNetCore.Mvc;

namespace DmsApi.Services
{
    public interface IUserServices
    {
        Task<User> login(Login login);
        Task<Roles> Updateroles(UpdateRoles roles);
        Task<User> PostUser(PostUser user);
        Task<List<User>> GetAllUsers();
        Task<User> GetUsers(Guid uid);
        Task<User> DeleteUsers(Guid uid, int status);
        Task<User> ForgotPassword(ForgotPassword forgotPassword);
        Task<List<Roles>> GetAllRoles();
        Task<Roles> GetRoles(Guid uid);
        Task<List<PendingApproval>> GetPendingRoleApproval(Guid? UserUID);
        Task<bool> UpdateRoleApproveAccess(AccessModel access);
        Task<bool> UpdateRoleRejectAccess(AccessModel access);
        Task<List<Products>> GetAllProducts();

        Task<List<PendingApproval>> GetPendingUserApproval(Guid? UserUID);

        Task<bool> PostUserApproveAccess(AccessModel access);
        Task<bool> PostUserRejectAccess(AccessModel access);


        // Task<bool> PostProductAccess(Products product);

    }
    public class UserServices : IUserServices
    {
        private readonly IUsersRepository _usersRepository;
        public UserServices(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public async Task<User> login(Login login)
        {
            return await _usersRepository.login(login);
        }
        public async Task<Roles> Updateroles(UpdateRoles roles)
        {
            var result = await _usersRepository.updateRole(roles);
            AccessModel access = new AccessModel() { ApprovalTypeId = RefApprovalType.Role, ModifiedBy = roles.CreatedBy, ManagerId = roles.ManagerId, Status = 0, UserId = result.Id };
            await _usersRepository.PostUpdateRoleApproval(access);
            return result;
        }

        public async Task<User> PostUser(PostUser user)
        {
            var result = await _usersRepository.PostUser(user);
            AccessModel access = new AccessModel() { ApprovalTypeId = RefApprovalType.User, CreatedBy = user.CreatedBy, ManagerId = user.ManagerId, Status = 0, UserId = result.Id };
            await _usersRepository.PostUpdateUserApproval(access);
            return result;
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _usersRepository.GetAllUsers();
        }

        public async Task<User> GetUsers(Guid uid)
        {
            return await _usersRepository.GetUsers(uid);
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
    }
}
