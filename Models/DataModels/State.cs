namespace ComplianceAPI.Models.DataModels
{
    public class State
    {
        public long Id { get; set; }
        public long? CountryId { get; set; }
        public string? StateCode { get; set; }
        public string? StateName { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public long? ModifiedBy { get; set; }
        public Guid? UID { get; }
        public string? StateReferenceCode { get; set; }
    }
}
