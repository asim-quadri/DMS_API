using ComplianceAPI.Helpers;
using ComplianceAPI.Helpers.Constants;
using ComplianceAPI.Models;
using ComplianceAPI.Models.DataModels;
using ComplianceAPI.Models.Enums;
using Dapper;
using Microsoft.EntityFrameworkCore;

namespace ComplianceAPI.Repository
{
    public interface IRegulationGroupRepository
    {
        Task<List<RegulationGroupModel>> GetAllRegulationGroups();

        Task<RegulationGroupModel> GetRegulationGroupByUID(Guid uid);

        Task<RegulationGroupModel> PostRegulationGroup(RegulationGroupModel country);

        Task<bool> PostRegulationGroupApprove(AccessModel access, string ApprovalStatus);

        Task<List<RegulationGroupModel>> GetCountryRegulationGroupMapping();

        Task<RegulationGroupModel> PostCountryRegulationGroupMapping(RegulationGroupModel country);

        Task<List<RegulationGroupModel>> GetRegulationGroupApprovalList(Guid UserUID);

        Task<List<RegulationGroupModel>> GetAllRegulationGroupApprovalList();

        Task<List<RegulationGroupModel>> GetAllCountryRegulationGroupMappingApproval();

        Task<List<RegulationGroupModel>> GetCountryRegulationGroupMappingApproval(Guid UserUID);

        Task<bool> PostCountryRegulationGroupMappingApprove(AccessModel access, string ApprovalStatus);

        Task<bool> AddNewRegulationGroupApprovalNotification(int createdBy, string regulationGroupName);

        Task<bool> AddNewRegulationGroupMappingApprovalNotification(int createdBy, long? regulationGroupId);

        Task<string> GetNextRegulationGroupCode();
    }

    public class RegulationGroupRepository : IRegulationGroupRepository
    {
        protected readonly IUnitOfWork _unitOfWork;
        private readonly IHelperRepository _helperRepository;
        private readonly ComplianceDbContext _dbContext;

        public RegulationGroupRepository(IUnitOfWork unitOfWork, IHelperRepository helperRepository, ComplianceDbContext dbContext)
        {
            _unitOfWork = unitOfWork;
            _helperRepository = helperRepository;
            _dbContext = dbContext;
        }

        public async Task<List<RegulationGroupModel>> GetAllRegulationGroups()
        {
            using (var connection = _unitOfWork.ConnectionFactory())
            {
                var mul = await connection.QueryMultipleAsync("[product_owner].USP_GETALLREGULATIONGROUP", commandType: System.Data.CommandType.StoredProcedure);
                var result = mul.Read<RegulationGroupModel>().ToList();
                mul.Dispose();
                return result;
            }
        }

        public async Task<RegulationGroupModel> GetRegulationGroupByUID(Guid uid)
        {
            //using (var sqlContext = _unitOfWork.ConnectionFactory())
            //{
            //    var mul = await sqlContext.QueryMultipleAsync("[product_owner].USP_GETCOUNTRYBYUID", new { UID = uid }, commandType: System.Data.CommandType.StoredProcedure);
            //    var Result = mul.Read<RegulationGroupModel>().FirstOrDefault();
            //    mul.Dispose();
            //    return Result;
            //}
            return null;
        }

