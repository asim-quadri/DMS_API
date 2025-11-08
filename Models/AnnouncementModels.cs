namespace ComplianceAPI.Models
{
    public class Announcement: Response
    {
        public long? Id { get; set; }
        public long? RegulationId { get; set; }
        public string? RegulationName { get; set; }
        public long? ComplianceId { get; set; }

        public long? RegisterId { get; set; }
        public long? TOCId { get; set; }
        public string? createdByName { get; set; }
        public string? CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? ApplicableDate { get; set; }
        public string? Description { get; set; }
        public string Subject { get; set; }
        public int? ApprovalManagerId { get; set; }
        public long? HistoryId { get; set; }
        public string? AnnouncementReferencecode { get; set; }
        public int? Status { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
    public class AnnouncementApprovalModal
    {
        public long? Id { get; set; }
        public long? RegulationId { get; set; }
        public long? ComplainceId { get; set; }
        public long? TOCId { get; set; }
        public string? createdByName { get; set; }
        public string? CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? ApplicableDate { get; set; }
        public string? Description { get; set; }
        public int? ApprovalManagerId { get; set; }
        public long? HistoryId { get; set; }
        public string? Status { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? DisplayName { get; set; }
        public string? FullName { get; set; }
        public string? approvedBy { get; set; }
        public Guid? ApproverUID { get; set; }
        public string? RegulationType { get; set; }
        public string? ApprovalType { get; set; }
        public int? ApprovalTypeId { get; set; }
        public long? ApproverManager { get; set; }
        public int? ApprovalStatus { get; set; }

        public long? RegisterId { get; set; }
    }

   

}
