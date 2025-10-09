namespace DmsApi.Models
{
    public class MajorIndustry
    {
        public int? Id { get; set; }
        public int? CountryId { get; set; }
        public string? MajorIndustryName { get; set; }
        public string? MajorIndustryCode { get; set; }
        public int? Status { get; set; }
        public int? ManagerId { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public Guid? UID { get; }
    }
    public class MinorIndustry
    {
        public int? Id { get; set; }
        public long? MajorIndustryId { get; set; }
        public string? MinorIndustryName { get; set; }
        public string? MinorIndustryCode { get; set; }
        public int? Status { get; set; }
        public int? ManagerId { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public Guid? UID { get; }
    }
    public class CountryMajorMapping
    {
        public int? Id { get; set; }
        public long? CountryId { get; set; }
        public string? CountryName { get; set; }
        public string? CountryCode { get; set; }
        public long? MajorIndustryId { get; set; }
        public string? MajorIndustryName { get; set; }
        public string? MajorIndustryCode { get; set; }
        public int? Status { get; set; }
        public int? ManagerId { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public Guid? UID { get; }
    }
    public class MajorMinorMapping
    {
        public int? Id { get; set; }
        public long? MajorIndustryId { get; set; }
        public string? MajorIndustryName { get; set; }
        public string? MajorIndustryCode { get; set; }
        public long? MinorIndustryId { get; set; }
        public string? MinorIndustryName { get; set; }
        public string? MinorIndustryCode { get; set; }
        public int? Status { get; set; }
        public int? ManagerId { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public Guid? UID { get; }
    }
    public class CountryMajorApproval
    {
        public long? Id { get; set; }
        public int? CountryId { get; set; }
        public string? CountryName { get; set; }
        public string? CountryCode { get; set; }
        public string? MajorIndustryName { get; set; }
        public string? MajorIndustryCode { get; set; }
        public int? MajorIndustryId { get; set; }
        public string? FullName { get; set; }
        public int? StatusId { get; set; }
        public string? Status { get; set; }
        public string? Type { get; set; }
        public Guid? UID { get; set; }
    }
    public class MajorMinorApproval
    {
        public long? Id { get; set; }
        public int? MajorIndustryId { get; set; }
        public string? MajorIndustryName { get; set; }
        public string? MajorIndustryCode { get; set; }
        public string? MinorIndustryName { get; set; }
        public string? MinorIndustryCode { get; set; }
        public int? MinorIndustryId { get; set; }
        public string? FullName { get; set; }
        public int? StatusId { get; set; }
        public string? Status { get; set; }
        public string? Type { get; set; }
        public Guid? UID { get; set; }
    }
}
