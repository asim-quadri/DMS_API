using ComplianceAPI.Models.DataModels;
using Microsoft.EntityFrameworkCore;

namespace ComplianceAPI.Repository
{
    public interface INotificationRepository
    {
        Task<bool> AddNotification(Notification notification);

        Task<bool> IsUserExists(Guid? userId);

        Task<List<Notification>> GetAllNotifications(long userId);

        Task<bool> MarkNotificationAsRead(Guid? notificationId, bool markAsRead);
    }

    public class NotificationRepository : INotificationRepository
    {
        private readonly ComplianceDbContext _dbContext;
        private readonly ILogger<NotificationRepository> _logger;

        public NotificationRepository(ComplianceDbContext dbContext, ILogger<NotificationRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<List<Notification>> GetAllNotifications(long userId)
        {
            try
            {
                var user = await _dbContext.Users.FindAsync(userId);
                if (user != null)
                {
                    var allNotifications = await _dbContext.Notifications
                    .Where(x => x.RecipientUserId == userId)
                    .OrderByDescending(x => x.CreatedDate)
                    .ToListAsync();
                    if (allNotifications != null)
                    {
                        return allNotifications;
                    }
                    return null;
                }
                else
                {
                    _logger.LogWarning("User not found with id: {userId}", userId);
                    return null;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while retrieving the notifications.");
                return new List<Notification>();
            }
        }

        public async Task<bool> AddNotification(Notification notification)
        {
            try
            {
                await _dbContext.Notifications.AddAsync(notification);
                await _dbContext.SaveChangesAsync();
                return true;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding notification");
                return false;
            }
        }

        public async Task<bool> IsUserExists(Guid? userId)
        {
            var user = _dbContext.Users.Where(x => x.UID == userId);
            if (user != null)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> MarkNotificationAsRead(Guid? notificationId, bool markAsRead)
        {
            try
            {
                var notification = await _dbContext.Notifications.FindAsync(notificationId.ToString());
                if (notification != null)
                {
                    notification.Status = Models.Enums.RefApprovalStatus.Approved;
                    notification.ReadDate = DateTime.UtcNow;
                    notification.MarkAsRead = true;
                    _dbContext.Notifications.Update(notification);
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                return false;
            }
        }
    }
}