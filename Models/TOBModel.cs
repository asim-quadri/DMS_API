namespace ComplianceAPI.Models
{
    public class TOBResponse
    {
        public int ResponseCode { get; set; }
        public string? ResponseMessage { get; set; }
        public object? ResultSet { get; set; }

    }
    public class TOB: TOBResponse
    {
        public long Id { get; set; }
        public long? HistoryId { get; set; }
        public string? EmpId { get; set; }
        public string? TOBName { get; set; }
        public int? Status { get; set; }
        public long? ManagerId { get; set; }
        public string? ManagerName { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public long? ModifiedBy { get; set; }
        public Guid? UID { get; set; }
        public string? RoleDisplayName { get; set; }
        public string? RoleName { get; set; }
        public int? RoleId { get; set; }
        public int? ApprovalManagerId { get; set; }
        public string? TOBReferenceCode { get; set; }

    }
    public class AddTOB
    {
        public long? Id { get; set; }
        public string? EmpId { get; set; }
        public string? TOBName { get; set; }
        public int? ManagerId { get; set; }
        public int? ApprovalManagerId { get; set; }
        public int? CreatedBy { get; set; }
        public Guid? UID { get; set; }
        public int? RoleId { get; set; }
        public byte? Status { get; set; }
    }
    public class TOBApprovalList
    {
        public long? Id { get; set; }
        public int? CountryId { get; set; }
        public long? TOBMappingId { get; set; }
        public string? CountryName { get; set; }
        public string? CountryCode { get; set; }
        public string? MajorIndustryName { get; set; }
        public string? MajorIndustryCode { get; set; }
        public int? MajorIndustryId { get; set; }
        public long? MinorIndustryId { get; set; }
        public string? MinorIndustryName { get; set; }
        public string? MinorIndustryCode { get; set; }
        public string? TOBName { get; set; }
        public string? ApprovedBy { get; set; }
        public int? StatusId { get; set; }
        public string? Status { get; set; }
        public string? Type { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public Guid? UID { get; set; }
    }
    public class TOBMappingList : Response
    {
        public int? Id { get; set; }
        public long? TOBId { get; set; }
        public string? TOBName { get; set; }
        public long? CountryId { get; set; }
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
        public Nullable<DateTime> CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public Nullable<DateTime> ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public Guid? UID { get; }
    }
}
