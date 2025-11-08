using ComplianceAPI.Helpers.Constants;
using ComplianceAPI.Models;
using ComplianceAPI.Models.DataModels;
using ComplianceAPI.Models.Enums;
using Dapper;
using Microsoft.EntityFrameworkCore;

namespace ComplianceAPI.Repository
{
    public interface IEntityTypeRepository
    {
        Task<List<EntityTypeModel>> GetAllEntityTypes();

        Task<EntityTypeModel> GetEntityTypeByUID(Guid uid);

        Task<EntityTypeModel> PostEntityType(EntityTypeModel country);

        Task<bool> PostEntityTypeApprove(AccessModel access, string ApprovalStatus);

        Task<List<EntityTypeModel>> GetCountryEntityTypeMapping();

        Task<EntityTypeModel> PostCountryEntityTypeMapping(EntityTypeModel country);

        Task<List<EntityTypeModel>> GetEntityTypeApprovalList(Guid UserUID);

        Task<List<EntityTypeModel>> GetAllEntityTypeApprovalList();

        Task<List<EntityTypeModel>> GetAllCountryEntityTypeMappingApproval();

        Task<List<EntityTypeModel>> GetCountryEntityTypeMappingApproval(Guid UserUID);

        Task<bool> PostCountryEntityTypeMappingApprove(AccessModel access, string ApprovalStatus);

        Task<bool> AddEntityTypeApprovalNotification(int createdBy, string entityType);

        Task<bool> AddEntityTypeMappingApprovalNotification(int createdBy, long? entityTypeId);

        Task<string> GetNextEntityTypeReferenceCode();

        //Task<bool> AddEntityTypeApprovalSuccessNotification(long createdBy, string approvalStatus);
        //Task<bool> AddEntityTypeMappingApprovalSuccessNotification(long createdBy, string approvalStatus);
    }

    public class EntityTypeRepository : IEntityTypeRepository
    {
        protected readonly IUnitOfWork _unitOfWork;
        private readonly IHelperRepository _helperRepository;
        private readonly ComplianceDbContext _dbContext;

        public EntityTypeRepository(IUnitOfWork unitOfWork, IHelperRepository helperRepository, ComplianceDbContext dbContext)
        {
            _unitOfWork = unitOfWork;
            _helperRepository = helperRepository;
            _dbContext = dbContext;
        }

        public async Task<List<EntityTypeModel>> GetAllEntityTypes()
        {
            using (var connection = _unitOfWork.ConnectionFactory())
            {
                var mul = await connection.QueryMultipleAsync("[product_owner].USP_GETALLENTITYTYPES", commandType: System.Data.CommandType.StoredProcedure);
                var result = mul.Read<EntityTypeModel>().ToList();
                mul.Dispose();
                return result;
            }
        }

        public async Task<EntityTypeModel> GetEntityTypeByUID(Guid uid)
        {
            //using (var sqlContext = _unitOfWork.ConnectionFactory())
            //{
            //    var mul = await sqlContext.QueryMultipleAsync("[product_owner].USP_GETCOUNTRYBYUID", new { UID = uid }, commandType: System.Data.CommandType.StoredProcedure);
            //    var Result = mul.Read<EntityTypeModel>().FirstOrDefault();
            //    mul.Dispose();
            //    return Result;
            //}
            return null;
        }

