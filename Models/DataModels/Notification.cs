using ComplianceAPI.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Enum = ComplianceAPI.Models.Enums;

namespace ComplianceAPI.Models.DataModels
{
    /// <summary>
    /// This class is used to store notification
    /// </summary>
    [Index(nameof(SenderUserId))]
    [Index(nameof(RecipientUserId))]
    [Index(nameof(CreatedDate))]

    [Table("Notifications", Schema = "dbo")]
    public class Notification
    {
        [Key]
        public string NotificationId { get; set; } = Guid.NewGuid().ToString();

        public string? NotificationTitle { get; set; }

        public string? NotificationMessage { get; set; }

        [Required]
        public long SenderUserId { get; set; }

        public string? SenderUserName { get; set; }

        [Required]
        public long RecipientUserId { get; set; }

        public string? RecipientUserName { get; set; }

        [Required]
        public ModuleType ModuleType { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        public DateTime? ReadDate { get; set; }

        public Enum.RefApprovalStatus Status { get; set; } = Enum.RefApprovalStatus.Pending;
        public bool MarkAsRead { get; set; }
    }
}