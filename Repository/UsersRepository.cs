using AutoMapper;
using DmsApi.Helpers;
using DmsApi.Models;
using Dapper;
using Microsoft.AspNetCore.Http.HttpResults;


namespace DmsApi.Repository
{

    public interface IUsersRepository
    {
        Task<User> login(Login login);
        Task<Roles> updateRole(UpdateRoles roles);
        Task<User> PostUser(PostUser user);
        Task<User> ForgotPassword(ForgotPassword forgotPassword);
        Task<List<User>> GetAllUsers();
        Task<User> GetUsers(Guid uid);
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

    }

    public class UsersRepository : IUsersRepository
    {
        protected readonly IUnitOfWork _unitOfWork;


        public UsersRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<User> login(Login login)
        {
            using (var sqlContext = _unitOfWork.ContextFactory())
            {
                var mul = await sqlContext.Connection.QueryFirstAsync<User>("USP_LOGIN", login, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction);
                sqlContext.Commit();
                return mul;
            }


        }

        public async Task<User> PostUser(PostUser user)
        {
            using (var sqlContext = _unitOfWork.ContextFactory())
            {
                user.Password = "123";
                var mul = await sqlContext.Connection.QueryFirstAsync<User>("USP_POSTUSER", user, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
                sqlContext.Commit();
                
                return mul;
            }
        }

        public async Task<User> DeleteUsers(Guid uid, int status)
        {
            using (var sqlContext = _unitOfWork.ContextFactory())
            {
                var mul = await sqlContext.Connection.QueryAsync<User>("USP_DELETEUSERBYUID", new { UID = uid, ModifiedBy = 1, Status = status }, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction);
                sqlContext.Commit();
                return mul.FirstOrDefault();
            }
        }
        public async Task<User> ForgotPassword(ForgotPassword forgotPassword)
        {
            using (var sqlContext = _unitOfWork.ContextFactory())
            {
                var mul = await sqlContext.Connection.QueryAsync<User>("USP_FORGOTPASSWORD", forgotPassword, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction);
                sqlContext.Commit();
                return mul.FirstOrDefault();
            }
        }

        public async Task<List<User>> GetAllUsers()
        {
            using (var connection = _unitOfWork.ConnectionFactory())
            {
                var mul = await connection.QueryMultipleAsync("USP_GETALLUSER", commandType: System.Data.CommandType.StoredProcedure);
                var result = mul.Read<User>().ToList();
                mul.Dispose();
                return result;
            }
        }
        public async Task<Roles> updateRole(UpdateRoles roles)
        {
            using (var sqlContext = _unitOfWork.ContextFactory())
            {
                var mul = await sqlContext.Connection.QueryFirstAsync<Roles>("USP_UPDATEROLE", new
                {
                    UID = roles.UID,
                    RoleDisplayName = roles.RoleDisplayName,
                    CreatedBy = roles.CreatedBy
                }, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
                sqlContext.Commit();
                return mul;
            }
        }

        public async Task<User> GetUsers(Guid uid)
        {
            using (var sqlContext = _unitOfWork.ConnectionFactory())
            {

                var mul = await sqlContext.QueryMultipleAsync("USP_GETUSERBYUID", new { UID = uid }, commandType: System.Data.CommandType.StoredProcedure);
                var Result = mul.Read<User>().FirstOrDefault();
                mul.Dispose();
                return Result;
            }
        }

        public async Task<List<Roles>> GetAllRoles()
        {
            using (var connection = _unitOfWork.ConnectionFactory())
            {
                using (var mul = await connection.QueryMultipleAsync("USP_GETALLROLES", commandType: System.Data.CommandType.StoredProcedure))
                {
                    var Result = mul.Read<Roles>().ToList();
                    mul.Dispose();
                    return Result;
                }
            }
        }
        public async Task<Roles> GetRoles(Guid uid)
        {
            using (var sqlContext = _unitOfWork.ConnectionFactory())
            {

                var mul = await sqlContext.QueryMultipleAsync("USP_GETROLEBYUID", new { UID = uid }, commandType: System.Data.CommandType.StoredProcedure);
                var Result = mul.Read<Roles>().FirstOrDefault();
                mul.Dispose();
                return Result;
            }
        }
        public async Task<List<PendingApproval>> GetPendingRolesApproval(Guid? UserUID)
        {
            try
            {
                using (var connection = _unitOfWork.ConnectionFactory())
                {
                    var result = await connection.QueryAsync<PendingApproval>("USP_GETUSERAPPROVALLIST", new { UserUID = UserUID, ApprovalType = RefApprovalType.Role }, commandType: System.Data.CommandType.StoredProcedure);
                    var Productresult = await connection.QueryAsync<PendingApproval>("USP_GETPRODUCTAPPROVALLIST", new { UserUID = UserUID }, commandType: System.Data.CommandType.StoredProcedure);
                    return result.Concat(Productresult).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> PostUpdateRoleApproval(AccessModel access)
        {
            using (var sqlContext = _unitOfWork.ContextFactory())
            {
                try
                {
                    var inputdata = new { UserId = access.UserId, ManagerId = access.ManagerId, ApprovalTypeId = RefApprovalType.Role, CreatedBy = access.CreatedBy, UID = access.UID, ApprovalStatus = RefApprovalStatus.Pending };
                    var obj = await sqlContext.Connection.QueryAsync("USP_UPDATEROLEAPPROVAL", inputdata, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
                    sqlContext.Commit();
                }
                catch (Exception ex)
                {
                    sqlContext.Rollback();
                    throw ex;
                }
            }

            return true;


        }
        public async Task<bool> UpdateRoleApproveAccess(AccessModel access)
        {
            using (var sqlContext = _unitOfWork.ContextFactory())
            {
                try
                {
                    await sqlContext.Connection.QueryAsync("USP_UPDATEROLEAPPROVAL", new { UserId = access.UserId, ManagerId = access.ManagerId, ApprovalTypeId = access.ApprovalTypeId, ApprovalStatus = RefApprovalStatusU.Approved, CreatedBy = access.CreatedBy, UID = access.UID }, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
                    sqlContext.Commit();

                }
                catch (Exception ex)
                {
                    sqlContext.Rollback();
                    throw ex;
                }
                return true;
            }
        }

        public async Task<bool> UpdateRoleRejectAccess(AccessModel access)
        {
            using (var sqlContext = _unitOfWork.ContextFactory())
            {
                try
                {
                    await sqlContext.Connection.QueryAsync("USP_UPDATEROLEAPPROVAL", new { UserId = access.UserId, ManagerId = access.ManagerId, ApprovalTypeId = access.ApprovalTypeId, ApprovalStatus = RefApprovalStatusU.Rejected, CreatedBy = access.CreatedBy, UID = access.UID }, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
                    sqlContext.Commit();
                }
                catch (Exception ex)
                {
                    sqlContext.Rollback();
                    throw ex;
                }
                return true;
            }
        }
        public async Task<List<Products>> GetAllProducts()
        {
            using (var connection = _unitOfWork.ConnectionFactory())
            {
                using (var mul = await connection.QueryMultipleAsync("USP_GETALLPRODUCTS", commandType: System.Data.CommandType.StoredProcedure))
                {
                    var Result = mul.Read<Products>().ToList();
                    mul.Dispose();
                    return Result;
                }
            }
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
            using (var sqlContext = _unitOfWork.ContextFactory())
            {
                try
                {
                    var inputdata = new { UserId = access.UserId, ManagerId = access.ManagerId, ApprovalTypeId = RefApprovalType.User, CreatedBy = access.CreatedBy, UID = access.UID, ApprovalStatus = RefApprovalStatusU.Pending };
                    var obj = await sqlContext.Connection.QueryAsync<UserProductMapping>("USP_UPDATEUSERAPPROVAL", inputdata, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
                    sqlContext.Commit();
                }
                catch (Exception ex)
                {
                    sqlContext.Rollback();
                    throw ex;
                }
            }

            return true;
            

        }

        public async Task<List<PendingApproval>> GetPendingUserApproval(Guid? UserUID)
        {
            try
            {
                using (var connection = _unitOfWork.ConnectionFactory())
                {
                    var result = await connection.QueryAsync<PendingApproval>("USP_GETUSERAPPROVALLIST", new { UserUID = UserUID, ApprovalType = RefApprovalType.User }, commandType: System.Data.CommandType.StoredProcedure);
                    var Productresult = await connection.QueryAsync<PendingApproval>("USP_GETPRODUCTAPPROVALLIST", new { UserUID = UserUID }, commandType: System.Data.CommandType.StoredProcedure);
                    return result.Concat(Productresult).ToList();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> PostUserApproveAccess(AccessModel access)
        {
            using (var sqlContext = _unitOfWork.ContextFactory())
            {
                try
                {
                    await sqlContext.Connection.QueryAsync("USP_UPDATEUSERAPPROVAL", new { UserId = access.UserId, ManagerId = access.ManagerId, ApprovalTypeId = access.ApprovalTypeId, ApprovalStatus = RefApprovalStatusU.Approved, CreatedBy = access.CreatedBy, UID = access.UID }, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
                    sqlContext.Commit();
                }
                catch (Exception ex)
                {
                    sqlContext.Rollback();
                    throw ex;
                }
                return true;
            }
        }

        public async Task<bool> PostUserRejectAccess(AccessModel access)
        {
            using (var sqlContext = _unitOfWork.ContextFactory())
            {
                try
                {
                    await sqlContext.Connection.QueryAsync("USP_UPDATEUSERAPPROVAL", new { UserId = access.UserId, ManagerId = access.ManagerId, ApprovalTypeId = access.ApprovalTypeId, ApprovalStatus = RefApprovalStatusU.Rejected, CreatedBy = access.CreatedBy, UID = access.UID }, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
                    sqlContext.Commit();
                }
                catch (Exception ex)
                {
                    sqlContext.Rollback();
                    throw ex;
                }
                return true;
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