namespace ComplianceAPI.Models.DataModels
{
    public class RegulationGroupMapping
    {
        public long? Id { get; set; }
        public int? CountryId { get; set; }
        public int? RegulationGroupId { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? UID { get; set; }
        public string? RegulationGroupReferenceCode { get; set; }

    }
}