        public async Task<EntityTypeModel> PostEntityType(EntityTypeModel regulation)
        {
            using (var sqlContext = _unitOfWork.ContextFactory())
            {
                var mul = await sqlContext.Connection.QueryFirstAsync<EntityTypeModel>("[product_owner].USP_POSTENTITYTYPE", new
                {
                    Id = regulation.Id,
                    EntityType = regulation.EntityType,
                    EntityTypeCode = regulation.EntityTypeCode,
                    EntityTypeReferenceCode = regulation.EntityTypeReferenceCode,
                    CreatedBy = regulation.CreatedBy,
                    ModifiedBy = regulation.ModifiedBy,
                    UID = regulation.UID
                }, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
                sqlContext.Commit();

                return mul;
            }
        }

        public async Task<bool> PostEntityTypeApprove(AccessModel access, string ApprovalStatus)
        {
            using (var sqlContext = _unitOfWork.ContextFactory())
            {
                try
                {
                    var inputdata = new { ManagerId = access.ManagerId, CreatedBy = access.CreatedBy, UID = access.UID, ApprovalStatus = ApprovalStatus, EntityTypeId = access.EntityTypeId, EntityTypeApprovalReferenceCode = access.ReferenceCode };

                    var obj = await sqlContext.Connection.QueryAsync<object>("[product_owner].USP_UPDATEENTITYTYPEAPPROVAL", inputdata, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
                    sqlContext.Commit();
                    if (ApprovalStatus != "Pending")
                    {
                        await AddEntrityTypeResponseNotification(access, ApprovalStatus, Models.Enums.ModuleType.EntityType);
                    }
                }
                catch (Exception ex)
                {
                    sqlContext.Rollback();
                    throw ex;
                }
            }

            return true;
        }

        public async Task<List<EntityTypeModel>> GetCountryEntityTypeMapping()
        {
            using (var sqlContext = _unitOfWork.ConnectionFactory())
            {
                var mul = await sqlContext.QueryMultipleAsync("[product_owner].USP_GETCOUNTRYENTITYTYPEMAPPING", commandType: System.Data.CommandType.StoredProcedure);
                var Result = mul.Read<EntityTypeModel>();
                mul.Dispose();
                return Result.ToList();
            }
        }

        public async Task<EntityTypeModel> PostCountryEntityTypeMapping(EntityTypeModel regulation)
        {
            using (var sqlContext = _unitOfWork.ContextFactory())
            {
                var mul = await sqlContext.Connection.QueryFirstOrDefaultAsync<EntityTypeModel>("[product_owner].USP_POSTCOUNTRYENTITYTYPEMAPPING", new
                {
                    Id = regulation.Id,
                    CountryId = regulation.CountryId,
                    EntityTypeId = regulation.EntityTypeId,
                    CreatedBy = regulation.CreatedBy,
                    UID = regulation.UID
                }, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
                sqlContext.Commit();

                return mul;
            }
        }

        public async Task<bool> PostCountryEntityTypeMappingApprove(AccessModel access, string ApprovalStatus)
        {
            using (var sqlContext = _unitOfWork.ContextFactory())
            {
                await sqlContext.Connection.QueryAsync<object>("[product_owner].USP_UPDATECOUNTRYENTITYTYPEMAPPINGAPPROVAL", new
                {
                    CountryEntityTypeMappingId = access.CountryEntityTypeMappingId,
                    ManagerId = access.ManagerId,
                    CreatedBy = access.CreatedBy,
                    UID = access.UID,
                    ApprovalStatus = ApprovalStatus,
                }, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
                sqlContext.Commit();
                if (ApprovalStatus != "Pending")
                {
                    await AddEntrityTypeResponseNotification(access, ApprovalStatus, Models.Enums.ModuleType.EntityTypeMapping);
                }

                return true;
            }
        }

        public async Task<List<EntityTypeModel>> GetEntityTypeApprovalList(Guid UserUID)
        {
            using (var sqlContext = _unitOfWork.ConnectionFactory())
            {
                var mul = await sqlContext.QueryMultipleAsync("[product_owner].USP_GETENTITYTYPEAPPROVALLIST", new { UserUID = UserUID }, commandType: System.Data.CommandType.StoredProcedure);
                var Result = mul.Read<EntityTypeModel>();
                mul.Dispose();
                return Result.ToList();
            }
        }

        public async Task<List<EntityTypeModel>> GetAllEntityTypeApprovalList()
        {
            using (var sqlContext = _unitOfWork.ConnectionFactory())
            {
                var mul = await sqlContext.QueryMultipleAsync("[product_owner].USP_GETALLENTITYTYPEAPPROVALLIST", commandType: System.Data.CommandType.StoredProcedure);
                var Result = mul.Read<EntityTypeModel>();
                mul.Dispose();
                return Result.ToList();
            }
        }

        public async Task<List<EntityTypeModel>> GetAllCountryEntityTypeMappingApproval()
        {
            using (var sqlContext = _unitOfWork.ConnectionFactory())
            {
                var mul = await sqlContext.QueryMultipleAsync("[product_owner].USP_GETALLCOUNTRYENTITYTYPEMAPPINGAPPROVALLIST", commandType: System.Data.CommandType.StoredProcedure);
                var Result = mul.Read<EntityTypeModel>();
                mul.Dispose();
                return Result.ToList();
            }
        }

        public async Task<List<EntityTypeModel>> GetCountryEntityTypeMappingApproval(Guid UserUID)
        {
            using (var sqlContext = _unitOfWork.ConnectionFactory())
            {
                var mul = await sqlContext.QueryMultipleAsync("[product_owner].USP_GETCOUNTRYENTITYTYPEMAPPINGAPPROVALLIST", new { UserUID = UserUID }, commandType: System.Data.CommandType.StoredProcedure);
                var Result = mul.Read<EntityTypeModel>();
                mul.Dispose();
                return Result.ToList();
            }
        }

        public async Task<bool> AddEntityTypeApprovalNotification(int createdBy, string entityType)
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
                        ModuleType = Models.Enums.ModuleType.TypeOfBranch
                    };
                    if (createdByUserDetails.UserRoleId == 1)
                    {
                        notification.NotificationTitle = string.Format(ApiConstants.NewEntityTypeBySuperAdminTitleTemplate, entityType);
                        notification.NotificationMessage = string.Format(ApiConstants.NewEntityTypeBySuperAdminMessageTemplate, entityType);
                        notification.Status = Models.Enums.RefApprovalStatus.Approved;
                    }
                    else
                    {
                        notification.NotificationTitle = string.Format(ApiConstants.NewEntityTypeTitleTemplate, createdByUserDetails.UserName, entityType);
                        notification.NotificationMessage = string.Format(ApiConstants.NewEntityTypeMessageTemplate, createdByUserDetails.UserName, entityType);
                        notification.Status = Models.Enums.RefApprovalStatus.Pending;
                    }
                    await _dbContext.AddAsync(notification);
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> AddEntityTypeMappingApprovalNotification(int createdBy, long? entityTypeId)
        {
            try
            {
                var createdBYUserDetails = await _helperRepository.GetUserAndManagerInfoAsync(createdBy);
                var entityTypeDetails = await _dbContext.EntityType.FindAsync(entityTypeId);
                if (createdBYUserDetails != null && entityTypeDetails != null)
                {
                    var notification = new Notification
                    {
                        NotificationId = Guid.NewGuid().ToString(),
                        SenderUserId = createdBYUserDetails.UserId,
                        SenderUserName = createdBYUserDetails.UserName,
                        RecipientUserId = createdBYUserDetails.ManagerId,
                        RecipientUserName = createdBYUserDetails.ManagerName,
                        ModuleType = Models.Enums.ModuleType.TypeOfBranchMapping,
                        CreatedDate = DateTime.UtcNow,
                        ReadDate = null,
                        MarkAsRead = false
                    };
                    if (createdBYUserDetails.UserRoleId == 1)
                    {
                        notification.NotificationTitle = string.Format(ApiConstants.NewEntityMappingBySuperAdminNotificationTitleTemplate, entityTypeDetails.EntityType);
                        notification.NotificationMessage = string.Format(ApiConstants.NewEntityMappingBySuperAdminNotificationMessageTemplate, entityTypeDetails.EntityType);
                        notification.Status = Models.Enums.RefApprovalStatus.Approved;
                    }
                    else
                    {
                        notification.NotificationTitle = string.Format(ApiConstants.NewEntityMappingNotificationTitleTemplate, createdBYUserDetails.UserName, entityTypeDetails.EntityType);
                        notification.NotificationMessage = string.Format(ApiConstants.NewEntityMappingNotificationMessageTemplate, createdBYUserDetails.UserName, entityTypeDetails.EntityType);
                        notification.Status = Models.Enums.RefApprovalStatus.Pending;
                    }
                    await _dbContext.AddAsync(notification);
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private async Task<bool> AddEntrityTypeResponseNotification(AccessModel access, string ApprovalStatus, Models.Enums.ModuleType moduleType)
        {
            try
            {
                var entityTypeDetails = await _dbContext.EntityTypeApproval.FirstOrDefaultAsync(x => x.UID == access.UID);
                var userInfo = await _helperRepository.GetUserAndManagerInfoAsync(entityTypeDetails.CreatedBy.Value);
                if (userInfo == null)
                {
                    return false;
                }
                var notification = new Notification
                {
                    NotificationId = Guid.NewGuid().ToString(),
                    SenderUserId = userInfo.ManagerId,
                    SenderUserName = userInfo.ManagerName,
                    RecipientUserId = userInfo.UserId,
                    RecipientUserName = userInfo.UserName,
                    CreatedDate = DateTime.UtcNow,
                    ReadDate = null,
                    MarkAsRead = false,
                    ModuleType = moduleType,
                    NotificationTitle = GetApprovalNotificationTitle(moduleType, ApprovalStatus, userInfo.ManagerName!),
                    NotificationMessage = GetApprovalNotificationMessage(moduleType, ApprovalStatus),
                    Status = ApprovalStatus == Models.Enums.RefApprovalStatus.Approved.ToString() ? Models.Enums.RefApprovalStatus.Approved : Models.Enums.RefApprovalStatus.Rejected
                };
                await _dbContext.AddAsync(notification);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static string GetApprovalNotificationMessage(Models.Enums.ModuleType moduleType, string approvalStatus)
        {
            if (moduleType == Models.Enums.ModuleType.EntityType)
            {
                if (approvalStatus == Models.Enums.RefApprovalStatus.Approved.ToString())
                    return "Entity Type Approved";
                if (approvalStatus == Models.Enums.RefApprovalStatus.Rejected.ToString())
                    return "Entity Type Rejected";
                if (approvalStatus == Models.Enums.RefApprovalStatus.Reviewed.ToString())
                    return "Entity Type Reviewed";
                if (approvalStatus == Models.Enums.RefApprovalStatus.Forward.ToString())
                    return "Entity Type Forwarded";
                if (approvalStatus == Models.Enums.RefApprovalStatus.Pending.ToString())
                    return "Entity Type Pending";
            }
            if (moduleType == Models.Enums.ModuleType.EntityTypeMapping)
            {
                if (approvalStatus == Models.Enums.RefApprovalStatus.Approved.ToString())
                    return "Entity Type Mapping Approved";
                if (approvalStatus == Models.Enums.RefApprovalStatus.Rejected.ToString())
                    return "Entity Type Mapping Rejected";
                if (approvalStatus == Models.Enums.RefApprovalStatus.Reviewed.ToString())
                    return "Entity Type Mapping Reviewed";
                if (approvalStatus == Models.Enums.RefApprovalStatus.Forward.ToString())
                    return "Entity Type Mapping Forwarded";
                if (approvalStatus == Models.Enums.RefApprovalStatus.Pending.ToString())
                    return "Entity Type Mapping Pending";
            }
            return $"{approvalStatus} status updated";
        }

        private static string GetApprovalNotificationTitle(Models.Enums.ModuleType moduleType, string approvalStatus, string managerName)
        {
            if (moduleType == Models.Enums.ModuleType.EntityType)
            {
                if (approvalStatus == Models.Enums.RefApprovalStatus.Approved.ToString())
                    return $"Your entity type request has been approved by {managerName}.";
                if (approvalStatus == Models.Enums.RefApprovalStatus.Rejected.ToString())
                    return $"Your entity type request has been rejected by {managerName}.";
                if (approvalStatus == Models.Enums.RefApprovalStatus.Reviewed.ToString())
                    return $"Your entity type request has been reviewed by {managerName}.";
                if (approvalStatus == Models.Enums.RefApprovalStatus.Forward.ToString())
                    return $"Your entity type request has been forwarded by {managerName}.";
                if (approvalStatus == Models.Enums.RefApprovalStatus.Pending.ToString())
                    return $"Your entity type request is pending with {managerName}.";
            }
            if (moduleType == Models.Enums.ModuleType.EntityTypeMapping)
            {
                if (approvalStatus == Models.Enums.RefApprovalStatus.Approved.ToString())
                    return $"Your entity type mapping request has been approved by {managerName}.";
                if (approvalStatus == Models.Enums.RefApprovalStatus.Rejected.ToString())
                    return $"Your entity type mapping request has been rejected by {managerName}.";
                if (approvalStatus == Models.Enums.RefApprovalStatus.Reviewed.ToString())
                    return $"Your entity type mapping request has been reviewed by {managerName}.";
                if (approvalStatus == Models.Enums.RefApprovalStatus.Forward.ToString())
                    return $"Your entity type mapping request has been forwarded by {managerName}.";
                if (approvalStatus == Models.Enums.RefApprovalStatus.Pending.ToString())
                    return $"Your entity type mapping request is pending with {managerName}.";
            }
            return $"Your request has been {approvalStatus.ToLower()} by {managerName}.";
        }

        public async Task<string> GetNextEntityTypeReferenceCode()
        {
            using (var connection = _unitOfWork.ConnectionFactory())
            {
                var count = await connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM [product_owner].[EntityType]");
                int nextNumber = count + 1;
                return $"{ReferencePrefixes.EntityType}{nextNumber.ToString("D3")}";
            }
        }
    }
}