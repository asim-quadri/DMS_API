namespace ComplianceAPI.Models
{
    public class RegulationSetupLegalEntityTypeDto
    {
        public long Id { get; set; }

        public long? RegulationSetupId { get; set; }

        public long? ComplianceId { get; set; }

        public long? LegalEntityType { get; set; }

        public string? EntityName { get; set; }

        public DateTime? CreatedOn { get; set; }

        public long? CreatedBy { get; set; }

        public long? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public Guid UID { get; set; }
    }
}