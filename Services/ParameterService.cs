using ComplianceAPI.Helpers;
using ComplianceAPI.Helpers.Constants;
using ComplianceAPI.Models;
using ComplianceAPI.Models.DataModels;
using ComplianceAPI.Repository;
using System.Data;
using System.Data.Common;
using System.Security.Cryptography.X509Certificates;
using RefApprovalType = ComplianceAPI.Helpers.RefApprovalType;

namespace ComplianceAPI.Services
{
    public interface IParameterService
    {
        Task<Parameter> GetHistoryParameters(long HistoryId);

        Task<bool> PostParameterReviewAccess(AccessModel access);

        Task<bool> PostParameterApproveAccess(AccessModel access);

        Task<bool> PostParameterRejectAccess(AccessModel access);

        Task<Parameter> AddParameter(AddParameter parameter, Guid? accessUID);

        Task<List<Parameters>> GetAllParameters();

        Task<List<Parameters>> GetParameterById(long id);

        Task<Parameter> DeleteParameter(Guid uid, int status);

        Task<List<PendingApproval>> GetPendingParameterApproval(Guid? UserUID);

        Task<List<PendingApproval>> GetAllParameterApproval();
        Task<string> GetNextParameterCode();

    }

    public class ParameterService : IParameterService
    {
        private readonly IParameterRepository _parameterRepository;
        private readonly IHelperRepository _helperRepository;
        public readonly INotificationRepository _notificationRepository;

        public ParameterService(IParameterRepository parameterRepository, INotificationRepository notificationRepository, IHelperRepository helperRepository)
        {
            _parameterRepository = parameterRepository;
            _helperRepository = helperRepository;
            _notificationRepository = notificationRepository;
        }

        public async Task<Parameter> GetHistoryParameters(long HistoryId)
        {
            return await _parameterRepository.GetHistoryParameters(HistoryId);
        }

        public async Task<bool> PostParameterReviewAccess(AccessModel access)
        {
            var res = await _parameterRepository.PostParameterReviewAccess(access);

            if (res)
            {
                await AddApprovalNotification(access, RefApprovalStatusU.Reviewed);
            }
            return res;
        }

        public async Task<bool> PostParameterApproveAccess(AccessModel access)
        {
            var result = await _parameterRepository.PostParameterApproveAccess(access);
            if (result)
            {
                await AddApprovalNotification(access, RefApprovalStatusU.Approved);
            }
            return result;
        }

        public async Task<bool> PostParameterRejectAccess(AccessModel access)
        {
            var result = await _parameterRepository.PostParameterRejectAccess(access);
            if (result)
            {
                await AddApprovalNotification(access, RefApprovalStatusU.Rejected);
            }
            return result;
        }

        public async Task<Parameter> AddParameter(AddParameter parameter, Guid? accessUID)
        {
            if (_parameterRepository.IsSuperAdmin((int)parameter.CreatedBy))
            {
                await _parameterRepository.AdminAddParameter(parameter);
            }
            var result = await _parameterRepository.AddParameter(parameter);
            if (accessUID == null)
            {
                AccessModel access = new AccessModel() { ApprovalTypeId = RefApprovalType.User, CreatedBy = parameter.CreatedBy, ManagerId = parameter.ApprovalManagerId == null ? 0 : parameter.ApprovalManagerId, Status = 0, UserId = result.Id, HistoryId = result.HistoryId };
                await _parameterRepository.PostUpdateParameterApproval(access);
            }
            await AddNotificationParameter(parameter);
            return result;
        }

        public async Task<List<PendingApproval>> GetAllParameterApproval()
        {
            return await _parameterRepository.GetAllParameterApproval();
        }

        public async Task<List<PendingApproval>> GetPendingParameterApproval(Guid? UserUID)
        {
            return await _parameterRepository.GetPendingParameterApproval(UserUID);
        }

        public async Task<List<Parameters>> GetAllParameters()
        {
            return await _parameterRepository.GetAllParameters();
        }

        public async Task<List<Parameters>> GetParameterById(long id)
        {
            return await _parameterRepository.GetParameterById(id);
        }

