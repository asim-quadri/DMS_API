namespace ComplianceAPI.Models
{
    public class NotificationSummary
    {
        public int TotalCount { get; set; }
        public int UnreadCount { get; set; }
        public List<ApprovalNotifications> Notifications { get; set; }
    }
}