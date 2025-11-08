namespace ComplianceAPI.Models.DataModels
{
    public class Organization
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
        public int? EntityTypeId { get; set; }
        public int? MajorIndustryId { get; set; }
        public int? MinorIndustryId { get; set; }
        public int? BillingLevelId { get; set; }
        public string? PrimaryEntity { get; set; }
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
}