        public async Task<RegulationGroupModel> PostRegulationGroup(RegulationGroupModel regulation)
        {
            try
            {
                using (var sqlContext = _unitOfWork.ContextFactory())
                {
                    var mul = await sqlContext.Connection.QueryFirstAsync<RegulationGroupModel>(
                        "[product_owner].USP_POSTREGULATIONGROUP",
                        new
                        {
                            Id = regulation.Id,
                            RegulationGroupName = regulation.RegulationGroupName,
                            RegulationGroupCode = regulation.RegulationGroupCode,
                            RegulationGroupReferenceCode = regulation.RegulationGroupReferenceCode,
                            CreatedBy = regulation.CreatedBy,
                            ModifiedBy = regulation.ModifiedBy,
                            UID = regulation.UID
                        },
                        commandType: System.Data.CommandType.StoredProcedure,
                        transaction: sqlContext.Transaction
                    ).ConfigureAwait(false);

                    sqlContext.Commit();
                    return mul;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> PostRegulationGroupApprove(AccessModel access, string ApprovalStatus)
        {
            using (var sqlContext = _unitOfWork.ContextFactory())
            {
                try
                {
                    var inputdata = new
                    {
                        ManagerId = access.ManagerId,
                        CreatedBy = access.CreatedBy,
                        UID = access.UID,
                        ApprovalStatus = ApprovalStatus,
                        RegulationGroupId = access.RegulationGroupId,
                        RegulationGroupApprovalReferenceCode = access.ReferenceCode // ✅ Correct property name
                    };

                    var obj = await sqlContext.Connection.QueryAsync<object>(
                        "[product_owner].USP_UPDATEREGULATIONGROUPAPPROVAL",
                        inputdata,
                        commandType: System.Data.CommandType.StoredProcedure,
                        transaction: sqlContext.Transaction
                    ).ConfigureAwait(false);

                    sqlContext.Commit();

                    await AddRegulationGroupResponseNotification(access, ApprovalStatus, Models.Enums.ModuleType.RegulationGroup);
                }
                catch (Exception ex)
                {
                    sqlContext.Rollback();
                    throw;
                }
            }

            return true;
        }

        public async Task<List<RegulationGroupModel>> GetCountryRegulationGroupMapping()
        {
            using (var sqlContext = _unitOfWork.ConnectionFactory())
            {
                var mul = await sqlContext.QueryMultipleAsync("[product_owner].USP_GETCOUNTRYREGULATIONGROUPMAPPING", commandType: System.Data.CommandType.StoredProcedure);
                var Result = mul.Read<RegulationGroupModel>();
                mul.Dispose();
                return Result.ToList();
            }
        }

        public async Task<RegulationGroupModel> PostCountryRegulationGroupMapping(RegulationGroupModel regulation)
        {
            using (var sqlContext = _unitOfWork.ContextFactory())
            {
                var mul = await sqlContext.Connection.QueryFirstAsync<RegulationGroupModel>("[product_owner].USP_POSTCOUNTRYREGULATIONGROUPMAPPING", new
                {
                    Id = regulation.Id,
                    CountryId = regulation.CountryId,
                    RegulationGroupId = regulation.RegulationGroupId,
                    CreatedBy = regulation.CreatedBy,
                    UID = regulation.UID
                }, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
                sqlContext.Commit();

                return mul;
            }
        }

        public async Task<bool> PostCountryRegulationGroupMappingApprove(AccessModel access, string ApprovalStatus)
        {
            using (var sqlContext = _unitOfWork.ContextFactory())
            {
                await sqlContext.Connection.QueryAsync<object>("[product_owner].USP_UPDATECOUNTRYREGULATIONGROUPMAPPINGAPPROVAL", new
                {
                    CountryRegulationGroupMappingId = access.CountryRegulationGroupMappingId,
                    ManagerId = access.ManagerId,
                    CreatedBy = access.CreatedBy,
                    UID = access.UID,
                    ApprovalStatus = ApprovalStatus,
                }, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
                sqlContext.Commit();
                await AddRegulationGroupResponseNotification(access, ApprovalStatus, Models.Enums.ModuleType.RegulationGroupMapping);
                return true;
            }
        }

        public async Task<List<RegulationGroupModel>> GetRegulationGroupApprovalList(Guid UserUID)
        {
            using (var sqlContext = _unitOfWork.ConnectionFactory())
            {
                var mul = await sqlContext.QueryMultipleAsync("[product_owner].USP_GETREGULATIONGROUPAPPROVALLIST", new { UserUID = UserUID }, commandType: System.Data.CommandType.StoredProcedure);
                var Result = mul.Read<RegulationGroupModel>();
                mul.Dispose();
                return Result.ToList();
            }
        }

        public async Task<List<RegulationGroupModel>> GetAllRegulationGroupApprovalList()
        {
            using (var sqlContext = _unitOfWork.ConnectionFactory())
            {
                var mul = await sqlContext.QueryMultipleAsync("[product_owner].USP_GETALLREGULATIONGROUPAPPROVALLIST", commandType: System.Data.CommandType.StoredProcedure);
                var Result = mul.Read<RegulationGroupModel>();
                mul.Dispose();
                return Result.ToList();
            }
        }

        public async Task<List<RegulationGroupModel>> GetAllCountryRegulationGroupMappingApproval()
        {
            using (var sqlContext = _unitOfWork.ConnectionFactory())
            {
                var mul = await sqlContext.QueryMultipleAsync("[product_owner].USP_GETALLCOUNTRYREGULATIONGROUPMAPPINGAPPROVALLIST", commandType: System.Data.CommandType.StoredProcedure);
                var Result = mul.Read<RegulationGroupModel>();
                mul.Dispose();
                return Result.ToList();
            }
        }

        public async Task<List<RegulationGroupModel>> GetCountryRegulationGroupMappingApproval(Guid UserUID)
        {
            using (var sqlContext = _unitOfWork.ConnectionFactory())
            {
                var mul = await sqlContext.QueryMultipleAsync("[product_owner].USP_GETCOUNTRYREGULATIONGROUPMAPPINGAPPROVALLIST", new { UserUID = UserUID }, commandType: System.Data.CommandType.StoredProcedure);
                var Result = mul.Read<RegulationGroupModel>();
                mul.Dispose();
                return Result.ToList();
            }
        }

        public async Task<bool> AddNewRegulationGroupApprovalNotification(int createdBy, string regulationGroupName)
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
                        ModuleType = Models.Enums.ModuleType.RegulationGroup
                    };
                    if (createdByUserDetails.UserRoleId == 1)
                    {
                        notification.NotificationTitle = string.Format(ApiConstants.NewRegulationGroupBySuperAdminNotificationTitleTemplate, regulationGroupName);
                        notification.NotificationMessage = string.Format(ApiConstants.NewRegulationGroupBySuperAdminNotificationMessageTemplate, regulationGroupName);
                        notification.Status = Models.Enums.RefApprovalStatus.Approved;
                    }
                    else
                    {
                        notification.NotificationTitle = string.Format(ApiConstants.NewRegulationGroupNotificationTitleTemplate, createdByUserDetails.UserName, regulationGroupName);
                        notification.NotificationMessage = string.Format(ApiConstants.NewRegulationGroupNotificationMessageTemplate, createdByUserDetails.UserName, regulationGroupName);
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

        public async Task<bool> AddNewRegulationGroupMappingApprovalNotification(int createdBy, long? regulationGroupId)
        {
            try
            {
                var createdByUserDetails = await _helperRepository.GetUserAndManagerInfoAsync(createdBy);
                var regulationGroupDetails = await _dbContext.RegulationGroups.FindAsync(regulationGroupId);
                if (createdByUserDetails != null && regulationGroupDetails != null)
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
                        ModuleType = Models.Enums.ModuleType.RegulationGroup
                    };
                    if (createdByUserDetails.UserRoleId == 1)
                    {
                        notification.NotificationTitle = string.Format(ApiConstants.NewRegulationGroupByMappingSuperAdminNotificationTitleTemplate, regulationGroupDetails.RegulationGroupName);
                        notification.NotificationMessage = string.Format(ApiConstants.NewRegulationGroupByMappingSuperAdminNotificationMessageTemplate, regulationGroupDetails.RegulationGroupName);
                        notification.Status = Models.Enums.RefApprovalStatus.Approved;
                    }
                    else
                    {
                        notification.NotificationTitle = string.Format(ApiConstants.NewRegulationGroupMappingNotificationTitleTemplate, createdByUserDetails.UserName, regulationGroupDetails.RegulationGroupName);
                        notification.NotificationMessage = string.Format(ApiConstants.NewRegulationGroupMappingNotificationMessageTemplate, createdByUserDetails.UserName, regulationGroupDetails.RegulationGroupName);
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

        private async Task<bool> AddRegulationGroupResponseNotification(AccessModel access, string approvalStatus, Models.Enums.ModuleType moduleType)
        {
            try
            {
                var regulationGroupInfo = await _dbContext.RegulationGroups.FindAsync(access.RegulationGroupId);
                var userInfo = await _helperRepository.GetUserAndManagerInfoAsync(regulationGroupInfo.CreatedBy.Value);
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
                    MarkAsRead = true,
                    ModuleType = moduleType,
                    NotificationTitle = GetRegulationGroupApprovalNotificationTitle(moduleType, approvalStatus, userInfo.ManagerName!),
                    NotificationMessage = GetRegulationGroupApprovalNotificationMessage(moduleType, approvalStatus),
                    Status = approvalStatus == Models.Enums.RefApprovalStatus.Approved.ToString() ? Models.Enums.RefApprovalStatus.Approved : Models.Enums.RefApprovalStatus.Rejected
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

        private static string GetRegulationGroupApprovalNotificationMessage(Models.Enums.ModuleType moduleType, string approvalStatus)
        {
            if (moduleType == Models.Enums.ModuleType.RegulationGroup)
            {
                if (approvalStatus == Models.Enums.RefApprovalStatus.Approved.ToString())
                    return $"Regulation Group Approved";
                if (approvalStatus == Models.Enums.RefApprovalStatus.Rejected.ToString())
                    return $"Regulation Group Rejected";
                if (approvalStatus == Models.Enums.RefApprovalStatus.Reviewed.ToString())
                    return $"Regulation Group Reviewed";
                if (approvalStatus == Models.Enums.RefApprovalStatus.Forward.ToString())
                    return $"Regulation Group Forwarded";
                if (approvalStatus == Models.Enums.RefApprovalStatus.Pending.ToString())
                    return $"Regulation Group Pending";
            }
            if (moduleType == Models.Enums.ModuleType.RegulationGroupMapping)
            {
                if (approvalStatus == Models.Enums.RefApprovalStatus.Approved.ToString())
                    return $"Regulation Group Mapping Approved";
                if (approvalStatus == Models.Enums.RefApprovalStatus.Rejected.ToString())
                    return $"Regulation Group Mapping Rejected";
                if (approvalStatus == Models.Enums.RefApprovalStatus.Reviewed.ToString())
                    return $"Regulation Group Mapping Reviewed";
                if (approvalStatus == Models.Enums.RefApprovalStatus.Forward.ToString())
                    return $"Regulation Group Mapping Forwarded";
                if (approvalStatus == Models.Enums.RefApprovalStatus.Pending.ToString())
                    return $"Regulation Group Mapping Pending";
            }
            return $"{approvalStatus} status updated";
        }

        private static string GetRegulationGroupApprovalNotificationTitle(Models.Enums.ModuleType moduleType, string approvalStatus, string managerName)
        {
            if (moduleType == Models.Enums.ModuleType.RegulationGroup)
            {
                if (approvalStatus == Models.Enums.RefApprovalStatus.Approved.ToString())
                    return $"Your regulation group request has been approved by {managerName}.";
                if (approvalStatus == Models.Enums.RefApprovalStatus.Rejected.ToString())
                    return $"Your regulation group request has been rejected by {managerName}.";
                if (approvalStatus == Models.Enums.RefApprovalStatus.Reviewed.ToString())
                    return $"Your regulation group  request has been reviewed by {managerName}.";
                if (approvalStatus == Models.Enums.RefApprovalStatus.Forward.ToString())
                    return $"Your regulation group request has been forwarded by {managerName}.";
                if (approvalStatus == Models.Enums.RefApprovalStatus.Pending.ToString())
                    return $"Your regulation group request is pending with {managerName}.";
            }
            if (moduleType == Models.Enums.ModuleType.RegulationGroupMapping)
            {
                if (approvalStatus == Models.Enums.RefApprovalStatus.Approved.ToString())
                    return $"Your regulation group mapping request has been approved by {managerName}.";
                if (approvalStatus == Models.Enums.RefApprovalStatus.Rejected.ToString())
                    return $"Your regulation group mapping  request has been rejected by {managerName}.";
                if (approvalStatus == Models.Enums.RefApprovalStatus.Reviewed.ToString())
                    return $"Your regulation group mapping  request has been reviewed by {managerName}.";
                if (approvalStatus == Models.Enums.RefApprovalStatus.Forward.ToString())
                    return $"Your regulation group mapping request has been forwarded by {managerName}.";
                if (approvalStatus == Models.Enums.RefApprovalStatus.Pending.ToString())
                    return $"Your regulation group mapping  request is pending with {managerName}.";
            }
            return $"Your request for has been {approvalStatus.ToLower()} by {managerName}.";
        }

        public async Task<string> GetNextRegulationGroupCode()
        {
            using (var connection = _unitOfWork.ConnectionFactory())
            {
                var count = await connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM [product_owner].[RegulationGroup]");
                int nextNumber = count + 1;
                return $"{nextNumber:D3}";
            }
        }
    }
}