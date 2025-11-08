namespace ComplianceAPI.Models.DataModels
{
    public class MajorIndustry
    {
        public long? Id { get; set; }
        public string? MajorIndustryName { get; set; }
        public string? MajorIndustryCode { get; set; }
        public int? Status { get; set; }
        public long? ManagerId { get; set; }
        public DateTime CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public long? ModifiedBy { get; set; }
        public Guid? UID { get; }
        public string? MajorIndustryReferenceCode { get; set; }

    }
}
