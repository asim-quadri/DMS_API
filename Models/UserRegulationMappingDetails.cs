namespace ComplianceAPI.Models
{
    public class UserRegulationMappingDetails
    {
        public long UserId { get; set; }
        public List<IdNamePair> Countries { get; set; } = new();
        public List<IdNamePair> States { get; set; } = new();
        public List<IdNamePair> Regulations { get; set; } = new();
        public List<IdNamePair> RegulationGroupName { get; set; }
        public List<IdNamePair> ComplianceType { get; set; }
        public List<IdNamePair> ComplianceId { get; set; }
    }

    public class IdNamePair
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}
