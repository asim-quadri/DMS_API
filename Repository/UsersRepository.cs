using ComplianceAPI.Helpers;
using ComplianceAPI.Helpers.Constants;
using ComplianceAPI.Models;
using ComplianceAPI.Models.DataModels;
using Microsoft.EntityFrameworkCore;
using RefApprovalStatus = ComplianceAPI.Helpers.RefApprovalStatus;
using User = ComplianceAPI.Models.User;

namespace ComplianceAPI.Repository
{
    public interface IUsersRepository
    {
        Task<User> login(Login login);

        Task<Roles> updateRoleAdmin(UpdateRoles roles);

        Task<Roles> updateRole(UpdateRoles roles);

        Task<Roles> GetHistoryRoles(long HistoryId);

        Task<User> AdminPostUser(PostUser user);

        Task<User> PostUser(PostUser user);

        Task<User> ForgotPassword(ForgotPassword forgotPassword);

        Task<List<User>> GetAllUsers();

        Task<User> GetUsers(Guid uid);

        Task<User> GetHistoryUsers(long HistoryId);

        Task<User> DeleteUsers(Guid uid, int status);

        Task<List<Roles>> GetAllRoles();

        Task<Roles> GetRoles(Guid uid);

        Task<List<Products>> GetAllProducts();

        //Task<bool> PostProductAccess(Products product);

        Task<bool> PostUpdateUserApproval(AccessModel access);

        Task<bool> PostUpdateRoleApproval(AccessModel access);

        Task<List<PendingApproval>> GetPendingUserApproval(Guid? UserUID);

        Task<List<PendingApproval>> GetPendingRolesApproval(Guid? UserUID);

        Task<bool> UpdateRoleApproveAccess(AccessModel access);

        Task<bool> UpdateRoleRejectAccess(AccessModel access);

        Task<bool> PostUserApproveAccess(AccessModel access);

        Task<bool> PostUserRejectAccess(AccessModel access);

        Task<bool> PostUserReviewAccess(AccessModel access);

        Task<bool> PostUserForwardAccess(AccessModel access);

        Task<bool> AddUserApprovalNotification(long createdBy, string userName, bool isSuperAdmin);

        bool IsSuperAdmin(int userId);

        Task<bool> AddUserRoleUpdationApprovalNotification(long createdBy, string roleDisplayName);
    }

    public class UsersRepository : IUsersRepository
    {
        protected readonly IUnitOfWork _unitOfWork;
        private readonly ComplianceDbContext dbContext;
        private readonly IEmail email;
        private readonly IHelperRepository _helperRepository;

        public UsersRepository(IUnitOfWork unitOfWork, ComplianceDbContext dbContext, IEmail email, IHelperRepository helperRepository)
        {
            _unitOfWork = unitOfWork;
            this.dbContext = dbContext;
            this.email = email;
            _helperRepository = helperRepository;
        }

        // Replace the faulty catch block in the login method with a proper exception handler
        public async Task<User> login(Login login)
        {

            try
            {
                var user = await dbContext.Users
                    .Where(u => (u.Email == login.Email || u.EmpId == login.UserId || u.Mobile == login.MobileNo) && u.Status == 1 && u.Password == login.Password)
                    .Join(
                        dbContext.UserRoleMapping,
                        u => u.Id,
                        urm => urm.UserId,
                        (u, urm) => new { User = u, UserRoleMapping = urm })
                    .Join(
                        dbContext.RefRoles,
                        uurm => uurm.UserRoleMapping.RoleId,
                        r => r.Id,
                        (uurm, r) => new User
                        {
                            Id = uurm.User.Id,
                            UID = uurm.User.UID,
                            EmpId = uurm.User.EmpId,
                            FirstName = uurm.User.FirstName,
                            LastName = uurm.User.LastName,
                            Email = uurm.User.Email,
                            Mobile = uurm.User.Mobile,
                            RoleId = r.Id,
                            Status = uurm.User.Status,
                            ManagerId = uurm.User.ManagerId ?? 0,
                            RoleName = r.RoleName,
                            FullName = uurm.User.FullName
                        })
                    .FirstOrDefaultAsync();

                return user;
            }
            catch (Exception ex)
            {
                // Optionally log the exception here
                return null;
            }
        }

        public async Task<User> AdminPostUser(PostUser user)
        {
            int superAdminCount = 0;
            int responseCode = 1;
            string message = "Successfully Saved";
            ComplianceAPI.Models.DataModels.User userobject = null;

            var existinguser = dbContext.Users.Where(s => s.Id == user.Id).FirstOrDefault();
            //if (existinguser == null)
            //if (existinguser?.EmpId != user.EmpId)
            //{
            //User user;
            if (user.Id == 0 || user.Id == null)
            {
                userobject = new ComplianceAPI.Models.DataModels.User
                {
                    EmpId = user.EmpId,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    FullName = user.FullName,
                    Email = user.Email,
                    Mobile = user.Mobile,
                    Password = new Random().Next(20000, 500000).ToString(),
                    Status = 1,
                    StartDate = user.StartDate,
                    EndDate = user.EndDate,
                    ManagerId = user.ManagerId,
                    CreatedOn = DateTime.Now,
                    CreatedBy = user.CreatedBy,
                    Gender = user.Gender,
                    DateOfBirth = user.DateOfBirth
                };
                dbContext.Users.Add(userobject);
                dbContext.SaveChanges();

                UserRoleMapping userRoleMapping = new UserRoleMapping
                {
                    UserId = userobject.Id,
                    RoleId = user.RoleId,
                    CreatedBy = user.CreatedBy,
                    CreatedOn = DateTime.Now
                };
                dbContext.UserRoleMapping.Add(userRoleMapping);
                dbContext.SaveChanges();
            }
            else
            {
                userobject = dbContext.Users.FirstOrDefault(u => u.UID == user.UID);
                if (user != null)
                {
                    userobject.FirstName = user.FirstName;
                    userobject.LastName = user.LastName;
                    userobject.FullName = user.FullName;
                    userobject.EmpId = user.EmpId;
                    userobject.Email = user.Email;
                    userobject.Mobile = user.Mobile;
                    //userobject.Password = user.Password;
                    userobject.StartDate = user.StartDate;
                    userobject.EndDate = user.EndDate;
                    userobject.ManagerId = user.ManagerId;
                    userobject.ModifiedBy = user.CreatedBy;
                    userobject.ModifiedOn = DateTime.Now;
                    userobject.Gender = user.Gender;
                    userobject.DateOfBirth = user.DateOfBirth;
                    userobject.Status = 0;

                    UserRoleMapping userRoleMapping = dbContext.UserRoleMapping.FirstOrDefault(ur => ur.UserId == user.Id);
                    if (userRoleMapping != null)
                    {
                        userRoleMapping.RoleId = user.RoleId;
                        userRoleMapping.ModifiedBy = user.CreatedBy;
                        userRoleMapping.ModifiedOn = DateTime.Now;
                    }

                    message = "Updated Successfully";
                    dbContext.SaveChanges();
                }
            }
            //}
            //else
            //{
            //    responseCode = 0;
            //    message = "User already Exists";
            //}

            return dbContext.Users
                .Where(u => u.Id == (userobject != null ? userobject.Id : existinguser.Id))
                .Select(u => new User
                {
                    Id = u.Id,
                    EmpId = u.EmpId,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    FullName = u.FullName,
                    Email = u.Email,
                    Mobile = u.Mobile,
                    Status = u.Status,
                    StartDate = u.StartDate,
                    EndDate = u.EndDate,
                    ManagerId = u.ManagerId,
                    CreatedOn = u.CreatedOn,
                    CreatedBy = u.CreatedBy,
                    ModifiedBy = u.ModifiedBy,
                    ModifiedOn = u.ModifiedOn,
                    Password = u.Password,
                    UID = u.UID,
                    Gender = u.Gender,
                    DateOfBirth = u.DateOfBirth,
                    ResponseCode = responseCode,
                    ResponseMessage = message
                }).FirstOrDefault();
        }

        public async Task<User> PostUser(PostUser user)
        {
            int superAdminCount = 0; int responseCode = 1;

            string message = "Sent for Approval";
            ComplianceAPI.Models.DataModels.UsersHistory userobject = null;

            userobject = new ComplianceAPI.Models.DataModels.UsersHistory
            {
                Id = user.Id,
                EmpId = user.EmpId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                FullName = user.FullName,
                Email = user.Email,
                Mobile = user.Mobile,
                Password = new Random().Next(20000, 500000).ToString(),
                Status = 0,
                StartDate = user.StartDate,
                EndDate = user.EndDate,
                ManagerId = user.ManagerId,
                CreatedOn = DateTime.Now,
                CreatedBy = user.CreatedBy,
                Gender = user.Gender,
                DateOfBirth = user.DateOfBirth
            };
            dbContext.UsersHistory.Add(userobject);
            dbContext.SaveChanges();

            UserRoleMappingHistory userRoleMapping = new UserRoleMappingHistory
            {
                UserId = userobject.Id,
                RoleId = user.RoleId,
                CreatedBy = user.CreatedBy,
                CreatedOn = DateTime.Now,
                UserHistoryId = userobject.HistoryId
            };
            dbContext.UserRoleMappingHistory.Add(userRoleMapping);
            dbContext.SaveChanges();

            return dbContext.UsersHistory
                .Where(u => u.HistoryId == userobject.HistoryId)
                .Select(u => new User
                {
                    HistoryId = u.HistoryId,
                    Id = (long)u.Id,
                    EmpId = u.EmpId,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    FullName = u.FullName,
                    Email = u.Email,
                    Mobile = u.Mobile,
                    Status = u.Status,
                    StartDate = u.StartDate,
                    EndDate = u.EndDate,
                    ManagerId = u.ManagerId,
                    CreatedOn = u.CreatedOn,
                    CreatedBy = u.CreatedBy,
                    ModifiedBy = u.ModifiedBy,
                    ModifiedOn = u.ModifiedOn,
                    UID = u.UID,
                    DateOfBirth = u.DateOfBirth,
                    Gender = u.Gender,
                    ResponseCode = responseCode,
                    ResponseMessage = message,
                })
                .FirstOrDefault();
        }

