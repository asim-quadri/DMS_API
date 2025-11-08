namespace ComplianceAPI.Models
{
    public class MajorModule
    {
        public long? Id { get; set; }

        public string MajorModuleName { get; set; } = null!;

        public bool IsActive { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}