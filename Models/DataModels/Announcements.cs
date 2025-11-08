namespace ComplianceAPI.Models.DataModels
{
    public class Announcements
    {
        public long? Id { get; set; }
        public long? RegulationId { get; set; }
        public long? ComplainceId { get; set; }
        public long? TOCId { get; set; }

        public long? RegisterationId { get; set; }
        public string? CreatedDate { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? ApplicableDate { get; set; }
        public string? Description { get; set; }
        public bool? Status { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? CreatedByName { get; set; }
        public Guid? UID { get; set; }

        public string? AnnouncementReferencecode { get; set; }
        public string? Subject { get; set; }
    }
}
