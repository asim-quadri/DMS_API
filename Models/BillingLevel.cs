namespace ComplianceAPI.Models
{
    public class BillingLevel
    {
        public int Id { get; set; }
        public Guid UUID { get; set; }
        public string BillingLevelName { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool IsActive { get; set; }
    }
}