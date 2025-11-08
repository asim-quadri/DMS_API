namespace ComplianceAPI.Models.DataModels
{
    public class RefApprovalType
    {
        public int Id { get; set; }
        public string? ApprovalType { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? UID { get; set; }
    }
}
