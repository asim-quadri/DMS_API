namespace ComplianceAPI.Models.DataModels
{
    public class TOBMapping
    {
        public long? Id { get; set; }
        public int? TOBId { get; set; }
        public int? CountryId { get; set; }
        public long? MajorIndustryId { get; set; }
        public long? MinorIndustryId { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public long? ModifiedBy { get; set; }
        public Guid? UID { get; }
    }
}
