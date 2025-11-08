namespace ComplianceAPI.Models
{
    public class DeliveryStatus
    {
        public int Id { get; set; }
        public Guid UID { get; set; }
        public string? DeliveryStatusName { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool IsActive { get; set; }
    }
}