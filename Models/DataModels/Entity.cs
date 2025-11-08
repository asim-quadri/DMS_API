namespace ComplianceAPI.Models.DataModels
{
    public class Entity
    {
        public long Id { get; set; }
        public string EntityName { get; set; }
        public long? OrganizationId { get; set; }
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
        public int? MajorIndustryId { get; set; }
        public int? MinorIndustryId { get; set; }
        public int? FinancialYearStart { get; set; }
        public int? FinancialYearEnd { get; set; }
        public string? FromMonth { get; set; }
        public string? ToMonth { get; set; }
    }
}
