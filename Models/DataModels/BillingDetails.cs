namespace ComplianceAPI.Models.DataModels
{
    public class BillingDetails
    {
        public long Id { get; set; }
        public long OrganizationId { get; set; }
        public long? EntityId { get; set; }
        public int Status { get; set; }
        public string? PAN { get; set; }
        public string? TAN { get; set; }
        public string? GSTNo { get; set; }
        public string? Address { get; set; }
        public string? PIN { get; set; }
        public string? City { get; set; }
        public int? CountryId { get; set; }
        public long? StateId { get; set; }
        public string? OrderId { get; set; }
        public int? ServiceProvider { get; set; }
        public int? FeePerEntity { get; set; }
        public int? FeePerUser { get; set; }
        public string? BillNumber { get; set; }
        public int? BillingFrequency { get; set; }
        public DateTime? BillDate { get; set; }
        public string? Remarks { get; set; }
        public DateTime? CollectionDate { get; set; }
        public int? BillAmount { get; set; }
        public decimal? TDS { get; set; }
        public int? ReceivedAmount { get; set; }
        public string? CheckNumber { get; set; }
        public int? BillStatus { get; set; }
        public int? DeliveryStatus { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public int? TotalFee { get; set; }
        public string? PaymentTerm { get; set; }
        public DateTime? DueDate { get; set; }
    }
    public class PostBillingDetails
    {
        public long Id { get; set; }
        public long OrganizationId { get; set; }
        public long? EntityId { get; set; }
        public string? PAN { get; set; }
        public string? TAN { get; set; }
        public string? GSTNo { get; set; }
        public string? Address { get; set; }
        public string? PIN { get; set; }
        public string? City { get; set; }
        public int? CountryId { get; set; }
        public int? StateId { get; set; }
        public string? OrderId { get; set; }
        public int? ServiceProvider { get; set; }
        public int? FeePerEntity { get; set; }
        public int? FeePerUser { get; set; }
        public string? BillNumber { get; set; }
        public int? BillingFrequency { get; set; }
        public DateTime? BillDate { get; set; }
        public string? Remarks { get; set; }
        public DateTime? CollectionDate { get; set; }
        public int? BillAmount { get; set; }
        public decimal? TDS { get; set; }
        public int? ReceivedAmount { get; set; }
        public string? CheckNumber { get; set; }
        public int? BillStatus { get; set; }
        public int? DeliveryStatus { get; set; }
        public long? CreatedBy { get; set; }
        public int? TotalFee { get; set; }
        public string? PaymentTerm { get; set; }
        public DateTime? DueDate { get; set; }

    }
    public class BillingDetailsView
    {
        public long Id { get; set; }
        public long OrganizationId { get; set; }
        public long? EntityId { get; set; }
        public int Status { get; set; }
        public string? PAN { get; set; }
        public string? TAN { get; set; }
        public string? GSTNo { get; set; }
        public string? Address { get; set; }
        public string? PIN { get; set; }
        public string? City { get; set; }
        public int? CountryId { get; set; }
        public long? StateId { get; set; }
        public string? OrderId { get; set; }
        public int? ServiceProvider { get; set; }
        public int? FeePerEntity { get; set; }
        public int? FeePerUser { get; set; }
        public string? BillNumber { get; set; }
        public int? BillingFrequency { get; set; }
        public DateTime? BillDate { get; set; }
        public string? Remarks { get; set; }
        public DateTime? CollectionDate { get; set; }
        public int? BillAmount { get; set; }
        public decimal? TDS { get; set; }
        public int? ReceivedAmount { get; set; }
        public string? CheckNumber { get; set; }
        public int? BillStatus { get; set; }
        public int? DeliveryStatus { get; set; }
        public int? TotalFee { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public string? OrganizationName { get; set; }
        public string? EntityName { get; set; }
        public string? CountryName { get; set; }
        public string? StateName { get; set; }
        public string? ServiceProviderName { get; set; }
        public string? Frequency { get; set; }
        public string? BillDateString { get; set; }
        public string? CollectionDateString { get; set; }
        public string? BillStatusName { get; set; }
        public string? DeliveryStatusName { get; set; }
        public string? TypeOfProduct { get; set; }
        public string? BillingLevelName { get; set; }
        public string? PaymentTerm { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
