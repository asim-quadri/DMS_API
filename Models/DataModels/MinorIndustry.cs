namespace ComplianceAPI.Models.DataModels
{
    public class MinorIndustry
    {
        public long? Id { get; set; }
        public long? MajorIndustryId { get; set; }
        public string? MinorIndustryName { get; set; }
        public string? MinorIndustryCode { get; set; }
        public int? Status { get; set; }
        public long? ManagerId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public long? ModifiedBy { get; set; }
        public Guid? UID { get; }
        public string? MinorIndustryReferenceCode { get; set; }
    }
}
