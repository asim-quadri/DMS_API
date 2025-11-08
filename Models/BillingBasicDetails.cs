namespace ComplianceAPI.Models
{
    public class BillingBasicDetails
    {
        public long Id { get; set; }
        public long OrganizationId { get; set; }
        public string? OrganizationName { get; set; }
        public long? EntityId { get; set; }
        public string? EntityName { get; set; }
        public string? BillNumber { get; set; }
        public DateTime? BillDate { get; set; }
        public int? ReceivedAmount { get; set; }
        public int? BillStatus { get; set; }
        public string? BillStatusName { get; set; }
        public DateTime? DueDate { get; set; }
        public long? CreatedById { get; set; }
        public string? CreatedByName { get; set; }
        public long? RoleId { get; set; }
        public string? RoleName { get; set; }
        public string? ColorName { get; set; } // "1", "2", "3", "Red", "Amber", "Green".
        public int? ColorCode { get; set; } // "1", "2", "3", "Red", "Amber", "Green".
    }
}
