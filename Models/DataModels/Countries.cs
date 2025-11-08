namespace ComplianceAPI.Models.DataModels
{
    public class Countries
    {
        public int? Id { get; set; }

        public string? CountryName { get; set; }

        public string? CountryCode { get; set; }

        public string? CountryCodeNumber { get; set; }

        public DateTime? FinancialStartDate { get; set; }

        public DateTime? FinancialEndDate { get; set; }

        public int? Status { get; set; }

        public int? CurrencyId { get; set; }

        public DateTime? CreatedOn { get; set; }

        public long? CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public long? ModifiedBy { get; set; }

        public Guid? UID { get; }

        public string? CountryReferenceCode { get; set; }

    }
}