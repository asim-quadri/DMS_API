using ComplianceAPI.Models;
using ComplianceAPI.Models.DataModels;
using ComplianceAPI.Models.Enums;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ComplianceAPI.Repository
{
    public interface IBillingRepository
    {
        Task<List<BillingLevel>> GetAllBillingLevel();
        Task<List<BillingFrequency>> GetAllBillingFrequency();
        Task<List<Models.ServiceProvider>> GetAllServiceProvider();
        Task<List<BillStatus>> GetAllBillStatus();
        Task<List<DeliveryStatus>> GetAllDeliveryStatus();
        Task<bool> PostBillingDetails(PostBillingDetails billingDetails);
        Task<BillingDetailsView> GetBillingDetailsView(long billingDetailId);
        Task<List<BillingDetailsView>> GetBillingDetailsViewByOrgId(long organizationId);
        Task<bool> UpdateBillingApproval(AccessModel access, string ApprovalStatus);
        Task<List<BillingBasicDetails>> GetBillingDetailsByEntityAsync(long? entityId);
    }
    public class BillingRepository : IBillingRepository
    {
        protected readonly IUnitOfWork _unitOfWork;
        private readonly ComplianceDbContext dbContext;
        string colorName = null;
        int colorCode = 0;
        public BillingRepository(IUnitOfWork unitOfWork, ComplianceDbContext dbContext)
        {
            _unitOfWork = unitOfWork;
            this.dbContext = dbContext;
        }
        public async Task<List<BillingLevel>> GetAllBillingLevel()
        {
            using (var connection = _unitOfWork.ConnectionFactory())
            {
                return dbContext.BillingLevel.ToList();
            }
        }
        public async Task<List<BillingFrequency>> GetAllBillingFrequency()
        {
            using (var connection = _unitOfWork.ConnectionFactory())
            {
                return dbContext.BillingFrequency.ToList();
            }
        }
        public async Task<List<Models.ServiceProvider>> GetAllServiceProvider()
        {
            using (var connection = _unitOfWork.ConnectionFactory())
            {
                return dbContext.ServiceProvider.ToList();
            }
        }
        public async Task<List<BillStatus>> GetAllBillStatus()
        {
            using (var connection = _unitOfWork.ConnectionFactory())
            {
                return dbContext.BillStatus.ToList();
            }
        }
        public async Task<List<DeliveryStatus>> GetAllDeliveryStatus()
        {
            using (var connection = _unitOfWork.ConnectionFactory())
            {
                return dbContext.DeliveryStatus.ToList();
            }
        }
        public async Task<bool> PostBillingDetails(PostBillingDetails billingDetails)
        {
            using (var sqlContext = _unitOfWork.ContextFactory())
            {
                try
                {
                    var obj = await sqlContext.Connection.QueryAsync<object>("[organizations].[USP_POSTBILLINGDETAILS]", billingDetails, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
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
        public async Task<BillingDetailsView> GetBillingDetailsView(long billingDetailId)
        {
            using (var connection = _unitOfWork.ConnectionFactory())
            {
                var parameters = new { Id = billingDetailId, OrganizationId = 0 };
                var mul = await connection.QueryMultipleAsync("[organizations].[USP_GETBILLINGDETAILSVIEW]", parameters, commandType: System.Data.CommandType.StoredProcedure);
                var result = mul.Read<BillingDetailsView>().FirstOrDefault();
                mul.Dispose();
                return result;
            }
        }
        public async Task<List<BillingDetailsView>> GetBillingDetailsViewByOrgId(long organizationId)
        {
            using (var connection = _unitOfWork.ConnectionFactory())
            {
                var parameters = new { Id = 0, OrganizationId = organizationId };
                var mul = await connection.QueryMultipleAsync("[organizations].[USP_GETBILLINGDETAILSVIEW]", parameters, commandType: System.Data.CommandType.StoredProcedure);
                var result = mul.Read<BillingDetailsView>().ToList();
                mul.Dispose();
                return result;
            }
        }
        public async Task<bool> UpdateBillingApproval(AccessModel access, string ApprovalStatus)
        {
            using (var sqlContext = _unitOfWork.ContextFactory())
            {
                try
                {
                    var inputdata = new { ManagerId = access.ManagerId, CreatedBy = access.CreatedBy, ApprovalStatus = ApprovalStatus, BillingDetailId = access.BillingDetailId };
                    var obj = await sqlContext.Connection.QueryAsync<object>("[organizations].[USP_UPDATEBILLINGDETAILAPPROVAL]", inputdata, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
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

        public async Task<List<BillingBasicDetails>> GetBillingDetailsByEntityAsync(long? entityId)
        {
            try
            {
                var billingDetails = await dbContext.BillingDetails
                    .Where(b => b.EntityId == entityId)
                    .ToListAsync();

                var orgIds = billingDetails.Select(b => b.OrganizationId).Distinct().ToList();
                var entityIds = billingDetails.Select(b => b.EntityId).Distinct().ToList();
                var createdByIds = billingDetails.Select(b => b.CreatedBy).Distinct().ToList();
                var createdByIdsLong = createdByIds
                    .Where(id => id.HasValue)
                    .Select(id => (long?)id.Value)
                    .Distinct()
                    .ToList();

                var billStatusIds = billingDetails.Select(b => b.BillStatus).Distinct().ToList();

                var organizations = await dbContext.Organizations
                    .Where(o => orgIds.Contains(o.Id))
                    .ToDictionaryAsync(o => o.Id, o => o.OrganizationName);

                var entities = await dbContext.Entity
                    .Where(e => entityIds.Contains(e.Id))
                    .ToDictionaryAsync(e => e.Id, e => e.EntityName);

                var users = await dbContext.Users
                    .Where(u => u.Id != 0 && createdByIdsLong.Contains(u.Id))
                    .ToDictionaryAsync(u => u.Id, u => u.FullName);

                var billStatuses = await dbContext.BillStatus
                    .Where(bs => billStatusIds.Contains(bs.Id))
                    .ToDictionaryAsync(bs => bs.Id, bs => bs.BillStatusName);

                // Get user-role mappings
                var userRoleMappings = await dbContext.UserRoleMapping
                    .Where(urm => createdByIdsLong.Contains(urm.UserId))
                    .ToListAsync();

                var roleIds = userRoleMappings.Select(urm => urm.RoleId).Distinct().ToList();

                // Get role names
                var roles = await dbContext.RefRoles
                    .Where(r => roleIds.Contains(r.Id))
                    .ToDictionaryAsync(r => r.Id, r => r.RoleName);

                // Build a dictionary for userId -> roleId
                var userIdToRoleId = userRoleMappings
                    .GroupBy(urm => urm.UserId)
                    .ToDictionary(g => g.Key, g => g.First().RoleId);

                var now = DateTime.Now.Date;
                var nowPlus7 = now.AddDays(7);

                var result = billingDetails.Select(b =>
                {
                    var billStatusName = b.BillStatus.HasValue && billStatuses.TryGetValue(b.BillStatus.Value, out var statusName) ? statusName : null;
                    

                    if (string.Equals(billStatusName, "Received", StringComparison.OrdinalIgnoreCase))
                    {
                        SetColor(LevelType.Green);
                    }
                    else if (b.DueDate.HasValue)
                    {
                        var dueDate = b.DueDate.Value.Date;

                        if (dueDate < now)
                            SetColor(LevelType.Red);
                        else if (dueDate <= nowPlus7)
                            SetColor(LevelType.Amber);
                        else
                            SetColor(LevelType.Green);
                    }

                    long? roleId = null;
                    string? roleName = null;
                    if (b.CreatedBy.HasValue && userIdToRoleId.TryGetValue(b.CreatedBy.Value, out var rId))
                    {
                        roleId = rId;
                        if (rId.HasValue)
                            roles.TryGetValue(rId.Value, out roleName);
                    }

                    return new BillingBasicDetails
                    {
                        Id = b.Id,
                        OrganizationId = b.OrganizationId,
                        OrganizationName = organizations.TryGetValue(b.OrganizationId, out var orgName) ? orgName : null,
                        EntityId = b.EntityId,
                        EntityName = b.EntityId.HasValue && entities.TryGetValue(b.EntityId.Value, out var entityName) ? entityName : null,
                        BillNumber = b.BillNumber,
                        BillDate = b.BillDate,
                        ReceivedAmount = b.ReceivedAmount,
                        BillStatus = b.BillStatus,
                        BillStatusName = billStatusName,
                        DueDate = b.DueDate,
                        CreatedById = b.CreatedBy,
                        CreatedByName = b.CreatedBy.HasValue && users.TryGetValue(b.CreatedBy.Value, out var userName) ? userName : null,
                        RoleId = roleId,
                        RoleName = roleName,
                        ColorCode = colorCode,
                        ColorName = colorName
                    };
                }).ToList();

                return result;
            }
            catch (Exception ex)
            {
                return new List<BillingBasicDetails>();
            }
        }
        void SetColor(LevelType level)
        {
            colorName = level.ToString();
            colorCode = (int)level;
        }

    }
}
