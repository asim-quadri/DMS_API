namespace ComplianceAPI.Models
{
    public class LevelMaster
    {
        public long? Id { get; set; }

        public string LevelType { get; set; } = null!;

        public bool IsActive { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}