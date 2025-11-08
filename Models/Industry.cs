namespace ComplianceAPI.Models
{
    public class MajorIndustry : Response
    {
        public int? Id { get; set; }

        public string? MajorIndustryName { get; set; }

        public string? MajorIndustryCode { get; set; }

        public int? Status { get; set; }

        public int? ManagerId { get; set; }

        public DateTime CreatedOn { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public long? ModifiedBy { get; set; }

        public Guid? UID { get; }

        public Guid? ApprovalUID { get; }

        public string? MajorIndustryReferenceCode { get; set; }
    }

    public class MinorIndustry : Response
    {
        public long Id { get; set; }

        public long? MajorIndustryId { get; set; }

        public string? MinorIndustryName { get; set; }

        public string? MinorIndustryCode { get; set; }

        public int? Status { get; set; }

        public int? ManagerId { get; set; }

        public DateTime CreatedOn { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public long? ModifiedBy { get; set; }

        public Guid? UID { get; }

        public Guid? ApprovalUID { get; }

        public string? MinorIndustryReferenceCode { get; set; }
    }

    public class CountryMajorMapping : Response
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

    public class MajorMinorMapping : Response
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

    public class IndustryrMapping : Response
    {
        public int? Id { get; set; }

        public long? CountryId { get; set; }

        public long? TOBId { get; set; }

        public string? TOBName { get; set; }

        public string? CountryName { get; set; }

        public string? CountryCode { get; set; }

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

        public long? ModifiedBy { get; set; }

        public Guid? UID { get; }

        public string? MajorIndustryReferenceCode { get; set; }

        public string? MinorIndustryReferenceCode { get; set; }

        public DateTime? InactivatedDate { get; set; }

        public string? CreatedByName { get; set; }

        public string? ApprovedByName { get; set; }
    }

    public class IndustryApproval
    {
        public long? Id { get; set; }

        public int? CountryId { get; set; }

        public long? IndustryMappingId { get; set; }

        public string? CountryName { get; set; }

        public string? CountryCode { get; set; }

        public string? MajorIndustryName { get; set; }

        public string? MajorIndustryCode { get; set; }

        public int? MajorIndustryId { get; set; }

        public long? MinorIndustryId { get; set; }

        public string? MinorIndustryName { get; set; }

        public string? MinorIndustryCode { get; set; }

        public int? StatusId { get; set; }

        public string? Status { get; set; }

        public string? Type { get; set; }

        public int? CreatedBy { get; set; }

        public Guid? UID { get; set; }

        public Guid? CountryUID { get; set; }
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

        public string? ApprovedBy { get; set; }

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

        public string? ApprovedBy { get; set; }

        public int? StatusId { get; set; }

        public string? Status { get; set; }

        public string? Type { get; set; }

        public DateTime CreatedOn { get; set; }

        public int? CreatedBy { get; set; }

        public Guid? UID { get; set; }
    }
}