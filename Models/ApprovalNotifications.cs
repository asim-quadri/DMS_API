using ComplianceAPI.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace ComplianceAPI.Models
{
    public class ApprovalNotifications
    {
        public string? NotificationId { get; set; }

        public string? NotificationTitle { get; set; }

        public string? NotificationMessage { get; set; }

        [Required]
        public long SenderUserId { get; set; }

        public string? SenderUserName { get; set; }

        [Required]
        public long RecipientUserId { get; set; }

        public string? RecipientUserName { get; set; }

        public ModuleType ModuleType { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        public DateTime? ReadDate { get; set; }

        public RefApprovalStatus Status { get; set; }
        public bool MarkAsRead { get; set; }
    }
}