using ComplianceAPI.Models;
using ComplianceAPI.Repository;

namespace ComplianceAPI.Services
{
    public interface INotificationService
    {
        Task<NotificationSummary> GetAllApprovalNotifications(long userId);

        Task<bool> MarkNotificationAsRead(Guid? UserUID, Guid? notificationId, bool markAsRead);
    }

    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(INotificationRepository notificationRepositoy)
        {
            _notificationRepository = notificationRepositoy;
        }

        public async Task<NotificationSummary> GetAllApprovalNotifications(long userId)
        {
            var result = await _notificationRepository.GetAllNotifications(userId);

            if (result != null)
            {
                var mapped = result.Select(n => new ApprovalNotifications
                {
                    NotificationId = n.NotificationId,
                    NotificationTitle = n.NotificationTitle,
                    NotificationMessage = n.NotificationMessage,
                    SenderUserId = n.SenderUserId,
                    SenderUserName = n.SenderUserName,
                    RecipientUserId = n.RecipientUserId,
                    RecipientUserName = n.RecipientUserName,
                    CreatedDate = n.CreatedDate,
                    ReadDate = n.ReadDate,
                    Status = n.Status,
                    ModuleType = n.ModuleType,
                    MarkAsRead=n.MarkAsRead
                }).ToList();

                var total = mapped.Count;
                var unread = mapped.Count(x=>x.MarkAsRead==false);

                return new NotificationSummary
                {
                    TotalCount = total,
                    UnreadCount = unread,
                    Notifications = mapped
                };
            }
            return new NotificationSummary
            {
                TotalCount = 0,
                UnreadCount = 0,
                Notifications = new List<ApprovalNotifications>()
            };
        }

        public async Task<bool> MarkNotificationAsRead(Guid? UserUID, Guid? notificationId, bool markAsRead)
        {
            if (!markAsRead)
            {
                return false;
            }
            if (await _notificationRepository.IsUserExists(UserUID))
            {
                var result = await _notificationRepository.MarkNotificationAsRead(notificationId, markAsRead);
                if (result)
                {
                    return true;
                }
                return false;
            }
            return false;
        }
    }
}