        public async Task<User> DeleteUsers(Guid uid, int status)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.UID == uid);

            if (user != null)
            {
                var id = user.Id;

                var isSuperAdmin = await (
                                     from u in dbContext.Users
                                     join urm in dbContext.UserRoleMapping on u.Id equals urm.UserId
                                     join rr in dbContext.RefRoles on urm.RoleId equals rr.Id
                                     where u.UID == uid && rr.RoleName == "SuperAdmin"
                                     select u
                                     ).AnyAsync();

                var hasActiveUsers = await dbContext.Users
                                     .Where(u => u.Status == 1 && u.ManagerId == id)
                                     .AnyAsync();
                if (!isSuperAdmin && !hasActiveUsers)
                {
                    user.Status = 0;
                    user.ModifiedOn = DateTime.Now;
                    user.ModifiedBy = 1;
                    await dbContext.SaveChangesAsync();

                    return new User() { ResponseCode = 1, ResponseMessage = "User Disabled Successfully" };
                    //return new User() { ResponseCode = 1, ResponseMessage = "Deleted Successfully" };
                }
                return new User() { ResponseCode = 0, ResponseMessage = "In order to delete, Please change the user's role under {user.FullName}" };
            }
            return new User() { ResponseCode = -1, ResponseMessage = "User not found." };

