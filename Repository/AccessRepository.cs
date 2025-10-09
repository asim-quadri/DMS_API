using DmsApi.Helpers;
using DmsApi.Models;
using Dapper;
using System.Xml;

namespace DmsApi.Repository
{
    public interface IAccessRepository
    {
        Task<List<AccessModel>> GetAccessList(Guid? UserUID); 
        Task<List<PendingApproval>> GetPendingApproval(Guid? UserUID);
        Task<bool> PostUserManagement(List<AccessModel> access);
        Task<bool> PostApproveAccess(AccessModel access);
        Task<bool> PostRejectAccess(AccessModel access);
    }
    public class AccessRepository : IAccessRepository
    {
        protected readonly IUnitOfWork _unitOfWork;


        public AccessRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<AccessModel>> GetAccessList(Guid? UserUID)
        {
            using (var connection = _unitOfWork.ConnectionFactory())
            {
                var result = await connection.QueryAsync<AccessModel>("USP_GETACCESSLIST", new { UserUID = UserUID }, commandType: System.Data.CommandType.StoredProcedure);
                return result.ToList();
            }
        }
        public async Task<List<PendingApproval>> GetPendingApproval(Guid? UserUID)
        {
            using (var connection = _unitOfWork.ConnectionFactory())
            {
                var result = await connection.QueryAsync<PendingApproval>("USP_GETAPPROVALLIST", new { UserUID = UserUID }, commandType: System.Data.CommandType.StoredProcedure);
                return result.ToList();
            }
        }


        public async Task<bool> PostUserManagement(List<AccessModel> accessModel)
        {
            List<UserProductMapping> mainresult = new List<UserProductMapping>();
            using (var connection = _unitOfWork.ConnectionFactory())
            {
                var result = await connection.QueryAsync<UserProductMapping>("USP_GETPRODUCTMAPPINGBYUSERID", new { UserId = accessModel[0].UserId }, commandType: System.Data.CommandType.StoredProcedure);
                mainresult = result.ToList();
            }

            foreach (var access in accessModel)
            {
                using (var sqlContext = _unitOfWork.ContextFactory())
                {
                    try
                    {

                        var inputdata = new { UserId = access.UserId, ProductId = access.ProductId, CreatedBy = access.CreatedBy, UID = access.UID, Status = RefApprovalStatusU.Pending, Enable = access.Enable };
                        var result = await sqlContext.Connection.QueryFirstAsync<UserProductMapping>("USP_ADDPRODUCTACCESS", inputdata, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
                        if (mainresult.Count(f => f.UserId == access.UserId && f.ProductId == access.ProductId) <= 0 || ((mainresult.Count(f => f.UserId == access.UserId && f.ProductId == access.ProductId) > 0 && mainresult.Find(f => f.UserId == access.UserId && f.ProductId == access.ProductId).Enable == 0) && access.Enable == 1))
                        {
                            var approvaldata = new { UserId = access.UserId, ManagerId = access.ManagerId, ProductMappingId = result.Id, ApprovalType = RefApprovalType.Access, CreatedBy = access.CreatedBy, UID = access.UID, ApprovalStatus = RefApprovalStatusU.Pending };
                            await sqlContext.Connection.QueryAsync<UserProductMapping>("USP_ADDUPDATEPRODUCTAPPROVAL", approvaldata, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
                        }

                        sqlContext.Commit();
                    }
                    catch (Exception ex)
                    {
                        sqlContext.Rollback();
                        throw ex;

                    }
                }
            }

            return true;
        }

        public async Task<bool> PostApproveAccess(AccessModel access)
        {
            List<PendingApproval> Productresult = new List<PendingApproval>();


            using (var connection = _unitOfWork.ConnectionFactory())
            {

                var result = await connection.QueryAsync<PendingApproval>("USP_GETPRODUCTAPPROVALLIST", new { UserUID = access.UserUID, Unique = 0 }, commandType: System.Data.CommandType.StoredProcedure);
                Productresult = result.ToList();

            }

            foreach (var item in Productresult)
            {

                using (var sqlContext = _unitOfWork.ContextFactory())
                {
                    try
                    {
                        await sqlContext.Connection.QueryAsync("USP_ADDUPDATEPRODUCTAPPROVAL", new { UserId = item.UserId, ManagerId = access.ManagerId, ProductMappingId = item.ProductMappingId, ApprovalType = item.ApprovalTypeId, ApprovalStatus = RefApprovalStatusU.Approved, CreatedBy = access.CreatedBy, UID = item.ApproverUID }, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
                        sqlContext.Commit();
                    }
                    catch (Exception ex)
                    {
                        sqlContext.Rollback();
                        throw ex;
                    }
                   
                }
            }
            return true;
        }

        public async Task<bool> PostRejectAccess(AccessModel access)
        {
            List<PendingApproval> Productresult = new List<PendingApproval>();


            using (var connection = _unitOfWork.ConnectionFactory())
            {

                var result = await connection.QueryAsync<PendingApproval>("USP_GETPRODUCTAPPROVALLIST", new { UserUID = access.UserUID, Unique = 0 }, commandType: System.Data.CommandType.StoredProcedure);
                Productresult = result.ToList();

            }

            foreach (var item in Productresult)
            {

                using (var sqlContext = _unitOfWork.ContextFactory())
                {
                    try
                    {
                        await sqlContext.Connection.QueryAsync("USP_ADDUPDATEPRODUCTAPPROVAL", new { UserId = item.UserId, ManagerId = access.ManagerId, ProductMappingId = item.ProductMappingId, ApprovalType = item.ApprovalTypeId, ApprovalStatus = RefApprovalStatusU.Rejected, CreatedBy = access.CreatedBy, UID = item.ApproverUID }, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
                        sqlContext.Commit();
                    }
                    catch (Exception ex)
                    {
                        sqlContext.Rollback();
                        throw ex;
                    }

                }
            }
            return true;
        }

    }
}
