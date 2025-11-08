using ComplianceAPI.Models;
using ComplianceAPI.Models.DataModels;
using Microsoft.EntityFrameworkCore;

namespace ComplianceAPI.Repository
{
    public interface IHelperRepository
    {
        bool IsSuperAdmin(long userId);
        Task<UserWithManager?> GetUserAndManagerInfoAsync(long userId);
    }
    public class HelperRepository : IHelperRepository
    {
        private readonly ComplianceDbContext dbContext;

        public HelperRepository(ComplianceDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public bool IsSuperAdmin(long userId)
        {

            var isSuperAdmin = (from user in dbContext.Users
                                join userRoleMapping in dbContext.UserRoleMapping on user.Id equals userRoleMapping.UserId
                                join refRole in dbContext.RefRoles on userRoleMapping.RoleId equals Convert.ToInt32(refRole.Id)
                                where user.Id == userId && (refRole.RoleName == "SuperAdmin" || refRole.RoleName == "ITSupportAdmin")
                                select user).Any();

            return isSuperAdmin;
        }

        public async Task<UserWithManager?> GetUserAndManagerInfoAsync(long userId)
        {
            var user = await dbContext.Users.FindAsync(userId);
            if (user == null)
            {
                return null;
            }
            
            var userRole = await dbContext.UserRoleMapping.FirstOrDefaultAsync(x => x.UserId == user.Id);
            var manager = await dbContext.Users.FindAsync(user.ManagerId);
           
            if (manager == null)
            {
                return null;
            }

            return new UserWithManager
            {
                UserId = user.Id,
                UserName = user.FullName,            
                ManagerId = manager.Id,
                ManagerName = manager.FullName,
                UserRoleId= userRole.RoleId,
                IsSuperAdmin = IsSuperAdmin(userId)
            };
        }

    }
}
