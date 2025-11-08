namespace ComplianceAPI.Models
{
    public class TypeOfCompliance
    {
        public int Id { get; set; }

        public int? RegulationSetupId { get; set; }

        public int? ComplianceId { get; set; }

        public string? TypeOfComplianceName { get; set; }
    }
}