            //using (var sqlContext = _unitOfWork.ContextFactory())
            //{
            //    var mul = await sqlContext.Connection.QueryAsync<User>("USP_DELETEUSERBYUID", new { UID = uid, ModifiedBy = 1, Status = status }, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction);
            //    sqlContext.Commit();
            //    return mul.FirstOrDefault();
            //}
        }

        public async Task<User> ForgotPassword(ForgotPassword forgotPassword)
        {
            var user = dbContext.Users.Where(s => s.Email == forgotPassword.Email).Select(u => new User
            {
                Id = (long)u.Id,
                EmpId = u.EmpId,
                FirstName = u.FirstName,
                LastName = u.LastName,
                FullName = u.FullName,
                Email = u.Email,
                Mobile = u.Mobile,
                Status = u.Status,
                StartDate = u.StartDate,
                EndDate = u.EndDate,
                ManagerId = u.ManagerId,
                CreatedOn = u.CreatedOn,
                CreatedBy = u.CreatedBy,
                ModifiedBy = u.ModifiedBy,
                ModifiedOn = u.ModifiedOn,
                UID = u.UID,
                Password = u.Password,
                ResponseCode = 1,
                ResponseMessage = "Password sent to your registered email address",
            }).FirstOrDefault();
            if (user != null)
            {
                email.Sendemail(user.Email, user.FullName, user.Email, user.Password);
            }
            else
            {
                user = new User()
                {
                    ResponseCode = 1,
                    ResponseMessage = "User not found",
                };
            }
            return user;
            //using (var sqlContext = _unitOfWork.ContextFactory())
            //{
            //    var mul = await sqlContext.Connection.QueryAsync<User>("USP_FORGOTPASSWORD", forgotPassword, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction);
            //    sqlContext.Commit();
            //    return mul.FirstOrDefault();
            //}
        }

        public async Task<List<User>> GetAllUsers()
        {
            try
            {
                // Updated code for activeUsers query:
                var activeUsers = await (from user in dbContext.Users
                                         join userRoleMapping in dbContext.UserRoleMapping on user.Id equals userRoleMapping.UserId
                                         join refRole in dbContext.RefRoles on userRoleMapping.RoleId.Value equals refRole.Id
                                         //where user.Status == 1
                                         orderby user.Status descending
                                         select new User
                                         {
                                             Id = user.Id,
                                             EmpId = user.EmpId,
                                             FirstName = user.FirstName,
                                             LastName = user.LastName,
                                             FullName = user.FullName,
                                             Email = user.Email,
                                             Mobile = user.Mobile,
                                             Status = user.Status,
                                             StartDate = user.StartDate,
                                             EndDate = user.EndDate.HasValue ? user.EndDate.Value.Date : (DateTime?)null,
                                             ManagerId = user.ManagerId,
                                             CreatedOn = user.CreatedOn,
                                             CreatedBy = user.CreatedBy,
                                             ModifiedBy = user.ModifiedBy,
                                             ModifiedOn = user.ModifiedOn,
                                             UID = user.UID,
                                             RoleDisplayName = refRole.RoleDisplayName,
                                             RoleId = userRoleMapping.RoleId,
                                             RoleName = refRole.RoleName,
                                             Gender = user.Gender,
                                             DateOfBirth = user.DateOfBirth.HasValue ? user.DateOfBirth.Value.Date : (DateTime?)null
                                         }).ToListAsync();

                return activeUsers;
            }
            catch (Exception ex)
            {
                return null;
            }

            //using (var connection = _unitOfWork.ConnectionFactory())
            //{
            //    var mul = await connection.QueryMultipleAsync("USP_GETALLUSER", commandType: System.Data.CommandType.StoredProcedure);
            //    var result = mul.Read<User>().ToList();
            //    mul.Dispose();
            //    return result;
            //}
        }

        public async Task<Roles> updateRoleAdmin(UpdateRoles roles)
        {
            try
            {
                var role = await dbContext.RefRoles.FirstOrDefaultAsync(r => r.UID == roles.UID);

                if (role == null)
                {
                    // Role not found
                    return new Roles() { ResponseCode = -1, ResponseMessage = "Role not found." };
                }

                role.RoleDisplayName = roles.RoleDisplayName;
                role.ModifiedBy = roles.CreatedBy;
                role.ModifiedOn = DateTime.Now;
                dbContext.Update(role);
                await dbContext.SaveChangesAsync();
                var response = new Roles();
                Helpers.Mapper.MapProperties(role, response);
                response.ResponseCode = 1; response.ResponseMessage = "Successfully Updated";
                return response;
            }
            catch (Exception ex)
            {
                // Handle exception
                return new Roles() { ResponseCode = -1, ResponseMessage = ex.Message };
            }

            //using (var sqlContext = _unitOfWork.ContextFactory())
            //{
            //    var mul = await sqlContext.Connection.QueryFirstAsync<Roles>("USP_UPDATEROLE", new
            //    {
            //        UID = roles.UID,
            //        RoleDisplayName = roles.RoleDisplayName,
            //        CreatedBy = roles.CreatedBy
            //    }, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
            //    sqlContext.Commit();
            //    return mul;
            //}
        }

        public async Task<Roles> updateRole(UpdateRoles roles)
        {
            try
            {
                var role = await dbContext.RefRoles.FirstOrDefaultAsync(r => r.UID == roles.UID);

                if (role == null)
                {
                    // Role not found
                    return new Roles() { ResponseCode = -1, ResponseMessage = "Role not found." };
                }
                RefRolesHistory refRolesHistory = new RefRolesHistory();
                Helpers.Mapper.MapProperties(role, refRolesHistory);
                refRolesHistory.Id = role.Id;
                refRolesHistory.RoleDisplayName = roles.RoleDisplayName;
                refRolesHistory.CreatedBy = roles.CreatedBy;
                refRolesHistory.CreatedOn = DateTime.Now;
                refRolesHistory.ModifiedBy = null;
                refRolesHistory.ModifiedOn = null;
                dbContext.Add<RefRolesHistory>(refRolesHistory);
                await dbContext.SaveChangesAsync();
                var response = new Roles();
                Helpers.Mapper.MapProperties(refRolesHistory, response);
                response.Id = refRolesHistory.HistoryId;
                response.ResponseCode = 1; response.ResponseMessage = "Successfully Updated";
                return response;
            }
            catch (Exception ex)
            {
                // Handle exception
                return new Roles() { ResponseCode = -1, ResponseMessage = ex.Message };
            }

            //using (var sqlContext = _unitOfWork.ContextFactory())
            //{
            //    var mul = await sqlContext.Connection.QueryFirstAsync<Roles>("USP_UPDATEROLE", new
            //    {
            //        UID = roles.UID,
            //        RoleDisplayName = roles.RoleDisplayName,
            //        CreatedBy = roles.CreatedBy
            //    }, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
            //    sqlContext.Commit();
            //    return mul;
            //}
        }

        public async Task<User> GetUsers(Guid uid)
        {
            var user = (from u in dbContext.Users
                        join um in dbContext.UserRoleMapping on u.Id equals um.UserId
                        join r in dbContext.RefRoles on (Int64)um.RoleId equals r.Id
                        join ud in dbContext.Users on u.ManagerId equals ud.Id into managerJoin
                        from manager in managerJoin.DefaultIfEmpty()
                        where u.UID == uid
                        select new User
                        {
                            Id = u.Id,
                            EmpId = u.EmpId,
                            FirstName = u.FirstName,
                            LastName = u.LastName,
                            FullName = u.FullName,
                            Email = u.Email,
                            Mobile = u.Mobile,
                            Status = u.Status,
                            StartDate = u.StartDate,
                            EndDate = u.EndDate.HasValue ? u.EndDate : (DateTime?)null,
                            ManagerId = u.ManagerId,
                            CreatedOn = u.CreatedOn,
                            CreatedBy = u.CreatedBy,
                            ModifiedBy = u.ModifiedBy,
                            ModifiedOn = u.ModifiedOn,
                            UID = u.UID,
                            RoleDisplayName = r.RoleDisplayName,
                            RoleId = um.RoleId,
                            ManagerName = manager.FullName
                        }).FirstOrDefault();

            return user;

            //using (var sqlContext = _unitOfWork.ConnectionFactory())
            //{
            //    var mul = await sqlContext.QueryMultipleAsync("USP_GETUSERBYUID", new { UID = uid }, commandType: System.Data.CommandType.StoredProcedure);
            //    var Result = mul.Read<User>().FirstOrDefault();
            //    mul.Dispose();
            //    return Result;
            //}
        }

        public async Task<User> GetHistoryUsers(long HistoryId)
        {
            var user = (from u in dbContext.UsersHistory
                        join um in dbContext.UserRoleMappingHistory on u.HistoryId equals um.UserHistoryId
                        join r in dbContext.RefRoles on (Int64)um.RoleId equals r.Id
                        join ud in dbContext.Users on u.ManagerId equals ud.Id into managerJoin
                        from manager in managerJoin.DefaultIfEmpty()
                        where u.HistoryId == HistoryId
                        select new User
                        {
                            HistoryId = u.HistoryId,
                            Id = u.Id,
                            EmpId = u.EmpId,
                            FirstName = u.FirstName,
                            LastName = u.LastName,
                            FullName = u.FullName,
                            Email = u.Email,
                            Mobile = u.Mobile,
                            Status = u.Status,
                            StartDate = u.StartDate,
                            EndDate = u.EndDate.HasValue ? u.EndDate : (DateTime?)null,
                            ManagerId = u.ManagerId,
                            CreatedOn = u.CreatedOn,
                            CreatedBy = u.CreatedBy,
                            ModifiedBy = u.ModifiedBy,
                            ModifiedOn = u.ModifiedOn,
                            UID = u.UID,
                            RoleDisplayName = r.RoleDisplayName,
                            RoleId = um.RoleId,
                            ManagerName = manager.FullName
                        });

            return user.FirstOrDefault(); ;

            //using (var sqlContext = _unitOfWork.ConnectionFactory())
            //{
            //    var mul = await sqlContext.QueryMultipleAsync("USP_GETUSERBYUID", new { UID = uid }, commandType: System.Data.CommandType.StoredProcedure);
            //    var Result = mul.Read<User>().FirstOrDefault();
            //    mul.Dispose();
            //    return Result;
            //}
        }

        public async Task<Roles> GetHistoryRoles(long HistoryId)
        {
            var user = (from u in dbContext.RefRolesHistory
                        join rk in dbContext.UserApproval on u.HistoryId equals rk.HistoryId
                        join r in dbContext.RefRoles on (Int64)u.Id equals r.Id
                        join ud in dbContext.Users on rk.ManagerId equals ud.Id into managerJoin
                        from manager in managerJoin.DefaultIfEmpty()
                        where u.HistoryId == HistoryId
                        select new Roles
                        {
                            HistoryId = u.HistoryId,
                            Id = u.Id,
                            ManagerId = rk.ManagerId,
                            CreatedOn = u.CreatedOn,
                            CreatedBy = u.CreatedBy,
                            ModifiedBy = u.ModifiedBy,
                            ModifiedOn = u.ModifiedOn,
                            UID = u.UID,
                            RoleDisplayName = u.RoleDisplayName,
                            RoleName = r.RoleName,
                            ManagerName = manager.FullName
                        }).FirstOrDefault();

            return user;

            //using (var sqlContext = _unitOfWork.ConnectionFactory())
            //{
            //    var mul = await sqlContext.QueryMultipleAsync("USP_GETUSERBYUID", new { UID = uid }, commandType: System.Data.CommandType.StoredProcedure);
            //    var Result = mul.Read<User>().FirstOrDefault();
            //    mul.Dispose();
            //    return Result;
            //}
        }

        public async Task<List<Roles>> GetAllRoles()
        {
            var Result = dbContext.RefRoles
                .Where(r => r.Status == 1)
                .Select(r => new Roles
                {
                    Id = r.Id,
                    RoleName = r.RoleName,
                    RoleDisplayName = r.RoleDisplayName,
                    Status = r.Status,
                    UID = r.UID
                })
                .ToList();
            return Result;
            //using (var connection = _unitOfWork.ConnectionFactory())
            //{
            //    using (var mul = await connection.QueryMultipleAsync("USP_GETALLROLES", commandType: System.Data.CommandType.StoredProcedure))
            //    {
            //        var Result = mul.Read<Roles>().ToList();
            //        mul.Dispose();
            //        return Result;
            //    }
            //}
        }

        public async Task<Roles> GetRoles(Guid uid)
        {
            var userRoles = await (from u in dbContext.RefRoles
                                   join um in dbContext.UserRoleMapping on u.Id equals um.UserId
                                   join r in dbContext.RefRoles on um.RoleId equals r.Id
                                   join ud in dbContext.Users on u.ManagerId equals ud.Id into managerJoin
                                   from manager in managerJoin.DefaultIfEmpty()
                                   where u.UID == uid
                                   select new ComplianceAPI.Models.Roles
                                   {
                                       Id = u.Id,
                                       Status = u.Status,
                                       // ManagerId = u.ManagerId,
                                       CreatedOn = u.CreatedOn,
                                       // CreatedBy = u.CreatedBy,
                                       // ModifiedBy = u.ModifiedBy,
                                       ModifiedOn = u.ModifiedOn,
                                       UID = u.UID,
                                       RoleName = u.RoleName,
                                       RoleDisplayName = u.RoleDisplayName,
                                       // ManagerName = manager.FullName
                                   }).FirstOrDefaultAsync();

            return userRoles;
            //using (var sqlContext = _unitOfWork.ConnectionFactory())
            //{
            //    var mul = await sqlContext.QueryMultipleAsync("USP_GETROLEBYUID", new { UID = uid }, commandType: System.Data.CommandType.StoredProcedure);
            //    var Result = mul.Read<Roles>().FirstOrDefault();
            //    mul.Dispose();
            //    return Result;
            //}
        }

        public async Task<List<PendingApproval>> GetPendingRolesApproval(Guid? UserUID)
        {
            try
            {
                var approvalTypes = dbContext.RefApprovalType
                                    .Where(at => new[] { "User", "Role", "Access" }.Contains(at.ApprovalType))
                                    .ToDictionary(at => at.ApprovalType, at => at.Id);

                var userAType = approvalTypes.GetValueOrDefault("User");
                var roleAType = approvalTypes.GetValueOrDefault("Role");
                var accessAType = approvalTypes.GetValueOrDefault("Access");

                var userId = dbContext.Users.Where(u => u.UID == UserUID).Select(u => u.Id).FirstOrDefault();

                var userApprovals = (from upa in dbContext.UserApproval
                                     join u in dbContext.RefRolesHistory on upa.HistoryId equals u.HistoryId
                                     join um in dbContext.Users on upa.ManagerId equals um.Id
                                     join uk in dbContext.Users on upa.CreatedBy equals uk.Id
                                     join rat in dbContext.RefApprovalType on upa.ApprovalType equals rat.Id
                                     join ras in dbContext.RefApprovalStatus on upa.ApprovalStatus equals ras.Id
                                     where (upa.ManagerId == userId || upa.CreatedBy == userId) && new[] { userAType, roleAType, accessAType }.Contains(rat.Id) && upa.ApprovalType == roleAType
                                     select new PendingApproval
                                     {
                                         // Id = upa.Id,
                                         HistoryId = u.HistoryId,
                                         RoleName = u.RoleName,
                                         RoleDisplayName = u.RoleDisplayName,
                                         ApproverManager = um != null ? um.ManagerId ?? 0 : 0,
                                         ApprovalType = rat.ApprovalType,
                                         ApprovalTypeId = rat.Id,
                                         Status = ras.Status,
                                         PointofContact = uk.FullName,
                                         approvedBy = um.FullName,
                                         ApproverUID = upa.UID,
                                         // UserUID = u.UID,
                                         CreatedBy = upa.CreatedBy,
                                         CreatedOn = upa.CreatedOn
                                     }).OrderByDescending(s => s.CreatedOn).ToList();

                //var Productresult = await (from u in dbContext.Users
                //                           join um in dbContext.Users on u.ManagerId equals um.Id into managerJoin
                //                           from um in managerJoin.DefaultIfEmpty()
                //                           join ur in dbContext.UserRoleMapping on u.Id equals ur.UserId
                //                           join rr in dbContext.RefRoles on ur.RoleId equals rr.Id
                //                           join upm in dbContext.UserProductMapping on u.Id equals upm.UserId
                //                           join upa in dbContext.UserProductApproval on upm.Id equals upa.ProductMappingId
                //                           join rat in dbContext.RefApprovalType on upa.ApprovalType equals rat.Id
                //                           join ras in dbContext.RefApprovalStatus on upa.ApprovalStatus equals ras.Id
                //                           join rp in dbContext.RefProducts on upm.ProductId equals rp.Id
                //                           join uam in dbContext.Users on upa.ManagerId equals uam.Id into approverJoin
                //                           from uam in approverJoin.DefaultIfEmpty()
                //                           where (upa.ManagerId == userId || upa.CreatedBy == userId) && new[] { userAType, roleAType, accessAType }.Contains(rat.Id)
                //                                 && rp.ParentProductId == null
                //                           select new PendingApproval
                //                           {
                //                               FullName = u.FullName,
                //                               EmpId = u.EmpId,
                //                               RoleDisplayName = rr.RoleDisplayName,
                //                               ApproverManager = uam != null ? uam.ManagerId ?? 0 : 0,
                //                               ApprovalType = rat.ApprovalType,
                //                               ApprovalTypeId = rat.Id,
                //                               Status = ras.Status,
                //                               //PointOfContact = uam.FullName,
                //                               ApproverUID = upa.UID,
                //                               //UserUID = u.UID,
                //                               //ProductMappingId = upm.Id,
                //                               ProductID = rp.Id,
                //                               //ParentProductId = rp.ParentProductId,
                //                               CreatedBy = upa.CreatedBy,
                //                               //UserId = upa.UserId
                //                           }).ToListAsync();

                return userApprovals; //.Concat(Productresult).ToList();

                //using (var connection = _unitOfWork.ConnectionFactory())
                //{
                //    var result = await connection.QueryAsync<PendingApproval>("USP_GETUSERAPPROVALLIST", new { UserUID = UserUID, ApprovalType = ComplianceAPI.Helpers.RefApprovalType.Role }, commandType: System.Data.CommandType.StoredProcedure);
                //    var Productresult = await connection.QueryAsync<PendingApproval>("USP_GETPRODUCTAPPROVALLIST", new { UserUID = UserUID }, commandType: System.Data.CommandType.StoredProcedure);
                //    return result.Concat(Productresult).ToList();
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> PostUpdateRoleApproval(AccessModel access)
        {
            try
            {
                var isAdmin = IsSuperAdmin((int)access.CreatedBy);
                access.ApprovalStatus = RefApprovalStatus.Pending;
                var approvalStatuses = dbContext.RefApprovalStatus
                                    .Where(s => new[] { "Approved", "Reviewed", "Pending", "Forward" }.Contains(s.Status))
                                    .ToDictionary(s => s.Status, s => s.Id);

                var approvedStatusId = approvalStatuses.GetValueOrDefault("Approved");
                var reviewerStatusId = approvalStatuses.GetValueOrDefault("Reviewed");
                var pendingApprovalId = approvalStatuses.GetValueOrDefault("Pending");
                var forwardId = approvalStatuses.GetValueOrDefault("Forward");
                var approvalStatusId = dbContext.RefApprovalStatus.Where(s => s.Status == RefApprovalStatusU.Pending.ToString())
               .Select(s => s.Id)
               .FirstOrDefault();
                var newManagerId = access.ManagerId == 0 ? (int?)null : access.ManagerId;
                if (isAdmin)
                {
                    if (approvalStatusId == approvedStatusId || approvalStatusId == pendingApprovalId)
                    {
                        approvalStatusId = approvedStatusId;
                    }
                }
                var existingUserApproval = await dbContext.UserApproval.FirstOrDefaultAsync(u => u.UID == access.UID);

                if (existingUserApproval == null)
                {
                    var newUserApproval = new UserApproval
                    {
                        UserId = access.UserId,
                        ManagerId = access.ManagerId,
                        ApprovalType = access.ApprovalTypeId,
                        ApprovalStatus = approvalStatusId,
                        CreatedBy = access.CreatedBy,
                        CreatedOn = DateTime.Now,
                        HistoryId = access.HistoryId,
                    };

                    await dbContext.UserApproval.AddAsync(newUserApproval);
                }
                else
                {
                    existingUserApproval.ApprovalStatus = approvalStatusId;
                    existingUserApproval.ModifiedBy = access.CreatedBy;
                    existingUserApproval.ModifiedOn = DateTime.Now;

                    //if ((access.ApprovalStatus == approvedStatusId || access.ApprovalStatus == reviewerStatusId) && newManagerId != null)
                    //{
                    //    var newPendingApproval = new UserApproval
                    //    {
                    //        UserId = access.UserId,
                    //        ManagerId = newManagerId.Value,
                    //        ApprovalType = access.ApprovalTypeId,
                    //        ApprovalStatus = pendingApprovalId,
                    //        CreatedBy = access.CreatedBy,
                    //        CreatedOn = DateTime.Now
                    //    };

                    //    await dbContext.UserApproval.AddAsync(newPendingApproval);
                    //}
                }

                if (approvalStatusId == approvedStatusId || ((approvalStatusId == forwardId || approvalStatusId == approvedStatusId) && isAdmin))
                {
                    var roleToUpdate = await dbContext.RefRoles.FirstOrDefaultAsync(r => r.Id == access.UserId);
                    if (roleToUpdate != null)
                    {
                        roleToUpdate.Status = 1;
                        roleToUpdate.ModifiedBy = access.CreatedBy;
                        roleToUpdate.ModifiedOn = DateTime.Now;
                    }
                }

                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Handle exception
                throw ex;
            }

            //using (var sqlContext = _unitOfWork.ContextFactory())
            //{
            //    try
            //    {
            //        var inputdata = new { UserId = access.UserId, ManagerId = access.ManagerId, ApprovalTypeId = ComplianceAPI.Helpers.RefApprovalType.Role, CreatedBy = access.CreatedBy, UID = access.UID, ApprovalStatus = RefApprovalStatus.Pending };
            //        var obj = await sqlContext.Connection.QueryAsync("USP_UPDATEROLEAPPROVAL", inputdata, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
            //        sqlContext.Commit();
            //    }
            //    catch (Exception ex)
            //    {
            //        sqlContext.Rollback();
            //        throw ex;
            //    }
            //}

            return true;
        }

        public async Task<bool> UpdateRoleApproveAccess(AccessModel access)
        {
            try
            {
                access.ApprovalStatus = RefApprovalStatus.Approved;
                var approvalStatuses = dbContext.RefApprovalStatus
                                    .Where(s => new[] { "Approved", "Reviewed", "Pending", "Forward" }.Contains(s.Status))
                                    .ToDictionary(s => s.Status, s => s.Id);

                var approvedStatusId = approvalStatuses.GetValueOrDefault("Approved");
                var reviewerStatusId = approvalStatuses.GetValueOrDefault("Reviewed");
                var pendingApprovalId = approvalStatuses.GetValueOrDefault("Pending");

                var newManagerId = access.ManagerId == 0 ? (int?)null : access.ManagerId;

                var existingUserApproval = await dbContext.UserApproval.FirstOrDefaultAsync(u => u.UID == access.UID);

                if (existingUserApproval == null)
                {
                    var newUserApproval = new UserApproval
                    {
                        UserId = access.UserId,
                        ManagerId = access.ManagerId,
                        ApprovalType = access.ApprovalTypeId,
                        ApprovalStatus = access.ApprovalStatus,
                        CreatedBy = access.CreatedBy,
                        CreatedOn = DateTime.Now
                    };

                    await dbContext.UserApproval.AddAsync(newUserApproval);
                }
                else
                {
                    existingUserApproval.ApprovalStatus = access.ApprovalStatus;
                    existingUserApproval.ModifiedBy = access.CreatedBy;
                    existingUserApproval.ModifiedOn = DateTime.Now;

                    //if ((access.ApprovalStatus == approvedStatusId || access.ApprovalStatus == reviewerStatusId) && newManagerId != null)
                    //{
                    //    var newPendingApproval = new UserApproval
                    //    {
                    //        UserId = access.UserId,
                    //        ManagerId = newManagerId.Value,
                    //        ApprovalType = access.ApprovalTypeId,
                    //        ApprovalStatus = pendingApprovalId,
                    //        CreatedBy = access.CreatedBy,
                    //        CreatedOn = DateTime.Now
                    //    };

                    //    await dbContext.UserApproval.AddAsync(newPendingApproval);
                    // }
                }

                if (access.ApprovalStatus == approvedStatusId && newManagerId == null)
                {
                    var historyrole = await dbContext.RefRolesHistory.FirstOrDefaultAsync(r => r.HistoryId == access.HistoryId);
                    var roleToUpdate = await dbContext.RefRoles.FirstOrDefaultAsync(r => r.Id == historyrole.Id);

                    if (roleToUpdate != null)
                    {
                        roleToUpdate.RoleDisplayName = historyrole.RoleDisplayName;
                        roleToUpdate.Status = 1;
                        roleToUpdate.ModifiedBy = access.CreatedBy;
                        roleToUpdate.ModifiedOn = DateTime.Now;
                        dbContext.Update(roleToUpdate);
                    }
                }
                await RoleUpdateApprovalResponseNotification(access.HistoryId.Value, RefApprovalStatusU.Approved.ToString());
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Handle exception
                throw ex;
            }

            return true;
            //using (var sqlContext = _unitOfWork.ContextFactory())
            //{
            //    try
            //    {
            //        await sqlContext.Connection.QueryAsync("USP_UPDATEROLEAPPROVAL", new { UserId = access.UserId, ManagerId = access.ManagerId, ApprovalTypeId = access.ApprovalTypeId, ApprovalStatus = RefApprovalStatusU.Approved, CreatedBy = access.CreatedBy, UID = access.UID }, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
            //        sqlContext.Commit();

            //    }
            //    catch (Exception ex)
            //    {
            //        sqlContext.Rollback();
            //        throw ex;
            //    }
            //    return true;
            //}
        }

        public async Task<bool> UpdateRoleRejectAccess(AccessModel access)
        {
            try
            {
                access.ApprovalStatus = RefApprovalStatus.Rejected;

                var approvalStatuses = dbContext.RefApprovalStatus
                                    .Where(s => new[] { "Approved", "Reviewed", "Pending", "Forward" }.Contains(s.Status))
                                    .ToDictionary(s => s.Status, s => s.Id);

                var approvedStatusId = approvalStatuses.GetValueOrDefault("Approved");
                var reviewerStatusId = approvalStatuses.GetValueOrDefault("Reviewed");
                var pendingApprovalId = approvalStatuses.GetValueOrDefault("Pending");

                var newManagerId = access.ManagerId == 0 ? (int?)null : access.ManagerId;

                var existingUserApproval = await dbContext.UserApproval.FirstOrDefaultAsync(u => u.UID == access.UID);

                if (existingUserApproval == null)
                {
                    var newUserApproval = new UserApproval
                    {
                        UserId = access.UserId,
                        ManagerId = access.ManagerId,
                        ApprovalType = access.ApprovalTypeId,
                        ApprovalStatus = access.ApprovalStatus,
                        CreatedBy = access.CreatedBy,
                        CreatedOn = DateTime.Now
                    };

                    await dbContext.UserApproval.AddAsync(newUserApproval);
                }
                else
                {
                    existingUserApproval.ApprovalStatus = access.ApprovalStatus;
                    existingUserApproval.ModifiedBy = access.CreatedBy;
                    existingUserApproval.ModifiedOn = DateTime.Now;

                    if ((access.ApprovalStatus == approvedStatusId || access.ApprovalStatus == reviewerStatusId) && newManagerId != null)
                    {
                        var newPendingApproval = new UserApproval
                        {
                            UserId = access.UserId,
                            ManagerId = newManagerId.Value,
                            ApprovalType = access.ApprovalTypeId,
                            ApprovalStatus = pendingApprovalId,
                            CreatedBy = access.CreatedBy,
                            CreatedOn = DateTime.Now
                        };

                        await dbContext.UserApproval.AddAsync(newPendingApproval);
                    }
                }

                if (access.ApprovalStatus == approvedStatusId && newManagerId == null)
                {
                    var roleToUpdate = await dbContext.RefRoles.FirstOrDefaultAsync(r => r.Id == access.UserId);
                    if (roleToUpdate != null)
                    {
                        roleToUpdate.Status = 1;
                        roleToUpdate.ModifiedBy = access.CreatedBy;
                        roleToUpdate.ModifiedOn = DateTime.Now;
                    }
                }

                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Handle exception
                throw ex;
            }

            return true;
            //using (var sqlContext = _unitOfWork.ContextFactory())
            //{
            //    try
            //    {
            //        await sqlContext.Connection.QueryAsync("USP_UPDATEROLEAPPROVAL", new { UserId = access.UserId, ManagerId = access.ManagerId, ApprovalTypeId = access.ApprovalTypeId, ApprovalStatus = RefApprovalStatusU.Rejected, CreatedBy = access.CreatedBy, UID = access.UID }, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
            //        sqlContext.Commit();
            //    }
            //    catch (Exception ex)
            //    {
            //        sqlContext.Rollback();
            //        throw ex;
            //    }
            //    return true;
            //}
        }

        public async Task<List<Products>> GetAllProducts()
        {
            var products = await (from product in dbContext.RefProducts
                                  select new Products
                                  {
                                      Id = product.Id,
                                      ProductName = product.ProductName,
                                      Status = product.Status,
                                      CreatedOn = product.CreatedOn,
                                      CreatedBy = product.CreatedBy,
                                      ModifiedBy = product.ModifiedBy,
                                      ModifiedOn = product.ModifiedOn,
                                      UID = product.UID
                                  }).ToListAsync();

            return products;
            //using (var connection = _unitOfWork.ConnectionFactory())
            //{
            //    using (var mul = await connection.QueryMultipleAsync("USP_GETALLPRODUCTS", commandType: System.Data.CommandType.StoredProcedure))
            //    {
            //        var Result = mul.Read<Products>().ToList();
            //        mul.Dispose();
            //        return Result;
            //    }
            //}
        }

        //public async Task<bool> PostProductAccess(Products products)
        //{
        //    using (var sqlContext = _unitOfWork.ContextFactory())
        //    {
        //        await sqlContext.Connection.QueryAsync("USP_UPDATACCESSCONTROL", new { UserId = products.UserId, ProductId = products.Id, CreatedBy = products.CreatedBy }, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
        //        sqlContext.Commit();
        //        return true;
        //    }
        //}

        public async Task<bool> PostUpdateUserApproval(AccessModel access)
        {
            var isAdmin = IsSuperAdmin((int)access.CreatedBy);

            var approvalStatuses = dbContext.RefApprovalStatus
                                    .Where(s => new[] { "Approved", "Reviewed", "Pending", "Forward" }.Contains(s.Status))
                                    .ToDictionary(s => s.Status, s => s.Id);

            var approvedStatusId = approvalStatuses.GetValueOrDefault("Approved");
            var reviewerStatusId = approvalStatuses.GetValueOrDefault("Reviewed");
            var pendingApprovalId = approvalStatuses.GetValueOrDefault("Pending");
            var forwardId = approvalStatuses.GetValueOrDefault("Forward");

            var approvalStatusId = dbContext.RefApprovalStatus.Where(s => s.Status == RefApprovalStatusU.Pending.ToString())
                .Select(s => s.Id)
                .FirstOrDefault();

            var newManagerId = access.ManagerId ?? 0;

            if (isAdmin)
            {
                if (approvalStatusId == approvedStatusId || approvalStatusId == pendingApprovalId)
                {
                    approvalStatusId = approvedStatusId;
                }
            }

            if (dbContext.UserApproval.Any(ua => ua.HistoryId == access.HistoryId && ua.ApprovalStatus == pendingApprovalId && ua.CreatedBy == access.CreatedBy))
            {
                return false;
            }

            if (!dbContext.UserApproval.Any(ua => ua.UID == access.UID))
            {
                dbContext.UserApproval.Add(new UserApproval
                {
                    UserId = access.UserId,
                    ManagerId = access.ManagerId,
                    ApprovalType = access.ApprovalTypeId,
                    ApprovalStatus = approvalStatusId,
                    CreatedBy = access.CreatedBy,
                    CreatedOn = DateTime.Now,
                    HistoryId = access.HistoryId
                });
            }
            else
            {
                var userApproval = dbContext.UserApproval.FirstOrDefault(ua => ua.UID == access.UID);
                if (userApproval != null)
                {
                    userApproval.ApprovalStatus = approvalStatusId;
                    userApproval.ModifiedBy = access.CreatedBy;
                    userApproval.ModifiedOn = DateTime.Now;

                    //if ((approvalStatusId == reviewerStatusId || approvalStatusId == forwardId) && !isAdmin)
                    //{
                    //    access.CreatedBy = dbContext.UserApproval.Where(ua => ua.UID == access.UID).Select(ua => ua.CreatedBy).FirstOrDefault();
                    //    dbContext.UserApproval.Add(new UserApproval
                    //    {
                    //        UserId = access.UserId,
                    //        ManagerId = newManagerId,
                    //        ApprovalType = access.ApprovalTypeId,
                    //        ApprovalStatus = pendingApprovalId,
                    //        CreatedBy = access.CreatedBy,
                    //        CreatedOn = DateTime.Now
                    //    });
                    //}
                }
            }

            if (approvalStatusId == approvedStatusId || ((approvalStatusId == forwardId || approvalStatusId == approvedStatusId) && isAdmin))
            {
                var user = dbContext.Users.FirstOrDefault(u => u.Id == access.UserId);
                if (user != null)
                {
                    user.Status = 1;
                    user.ModifiedBy = access.CreatedBy;
                    user.ModifiedOn = DateTime.Now;
                }
            }

            dbContext.SaveChanges();

            //using (var sqlContext = _unitOfWork.ContextFactory())
            //{
            //    try
            //    {
            //        var inputdata = new { UserId = access.UserId, ManagerId = access.ManagerId, ApprovalTypeId = RefApprovalType.User, CreatedBy = access.CreatedBy, UID = access.UID, ApprovalStatus = RefApprovalStatusU.Pending };
            //        var obj = await sqlContext.Connection.QueryAsync<UserProductMapping>("USP_UPDATEUSERAPPROVAL", inputdata, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
            //        sqlContext.Commit();
            //    }
            //    catch (Exception ex)
            //    {
            //        sqlContext.Rollback();
            //        throw ex;
            //    }
            //}

            return true;
        }

        public async Task<List<PendingApproval>> GetPendingUserApproval(Guid? UserUID)
        {
            try
            {
                var approvalTypeIds = dbContext.RefApprovalType
                                    .Where(at => at.ApprovalType == "User" || at.ApprovalType == "Role" || at.ApprovalType == "Access")
                                    .ToDictionary(at => at.ApprovalType, at => at.Id);

                var userAType = approvalTypeIds.GetValueOrDefault("User");
                var roleAType = approvalTypeIds.GetValueOrDefault("Role");
                var accessAType = approvalTypeIds.GetValueOrDefault("Access");

                var userId = dbContext.Users
                    .Where(u => u.UID == UserUID)
                    .Select(u => u.Id)
                    .FirstOrDefault();

                var userApprovals = (from upa in dbContext.UserApproval
                                     join u in dbContext.UsersHistory
                                        on upa.HistoryId equals u.HistoryId
                                     join um in dbContext.Users on upa.ManagerId equals um.Id
                                     join uk in dbContext.Users on upa.CreatedBy equals uk.Id
                                     join ur in dbContext.UserRoleMappingHistory on u.HistoryId equals ur.UserHistoryId
                                     join rr in dbContext.RefRoles on ur.RoleId equals rr.Id
                                     join rat in dbContext.RefApprovalType on upa.ApprovalType equals rat.Id
                                     join ras in dbContext.RefApprovalStatus on upa.ApprovalStatus equals ras.Id
                                     where (upa.ManagerId == userId || upa.CreatedBy == userId) && new[] { userAType, roleAType, accessAType }.Contains(rat.Id) && upa.ApprovalType == userAType
                                     select new PendingApproval
                                     {
                                         // Id = upa.Id,
                                         HistoryId = u.HistoryId,
                                         FullName = u.FullName,
                                         EmpId = u.EmpId,
                                         RoleDisplayName = rr.RoleDisplayName,
                                         ApproverManager = um != null ? um.ManagerId ?? 0 : 0,
                                         ApprovalType = rat.ApprovalType,
                                         ApprovalTypeId = rat.Id,
                                         Status = ras.Status,
                                         PointofContact = uk.FullName,
                                         approvedBy = um.FullName,
                                         ApproverUID = upa.UID,
                                         UserUID = u.UID,
                                         CreatedBy = upa.CreatedBy,
                                         CreatedOn = upa.CreatedOn
                                     }).OrderByDescending(s => s.CreatedOn).ToList();

                //var Productresult = await (from u in dbContext.Users
                //                           join um in dbContext.Users on u.ManagerId equals um.Id into managerJoin
                //                           from um in managerJoin.DefaultIfEmpty()
                //                           join ur in dbContext.UserRoleMapping on u.Id equals ur.UserId
                //                           join rr in dbContext.RefRoles on ur.RoleId equals rr.Id
                //                           join upm in dbContext.UserProductMapping on u.Id equals upm.UserId
                //                           join upa in dbContext.UserProductApproval on upm.Id equals upa.ProductMappingId
                //                           join rat in dbContext.RefApprovalType on upa.ApprovalType equals rat.Id
                //                           join ras in dbContext.RefApprovalStatus on upa.ApprovalStatus equals ras.Id
                //                           join rp in dbContext.RefProducts on upm.ProductId equals rp.Id
                //                           join uam in dbContext.Users on upa.ManagerId equals uam.Id into approverJoin
                //                           from uam in approverJoin.DefaultIfEmpty()
                //                           where (upa.ManagerId == userId || upa.CreatedBy == userId) && new[] { userAType, roleAType, accessAType }.Contains(rat.Id)
                //                                 && rp.ParentProductId == null
                //                           select new PendingApproval
                //                           {
                //                               FullName = u.FullName,
                //                               EmpId = u.EmpId,
                //                               RoleDisplayName = rr.RoleDisplayName,
                //                               ApproverManager = uam != null ? uam.ManagerId ?? 0 : 0,
                //                               ApprovalType = rat.ApprovalType,
                //                               ApprovalTypeId = rat.Id,
                //                               Status = ras.Status,
                //                               PointofContact = uam.FullName,
                //                               ApproverUID = upa.UID,
                //                               UserUID = u.UID,
                //                               ProductMappingId = upm.Id,
                //                               ProductID = rp.Id,
                //                               //ParentProductId = rp.ParentProductId,
                //                               CreatedBy = upa.CreatedBy,
                //                               UserId = upa.UserId
                //                           }).ToListAsync();

                return userApprovals;
                //return userApprovals.Concat(Productresult).ToList();

                //using (var connection = _unitOfWork.ConnectionFactory())
                //{
                //    var result = await connection.QueryAsync<PendingApproval>("USP_GETUSERAPPROVALLIST", new { UserUID = UserUID, ApprovalType = ComplianceAPI.Helpers.RefApprovalType.User }, commandType: System.Data.CommandType.StoredProcedure);
                //    var Productresult = await connection.QueryAsync<PendingApproval>("USP_GETPRODUCTAPPROVALLIST", new { UserUID = UserUID }, commandType: System.Data.CommandType.StoredProcedure);
                //    return result.Concat(Productresult).ToList();
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> PostUserApproveAccess(AccessModel access)
        {
            var isAdmin = IsSuperAdmin((int)access.CreatedBy);

            var statusIds = dbContext.RefApprovalStatus
                            .Where(s => s.Status == "Approved" || s.Status == "Reviewed" || s.Status == "Pending" || s.Status == "Forward")
                            .ToDictionary(s => s.Status, s => s.Id);

            var approvedStatusId = statusIds.GetValueOrDefault("Approved");
            var reviewerStatusId = statusIds.GetValueOrDefault("Reviewed");
            var pendingApprovalId = statusIds.GetValueOrDefault("Pending");
            var forwardId = statusIds.GetValueOrDefault("Forward");

            var approvalStatusId = dbContext.RefApprovalStatus.Where(s => s.Status == RefApprovalStatusU.Approved.ToString())
                .Select(s => s.Id)
                .FirstOrDefault();

            var newManagerId = access.ManagerId ?? 0;

            if (isAdmin)
            {
                if (approvalStatusId == approvedStatusId || approvalStatusId == pendingApprovalId)
                {
                    approvalStatusId = approvedStatusId;
                }
            }

            if (dbContext.UserApproval.Any(ua => ua.UserId == access.UserId && ua.ApprovalStatus == pendingApprovalId && ua.CreatedBy == access.CreatedBy))
            {
                return false;
            }

            if (!dbContext.UserApproval.Any(ua => ua.UID == access.UID))
            {
                dbContext.UserApproval.Add(new UserApproval
                {
                    UserId = access.UserId,
                    ManagerId = access.ManagerId,
                    ApprovalType = access.ApprovalTypeId,
                    ApprovalStatus = approvalStatusId,
                    CreatedBy = access.CreatedBy,
                    CreatedOn = DateTime.Now
                });
            }
            else
            {
                var userApproval = dbContext.UserApproval.FirstOrDefault(ua => ua.UID == access.UID);
                if (userApproval != null)
                {
                    userApproval.ApprovalStatus = approvalStatusId;
                    userApproval.ModifiedBy = access.CreatedBy;
                    userApproval.ModifiedOn = DateTime.Now;

                    if ((approvalStatusId == reviewerStatusId || approvalStatusId == forwardId) && !isAdmin)
                    {
                        access.CreatedBy = dbContext.UserApproval.Where(ua => ua.UID == access.UID).Select(ua => ua.CreatedBy).FirstOrDefault();
                        dbContext.UserApproval.Add(new UserApproval
                        {
                            UserId = access.UserId,
                            ManagerId = newManagerId,
                            ApprovalType = access.ApprovalTypeId,
                            ApprovalStatus = approvedStatusId,
                            CreatedBy = access.CreatedBy,
                            CreatedOn = DateTime.Now
                        });
                    }
                }
            }

            if (approvalStatusId == approvedStatusId || ((approvalStatusId == forwardId || approvalStatusId == approvedStatusId) && isAdmin))
            {
                var user = dbContext.UsersHistory.FirstOrDefault(u => u.HistoryId == access.HistoryId);
                var role = dbContext.UserRoleMappingHistory.Where(f => f.UserHistoryId == access.HistoryId).OrderByDescending(d => d.CreatedOn).FirstOrDefault();
                if (user != null)
                {
                    ComplianceAPI.Models.DataModels.User userobject = null;
                    if (user.Id == 0 || user.Id == null)
                    {
                        userobject = new ComplianceAPI.Models.DataModels.User
                        {
                            EmpId = user.EmpId,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            FullName = user.FullName,
                            Email = user.Email,
                            Mobile = user.Mobile,
                            Password = new Random().Next(20000, 500000).ToString(),
                            Status = 1,
                            StartDate = user.StartDate,
                            EndDate = user.EndDate,
                            ManagerId = user.ManagerId,
                            CreatedOn = DateTime.Now,
                            CreatedBy = user.CreatedBy,
                        };
                        dbContext.Users.Add(userobject);
                        dbContext.SaveChanges();

                        UserRoleMapping userRoleMapping = new UserRoleMapping
                        {
                            UserId = userobject.Id,
                            RoleId = role.RoleId,
                            CreatedBy = user.CreatedBy,
                            CreatedOn = DateTime.Now
                        };
                        dbContext.UserRoleMapping.Add(userRoleMapping);
                        // dbContext.SaveChanges();
                    }
                    else
                    {
                        userobject = dbContext.Users.FirstOrDefault(u => u.Id == user.Id);
                        if (user != null)
                        {
                            userobject.FirstName = user.FirstName;
                            userobject.LastName = user.LastName;
                            userobject.FullName = user.FullName;
                            userobject.EmpId = user.EmpId;
                            userobject.Email = user.Email;
                            userobject.Mobile = user.Mobile;
                            //userobject.Password = user.Password;
                            userobject.StartDate = user.StartDate;
                            userobject.EndDate = user.EndDate;
                            userobject.ManagerId = user.ManagerId;
                            userobject.ModifiedBy = user.CreatedBy;
                            userobject.ModifiedOn = DateTime.Now;
                            userobject.Status = 1;

                            UserRoleMapping userRoleMapping = dbContext.UserRoleMapping.FirstOrDefault(ur => ur.UserId == user.Id);
                            if (userRoleMapping != null)
                            {
                                userRoleMapping.RoleId = role.RoleId;
                                userRoleMapping.ModifiedBy = user.CreatedBy;
                                userRoleMapping.ModifiedOn = DateTime.Now;
                            }
                            dbContext.Update(userobject);
                            dbContext.Update(userRoleMapping);
                            // dbContext.SaveChanges();
                        }
                    }
                    email.Sendemail(userobject.Email, userobject.FullName, userobject.Email, userobject.Password);
                }
            }
            dbContext.SaveChanges();

            return true;

            //using (var sqlContext = _unitOfWork.ContextFactory())
            //{
            //    try
            //    {
            //        await sqlContext.Connection.QueryAsync("USP_UPDATEUSERAPPROVAL", new { UserId = access.UserId, ManagerId = access.ManagerId, ApprovalTypeId = access.ApprovalTypeId, ApprovalStatus = RefApprovalStatusU.Approved, CreatedBy = access.CreatedBy, UID = access.UID }, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
            //        sqlContext.Commit();
            //    }
            //    catch (Exception ex)
            //    {
            //        sqlContext.Rollback();
            //        throw ex;
            //    }
            //    return true;
            //}
        }

        public async Task<bool> PostUserRejectAccess(AccessModel access)
        {
            var isAdmin = IsSuperAdmin((int)access.CreatedBy);

            var approvedStatusId = dbContext.RefApprovalStatus.Where(s => s.Status == "Approved").Select(s => s.Id)
                .FirstOrDefault();

            var reviewerStatusId = dbContext.RefApprovalStatus.Where(s => s.Status == "Reviewed").Select(s => s.Id)
                .FirstOrDefault();

            var pendingApprovalId = dbContext.RefApprovalStatus.Where(s => s.Status == "Pending").Select(s => s.Id)
                .FirstOrDefault();

            var forwardId = dbContext.RefApprovalStatus.Where(s => s.Status == "Forward").Select(s => s.Id)
                .FirstOrDefault();
            var rejectedId = dbContext.RefApprovalStatus.Where(s => s.Status == "Rejected").Select(s => s.Id)
                .FirstOrDefault();

            var approvalStatusId = dbContext.RefApprovalStatus.Where(s => s.Status == RefApprovalStatusU.Rejected.ToString())
                .Select(s => s.Id)
                .FirstOrDefault();

            var newManagerId = access.ManagerId ?? 0;

            if (isAdmin)
            {
                if (approvalStatusId == approvedStatusId || approvalStatusId == pendingApprovalId)
                {
                    approvalStatusId = approvedStatusId;
                }
            }

            if (dbContext.UserApproval.Any(ua => ua.UserId == access.UserId && ua.ApprovalStatus == pendingApprovalId && ua.CreatedBy == access.CreatedBy))
            {
                return false;
            }

            if (!dbContext.UserApproval.Any(ua => ua.UID == access.UID))
            {
                dbContext.UserApproval.Add(new UserApproval
                {
                    UserId = access.UserId,
                    ManagerId = access.ManagerId,
                    ApprovalType = access.ApprovalTypeId,
                    ApprovalStatus = approvalStatusId,
                    CreatedBy = access.CreatedBy,
                    CreatedOn = DateTime.Now
                });
            }
            else
            {
                var userApproval = dbContext.UserApproval.FirstOrDefault(ua => ua.UID == access.UID);
                if (userApproval != null)
                {
                    userApproval.ApprovalStatus = approvalStatusId;
                    userApproval.ModifiedBy = access.CreatedBy;
                    userApproval.ModifiedOn = DateTime.Now;

                    if ((approvalStatusId == reviewerStatusId || approvalStatusId == forwardId) && !isAdmin)
                    {
                        access.CreatedBy = dbContext.UserApproval.Where(ua => ua.UID == access.UID).Select(ua => ua.CreatedBy).FirstOrDefault();
                        dbContext.UserApproval.Add(new UserApproval
                        {
                            UserId = access.UserId,
                            ManagerId = newManagerId,
                            ApprovalType = access.ApprovalTypeId,
                            ApprovalStatus = approvedStatusId,
                            CreatedBy = access.CreatedBy,
                            CreatedOn = DateTime.Now
                        });
                    }
                }
            }

            if (approvalStatusId == approvedStatusId || ((approvalStatusId == forwardId || approvalStatusId == approvedStatusId) && isAdmin))
            {
                var user = dbContext.Users.FirstOrDefault(u => u.Id == access.UserId);
                if (user != null)
                {
                    user.Status = 1;
                    user.ModifiedBy = access.CreatedBy;
                    user.ModifiedOn = DateTime.Now;
                }
            }

            dbContext.SaveChanges();
            await RoleUpdateApprovalResponseNotification(access.HistoryId.Value, RefApprovalStatusU.Rejected.ToString());
            return true;
            //using (var sqlContext = _unitOfWork.ContextFactory())
            //{
            //    try
            //    {
            //        await sqlContext.Connection.QueryAsync("USP_UPDATEUSERAPPROVAL", new { UserId = access.UserId, ManagerId = access.ManagerId, ApprovalTypeId = access.ApprovalTypeId, ApprovalStatus = RefApprovalStatusU.Rejected, CreatedBy = access.CreatedBy, UID = access.UID }, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
            //        sqlContext.Commit();
            //    }
            //    catch (Exception ex)
            //    {
            //        sqlContext.Rollback();
            //        throw ex;
            //    }
            //    return true;
            //}
        }

        public async Task<bool> PostUserReviewAccess(AccessModel access)
        {
            var isAdmin = IsSuperAdmin((int)access.CreatedBy);

            var approvedStatusId = dbContext.RefApprovalStatus.Where(s => s.Status == "Approved").Select(s => s.Id)
                .FirstOrDefault();

            var reviewerStatusId = dbContext.RefApprovalStatus.Where(s => s.Status == "Reviewed").Select(s => s.Id)
                .FirstOrDefault();

            var pendingApprovalId = dbContext.RefApprovalStatus.Where(s => s.Status == "Pending").Select(s => s.Id)
                .FirstOrDefault();

            var forwardId = dbContext.RefApprovalStatus.Where(s => s.Status == "Forward").Select(s => s.Id)
                .FirstOrDefault();
            var rejectedId = dbContext.RefApprovalStatus.Where(s => s.Status == "Rejected").Select(s => s.Id)
                .FirstOrDefault();

            var approvalStatusId = dbContext.RefApprovalStatus.Where(s => s.Status == RefApprovalStatusU.Reviewed.ToString())
                .Select(s => s.Id)
                .FirstOrDefault();

            var newManagerId = access.ManagerId ?? 0;

            if (isAdmin)
            {
                if (approvalStatusId == approvedStatusId || approvalStatusId == pendingApprovalId)
                {
                    approvalStatusId = approvedStatusId;
                }
            }

            if (dbContext.UserApproval.Any(ua => ua.UserId == access.UserId && ua.ApprovalStatus == pendingApprovalId && ua.CreatedBy == access.CreatedBy))
            {
                return false;
            }

            if (!dbContext.UserApproval.Any(ua => ua.UID == access.UID))
            {
                dbContext.UserApproval.Add(new UserApproval
                {
                    UserId = access.UserId,
                    ManagerId = access.ManagerId,
                    ApprovalType = access.ApprovalTypeId,
                    ApprovalStatus = approvalStatusId,
                    CreatedBy = access.CreatedBy,
                    CreatedOn = DateTime.Now
                });
            }
            else
            {
                var userApproval = dbContext.UserApproval.FirstOrDefault(ua => ua.UID == access.UID);
                if (userApproval != null)
                {
                    userApproval.ApprovalStatus = approvalStatusId;
                    userApproval.ModifiedBy = access.CreatedBy;
                    userApproval.ModifiedOn = DateTime.Now;

                    if ((approvalStatusId == reviewerStatusId || approvalStatusId == forwardId) && !isAdmin)
                    {
                        access.CreatedBy = dbContext.UserApproval.Where(ua => ua.UID == access.UID).Select(ua => ua.CreatedBy).FirstOrDefault();
                        dbContext.UserApproval.Add(new UserApproval
                        {
                            UserId = access.UserId,
                            ManagerId = newManagerId,
                            ApprovalType = access.ApprovalTypeId,
                            ApprovalStatus = approvedStatusId,
                            CreatedBy = access.CreatedBy,
                            CreatedOn = DateTime.Now
                        });
                    }
                }
            }

            if (approvalStatusId == approvedStatusId || ((approvalStatusId == forwardId || approvalStatusId == approvedStatusId) && isAdmin))
            {
                var user = dbContext.Users.FirstOrDefault(u => u.Id == access.UserId);
                if (user != null)
                {
                    user.Status = 1;
                    user.ModifiedBy = access.CreatedBy;
                    user.ModifiedOn = DateTime.Now;
                }
            }

            dbContext.SaveChanges();

            return true;
            //using (var sqlContext = _unitOfWork.ContextFactory())
            //{
            //    try
            //    {
            //        await sqlContext.Connection.QueryAsync("USP_UPDATEUSERAPPROVAL", new { UserId = access.UserId, ManagerId = access.ManagerId, ApprovalTypeId = access.ApprovalTypeId, ApprovalStatus = RefApprovalStatusU.Reviewed, CreatedBy = access.CreatedBy, UID = access.UID }, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
            //        sqlContext.Commit();
            //    }
            //    catch (Exception ex)
            //    {
            //        sqlContext.Rollback();
            //        throw ex;
            //    }
            //    return true;
            //}
        }

        public async Task<bool> PostUserForwardAccess(AccessModel access)
        {
            var isAdmin = IsSuperAdmin((int)access.CreatedBy);

            var approvedStatusId = dbContext.RefApprovalStatus.Where(s => s.Status == "Approved").Select(s => s.Id)
                .FirstOrDefault();

            var reviewerStatusId = dbContext.RefApprovalStatus.Where(s => s.Status == "Reviewed").Select(s => s.Id)
                .FirstOrDefault();

            var pendingApprovalId = dbContext.RefApprovalStatus.Where(s => s.Status == "Pending").Select(s => s.Id)
                .FirstOrDefault();

            var forwardId = dbContext.RefApprovalStatus.Where(s => s.Status == "Forward").Select(s => s.Id)
                .FirstOrDefault();
            var rejectedId = dbContext.RefApprovalStatus.Where(s => s.Status == "Rejected").Select(s => s.Id)
                .FirstOrDefault();

            var approvalStatusId = dbContext.RefApprovalStatus.Where(s => s.Status == RefApprovalStatusU.Forward.ToString())
                .Select(s => s.Id)
                .FirstOrDefault();

            var newManagerId = access.ManagerId ?? 0;

            if (isAdmin)
            {
                if (approvalStatusId == approvedStatusId || approvalStatusId == pendingApprovalId)
                {
                    approvalStatusId = approvedStatusId;
                }
            }

            if (dbContext.UserApproval.Any(ua => ua.UserId == access.UserId && ua.ApprovalStatus == pendingApprovalId && ua.CreatedBy == access.CreatedBy))
            {
                return false;
            }

            if (!dbContext.UserApproval.Any(ua => ua.UID == access.UID))
            {
                dbContext.UserApproval.Add(new UserApproval
                {
                    UserId = access.UserId,
                    ManagerId = access.ManagerId,
                    ApprovalType = access.ApprovalTypeId,
                    ApprovalStatus = approvalStatusId,
                    CreatedBy = access.CreatedBy,
                    CreatedOn = DateTime.Now
                });
            }
            else
            {
                var userApproval = dbContext.UserApproval.FirstOrDefault(ua => ua.UID == access.UID);
                if (userApproval != null)
                {
                    userApproval.ApprovalStatus = approvalStatusId;
                    userApproval.ModifiedBy = access.CreatedBy;
                    userApproval.ModifiedOn = DateTime.Now;

                    if ((approvalStatusId == reviewerStatusId || approvalStatusId == forwardId) && !isAdmin)
                    {
                        access.CreatedBy = dbContext.UserApproval.Where(ua => ua.UID == access.UID).Select(ua => ua.CreatedBy).FirstOrDefault();
                        dbContext.UserApproval.Add(new UserApproval
                        {
                            UserId = access.UserId,
                            ManagerId = newManagerId,
                            ApprovalType = access.ApprovalTypeId,
                            ApprovalStatus = approvalStatusId,
                            CreatedBy = access.CreatedBy,
                            CreatedOn = DateTime.Now,
                            HistoryId = access.HistoryId
                        });
                    }
                }
            }

            if (approvalStatusId == approvedStatusId || ((approvalStatusId == forwardId || approvalStatusId == approvedStatusId) && isAdmin))
            {
                var user = dbContext.Users.FirstOrDefault(u => u.Id == access.UserId);
                if (user != null)
                {
                    user.Status = 1;
                    user.ModifiedBy = access.CreatedBy;
                    user.ModifiedOn = DateTime.Now;
                }
            }

            dbContext.SaveChanges();

            return true;
            //using (var sqlContext = _unitOfWork.ContextFactory())
            //{
            //    try
            //    {
            //        await sqlContext.Connection.QueryAsync("USP_UPDATEUSERAPPROVAL", new { UserId = access.UserId, ManagerId = access.ManagerId, ApprovalTypeId = access.ApprovalTypeId, ApprovalStatus = RefApprovalStatusU.Forward, CreatedBy = access.CreatedBy, UID = access.UID }, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
            //        sqlContext.Commit();
            //    }
            //    catch (Exception ex)
            //    {
            //        sqlContext.Rollback();
            //        throw ex;
            //    }
            //    return true;
            //}
        }

        public bool IsSuperAdmin(int userId)
        {
            var isSuperAdmin = (from user in dbContext.Users
                                join userRoleMapping in dbContext.UserRoleMapping on user.Id equals userRoleMapping.UserId
                                join refRole in dbContext.RefRoles on userRoleMapping.RoleId equals Convert.ToInt32(refRole.Id)
                                where user.Id == userId && (refRole.RoleName == "SuperAdmin" || refRole.RoleName == "ITSupportAdmin")
                                select user).Any();

            return isSuperAdmin;
        }

        public async Task<bool> AddUserApprovalNotification(long createdBy, string userName, bool isSuperAdmin)
        {
            try
            {
                var createdByDetails = await _helperRepository.GetUserAndManagerInfoAsync(createdBy);
                if (createdByDetails != null)
                {
                    var notification = new Notification
                    {
                        NotificationId = Guid.NewGuid().ToString(),
                        NotificationTitle = isSuperAdmin
                            ? string.Format(ApiConstants.NewUserBySuperAdminrNotificationTitleTemplate, userName)
                            : string.Format(ApiConstants.NewUserNotificationTitleTemplate, createdByDetails.UserName, userName),
                        NotificationMessage = isSuperAdmin
                            ? $"User {userName} has been created and approved by Super Admin."
                            : string.Format(ApiConstants.NewUserNotificationMessageTemplate, createdByDetails.UserName, userName),
                        SenderUserId = createdByDetails.UserId,
                        RecipientUserId = createdByDetails.ManagerId,
                        CreatedDate = DateTime.UtcNow,
                        Status = isSuperAdmin
                            ? Models.Enums.RefApprovalStatus.Approved
                            : Models.Enums.RefApprovalStatus.Pending,
                        ModuleType = Models.Enums.ModuleType.User,
                        SenderUserName = createdByDetails?.UserName,
                        RecipientUserName = createdByDetails?.ManagerName,
                        MarkAsRead = false
                    };

                    dbContext.Notifications.Add(notification);
                    await dbContext.SaveChangesAsync();

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> AddUserRoleUpdationApprovalNotification(long createdBy, string roleDisplayName)
        {
            try
            {
                var createdByUserDetails = await _helperRepository.GetUserAndManagerInfoAsync(createdBy);
                if (createdByUserDetails != null)
                {
                    var notification = new Notification
                    {
                        NotificationId = Guid.NewGuid().ToString(),
                        SenderUserId = createdByUserDetails.UserId,
                        SenderUserName = createdByUserDetails.UserName,
                        RecipientUserId = createdByUserDetails.ManagerId,
                        RecipientUserName = createdByUserDetails.ManagerName,
                        CreatedDate = DateTime.UtcNow,
                        ReadDate = null,
                        MarkAsRead = false,
                        ModuleType = Models.Enums.ModuleType.Role
                    };
                    if (createdByUserDetails.UserRoleId == 1)
                    {
                        notification.NotificationTitle = string.Format(ApiConstants.RoleUpdationBySuperAdminNotificationTitleTemplate, roleDisplayName);
                        notification.NotificationMessage = string.Format(ApiConstants.RoleUpdationBySuperAdminNotificationMessageTemplate, roleDisplayName);
                        notification.Status = Models.Enums.RefApprovalStatus.Approved;
                    }
                    else
                    {
                        notification.NotificationTitle = string.Format(ApiConstants.RoleUpdationNotificationTitleTemplate, createdByUserDetails.UserName, roleDisplayName);
                        notification.NotificationMessage = string.Format(ApiConstants.RoleUpdationNotificationMessageTemplate, createdByUserDetails.UserName, roleDisplayName);
                        notification.Status = Models.Enums.RefApprovalStatus.Pending;
                    }
                    await dbContext.AddAsync(notification);
                    await dbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private async Task<bool> RoleUpdateApprovalResponseNotification(long roleHistoryId, string approvalStatus)
        {
            try
            {
                var roleHistoryDetails = await dbContext.RefRolesHistory.FindAsync(roleHistoryId);
                if (roleHistoryDetails != null)
                {
                    var userDetails = await _helperRepository.GetUserAndManagerInfoAsync(roleHistoryDetails.CreatedBy.Value);
                    if (userDetails != null)
                    {
                        var notification = new Notification
                        {
                            NotificationId = Guid.NewGuid().ToString(),
                            SenderUserId = userDetails.ManagerId,
                            SenderUserName = userDetails.ManagerName,
                            RecipientUserId = userDetails.UserId,
                            RecipientUserName = userDetails.UserName,
                            ModuleType = Models.Enums.ModuleType.Organization,
                            CreatedDate = DateTime.UtcNow,
                            ReadDate = null,
                            MarkAsRead = false
                        };
                        if (approvalStatus == Models.Enums.RefApprovalStatus.Approved.ToString())
                        {
                            notification.Status = Models.Enums.RefApprovalStatus.Approved;
                            notification.NotificationTitle = $"<b>{userDetails.ManagerName}</b> approved a <b>Role Update</b>";
                            notification.NotificationMessage = $"<b>{userDetails.ManagerName}</b> approved a <b>Role Update</b>";
                        }
                        else if (approvalStatus == Models.Enums.RefApprovalStatus.Rejected.ToString())
                        {
                            notification.Status = Models.Enums.RefApprovalStatus.Rejected;
                            notification.NotificationTitle = $"<b>{userDetails.ManagerName}</b> rejected the <b>Role Update</b>";
                            notification.NotificationMessage = $"<b>{userDetails.ManagerName}</b> rejected the <b>Role Update</b>";
                        }

                        await dbContext.AddAsync(notification);
                        await dbContext.SaveChangesAsync();
                        return true;
                    }
                    return false;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}

//using (var sqlContext = _unitOfWork.ContextFactory())
//{
//    try
//    {
//        var inputdata = new { UserId = access.UserId, ProductId = access.ProductId, Status = access.Status, CreatedBy = access.CreatedBy, UID = access.UID };
//        var obj = await sqlContext.Connection.QueryFirstAsync<UserProductMapping>("USP_ADDUSERACCESS", inputdata, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);

//        await sqlContext.Connection.QueryAsync("USP_ADDUPDATEPRODUCTAPPROVAL", new { UserId = access.UserId, ManagerId = access.ManagerId, ProductMappingId = obj.Id, ApprovalType = access.ApprovalType, ApprovalStatus = RefApprovalStatus.Pending, CreatedBy = access.CreatedBy, UID = access.UID }, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
//        sqlContext.Commit();
//    }
//    catch (Exception ex)
//    {
//        sqlContext.Rollback();
//    }
//    return true;
//}