        public async Task<Parameter> DeleteParameter(Guid uid, int status)
        {
            return await _parameterRepository.DeleteParameter(uid, status);
        }

        private async Task<bool> AddNotificationParameter(AddParameter addParameter)
        {
            if (addParameter == null || addParameter.CreatedBy == 0)
            {
                return false;
            }
            var userInfo = await _helperRepository.GetUserAndManagerInfoAsync((long)addParameter.CreatedBy!);
            if (userInfo == null)
            {
                return false;
            }
            var notification = new Notification()
            {
                NotificationId = Guid.NewGuid().ToString(),
                NotificationMessage = userInfo.IsSuperAdmin ? string.Format(ApiConstants.AddParameterTitleAdminTemplate, userInfo.UserName, Models.Enums.ModuleType.Parameter) : string.Format(ApiConstants.AddParameterTitleTemplate, userInfo.UserName, Models.Enums.ModuleType.Parameter),
                NotificationTitle = string.Format(ApiConstants.AddParameterMessageTemplate, addParameter.ParameterName, userInfo.UserName),
                RecipientUserName = userInfo.ManagerName,
                RecipientUserId = userInfo.ManagerId,
                ReadDate = null,
                CreatedDate = DateTime.Now,
                MarkAsRead = false,
                SenderUserName = userInfo.UserName,
                SenderUserId = userInfo.UserId,
                ModuleType = Models.Enums.ModuleType.Parameter,
                Status = userInfo.IsSuperAdmin ? Models.Enums.RefApprovalStatus.Approved : Models.Enums.RefApprovalStatus.Pending
            };
            var isNotificatedSaved = await _notificationRepository.AddNotification(notification);
            return isNotificatedSaved;
        }

        private async Task<bool> AddApprovalNotification(AccessModel access, string actionType)
        {
            if (access == null || access.CreatedBy == null || access.CreatedBy == 0)
                return false;
            var parameterDetails = await _parameterRepository.GetHistoryParameters(access.HistoryId.Value);
            var userInfo = await _helperRepository.GetUserAndManagerInfoAsync(parameterDetails.CreatedBy.Value);
            if (userInfo == null)
                return false;

            string notificationTitle = actionType switch
            {
                "Approved" => $"<b>{userInfo.ManagerName}</b> approved a  <b>New Parameter</b>.",
                "Rejected" => $"<b>{userInfo.ManagerName}</b> rejected the <b>New Parameter</b>.",
                "Forward" => $"<b>{userInfo.ManagerName}</b> forwarded the <b>New Parameter</b>.",
                "Reviewed" => $"<b>{userInfo.ManagerName}</b> reviewed the <b>New Parameter</b>.",
                _ => $"<b>{userInfo.ManagerName}</b> took an action on the <b>New Parameter</b>."
            };

            string notificationMessage = $"<b>{actionType}</b> action by <b>{userInfo!.UserName}</b>";

            var status = actionType switch
            {
                "Approve" => Models.Enums.RefApprovalStatus.Approved,
                "Reject" => Models.Enums.RefApprovalStatus.Rejected,
                "Forward" => Models.Enums.RefApprovalStatus.Forward,
                "Review" => Models.Enums.RefApprovalStatus.Reviewed,
                _ => Models.Enums.RefApprovalStatus.Pending
            };

            var notification = new Notification
            {
                NotificationId = Guid.NewGuid().ToString(),
                NotificationMessage = notificationMessage,
                NotificationTitle = notificationTitle,
                RecipientUserName = userInfo?.UserName ?? string.Empty,
                RecipientUserId = userInfo?.UserId ?? 0,
                ReadDate = null,
                CreatedDate = DateTime.Now,
                MarkAsRead = false,
                SenderUserName = userInfo.UserName,
                SenderUserId = userInfo.UserId,
                ModuleType = Models.Enums.ModuleType.Parameter,
                Status = status
            };

            return await _notificationRepository.AddNotification(notification);
        }
        public async Task<string> GetNextParameterCode()
        {
            return await _parameterRepository.GetNextParameterCode();
        }

    }
}