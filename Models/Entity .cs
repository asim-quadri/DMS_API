namespace ComplianceAPI.Models
{
    public class Entity : Response
    {
        public long Id { get; set; }
        public string EntityName { get; set; }
        public long? OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public int? MajorIndustryId { get; set; }
        public int? MinorIndustryId { get; set; }
        public string MajorIndustry { get; set; }
        public string MinorIndustry { get; set; }
        public string MajorIndustryName { get; set; }
        public string MinorIndustryName { get; set; }
        public string CountryId { get; set; }
        public string CountryName { get; set; }
        public long? StateId { get; set; }
        public string StateName { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Pin { get; set; }

        public int? Status { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public string? UID { get; set;}
        public int? ManagerId { get; set; }
        public int? EntityTypeId { get; set; }
        public string EntityType { get; set; }
        public string CustomerId { get; set; }
        public string PointOfContact { get; set; }
        public string Approvedby { get; set; }
        public string FinancialYearStart { get; set; }
        public string FinancialYearEnd { get; set; }
        public string ToMonth { get; set; }
        public string FromMonth { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool HasBillingDetails { get; set; }
        public bool HasServiceRequests { get; set; }
    }

    public class PostEntity : Response
    {
        public long Id { get; set; }
        public long? OrganizationId { get; set; }
        public string EntityName { get; set; }
        public long? CountryId { get; set; }
        public long? StateId { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Pin { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public long? ModifiedBy { get; set; }
        public Guid? UID { get; set; }
        public long? ManagerId { get; set; }
        public int? EntityTypeId { get; set; }
        public int? MajorIndustry { get; set; }
        public int? MinorIndustry { get; set; }
        public int? FinancialYearStart { get; set; }
        public int? FinancialYearEnd { get; set; }
        public string? FromMonth { get; set; }
        public string? ToMonth { get; set; }
    }

    public class EntityApprovalList : Response
    {
        public long? OrganizationId { get; set; }
        public long? EntityId { get; set; }
        public long? BillingDetilsId { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string CustomerId { get; set; }
        public string PointOfContact { get; set; }
        public string Approvedby { get; set; }
        public string OnboardingStage { get; set; }
        public string Status { get; set; }
        public int? StatusId { get; set; }
        public int? CreatedBy { get; set; }
        public Guid? UID { get; set; }
    }

    public class FinancialMonth
    {
        public string FromMonth { get; set; }
        public string ToMonth { get; set; }
    }
}
