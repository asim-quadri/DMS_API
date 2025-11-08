namespace ComplianceAPI.Models
{
    public class PostUserRegulationMapping
    {
        public long UserId { get; set; }
        public string CountryIds { get; set; } = string.Empty;
        public string StateIds { get; set; } = string.Empty;
        public string RegulationIds { get; set; } = string.Empty;
        public string RegulationGroupId { get; set; } = string.Empty;
        public string? ComplianceType { get; set; }
        public string? ComplianceId { get; set; }
    }
}
