namespace ComplianceAPI.Models.DataModels
{
    public class RegulationGrouping
    {
        public long? Id { get; set; }
        public string? RegulationGroupName { get; set; }
        public string? RegulationGroupCode { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? UID { get; set; }
    }
}
