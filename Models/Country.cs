namespace DmsApi.Models
{
    public class Country : Response
    {
        public long Id { get; set; }
        public string? CountryName { get; set; }
        public string? CountryCode { get; set; }
        public int CountryCodeNumber { get; set; }
        public DateTime? FinancialStartDate { get; set; }
        public DateTime? FinancialEndDate { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public Guid? UID { get; }
        public int? ManagerId { get; set; }

    }
    public class States : Response
    {
        public long Id { get; set; }
        public long? CountryId { get; set; }
        public string? StateCode { get; set; }
        public string? StateName { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public Guid? UID { get; }
        public int? ManagerId { get; set; }
    }

    public class CountryStateMapping : Response
    {
        public long? Id { get; set; }
        public long? CountryId { get; set; }
        public long? StateId { get; set; }
        public int? Status { get; set; }
        public Guid? UID { get; set; }
        public string? CountryName { get; set; }
        public string? StateName { get; set; }
        public string? CountryCode { get; set; }
        public string? StateCode { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public int? ManagerId { get; set; }
    }

    public class CountryStateApproval
    {
        public long? Id { get; set; }
        public int? CountryId { get; set; }
        public string? CountryName { get; set; }
        public string? CountryCode { get; set; }
        public DateTime? FinancialStartDate { get; set; }
        public DateTime? FinancialEndDate { get; set; }
        public string? FinancialYear { get; set; }
        public string? StateName { get; set; }
        public string? StateCode { get; set; }
        public int? StateId { get; set; }
        public string? FullName { get; set; }
        public int? StatusId { get; set; }
        public string? Status { get; set; }
        public string? Type { get; set; }
        public int? CreatedBy { get; set; }
        public Guid? UID { get; set; }
    }
}
