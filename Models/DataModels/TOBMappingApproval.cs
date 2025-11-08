namespace ComplianceAPI.Models.DataModels
{
    public class TOBMappingApproval
    {
        public long Id { get; set; }
        public long ManagerId { get; set; }
        public long TOBMappingId { get; set; }
        public int ApprovalStatus { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? UID { get; set; }
    }
}
