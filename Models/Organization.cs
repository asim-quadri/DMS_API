namespace ComplianceAPI.Models
{
    public class Organization : Response
    {
        public long Id { get; set; }
        public long? HistoryId { get; set; }
        public string OrganizationName { get; set; }
        //public string OrganizationCode { get; set; }
        public string CountryId { get; set; }
        public long? CountryDDId { get; set; }
        public long? StateId { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Pin { get; set; }
        public string TypeOfProduct { get; set; }
        public int? NumberOfEntities { get; set; }
        public int? NumberOfUsers { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public long? ModifiedBy { get; set; }
        public Guid? UID { get; set; }
        public long? ManagerId { get; set; }
        public Guid? ApprovalUID { get; }
        public int? EntityTypeId { get; set; }
        public string? PrimaryEntity { get; set; }
        public int? MajorIndustryId { get; set; }
        public int? MinorIndustryId { get; set; }
        public int? BillingLevelId { get; set; }
        public int? FinancialYearStart { get; set; }
        public int? FinancialYearEnd { get; set; }
        public string? FromMonth { get; set; }
        public string? ToMonth { get; set; }
    }

    public class PostOrganization : Response
    {
        public long Id { get; set; }
        public string OrganizationName { get; set; }
        //public string OrganizationCode { get; set; }
        public string CountryId { get; set; }
        public long? CountryDDId { get; set; }
        public long? StateId { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Pin { get; set; }
        public string? TypeOfProduct { get; set; }
        public int? NumberOfEntities { get; set; }
        public int? NumberOfUsers { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public long? ModifiedBy { get; set; }
        public Guid? UID { get; }
        public long? ManagerId { get; set; }
        public Guid? ApprovalUID { get; }
        public int? EntityType { get; set; }
        public string? PrimaryEntity { get; set; }
        public int? MajorIndustry { get; set; }
        public int? MinorIndustry { get; set; }
        public int? BillingLevelId { get; set; }
        public string? FullName { get; set; }
        public string? EmailID { get; set; }
        public string? Designation { get; set; }
        public string? Password { get; set; }
        public int? FinancialYearStart { get; set; }
        public int? FinancialYearEnd { get; set; }
        public string? FromMonth { get; set; }
        public string? ToMonth { get; set; }
        public int? NoofBranches { get; set; }
        public int? RoleId { get; set; }
    }

    public class OrganizationApprovalList : Response
    {
        public long? OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public string FullName { get; set; }
        public long Id { get; set; }
        public int? StatusId { get; set; }
        public string? Status { get; set; }
        public int? CreatedBy { get; set; }
        public string Type { get; set; }
        public Guid? UID { get; }
        public string Approvedby { get; set; }
    }

    public class OrganizationApproval
    {
        public long Id { get; set; }
        public long OrganizationId { get; set; }
        public long ManagerId { get; set; }
        public int ApprovalStatus { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? UID { get; set; }

    }

    public class OrganizationDetail
    {
        public long Id { get; set; }
        public string OrganizationName { get; set; }
        //public string OrganizationCode { get; set; }
        public string CountryId { get; set; }
        public long? CountryDDId { get; set; }
        public long? StateId { get; set; }
        public string? StateName { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Pin { get; set; }
        public string? TypeOfProduct { get; set; }
        public int? NumberOfEntities { get; set; }
        public int? NumberOfUsers { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public long? ModifiedBy { get; set; }
        public Guid? UID { get; }
        public long? ManagerId { get; set; }
        public Guid? ApprovalUID { get; }
        public int? EntityTypeId { get; set; }
        public string? PrimaryEntity { get; set; }
        public int? MajorIndustryId { get; set; }
        public int? MinorIndustryId { get; set; }
        public int? BillingLevelId { get; set; }
        public string? BillingLevel { get; set; }
        public string? FullName { get; set; }
        public string? EmailID { get; set; }
        public string? Designation { get; set; }
        public string? Password { get; set; }
        public int? FinancialYearStart { get; set; }
        public int? FinancialYearEnd { get; set; }
        public string? FromMonth { get; set; }
        public string? ToMonth { get; set; }
        public int? NoofBranches { get; set; }
        public int? RoleId { get; set; }
        public List<Entity> Entities { get; set; }
    }

    public class OrganizationEntityList
    {
        public long? Id { get; set; }
        public string? OrganizationName { get; set; }
        public bool IsOrganization { get; set; }
        public List<EntityList> EntityList { get; set; }
    }

    public class EntityList
    {
        public long? EntityId { get; set; }
        public string? EntityName { get; set; }
    }

}
