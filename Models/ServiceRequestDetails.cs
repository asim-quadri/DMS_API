namespace ComplianceAPI.Models
{
    public class ServiceRequestDetails
    {
        public long Id { get; set; }
        public string Subject { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int? Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ExpectedDate { get; set; }
        public string? EntityName { get; set; }
        public string? OrganizationName { get; set; }
        public string? UserName { get; set; }
        public string? Comments { get; set; }
        public string? StatusDisplay { get; set; } // "1", "2", "3", "Red", "Amber", "Green".
    }
}
