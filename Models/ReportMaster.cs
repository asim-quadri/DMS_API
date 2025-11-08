namespace ComplianceAPI.Models
{
    public class ReportMaster
    {
        public long? Id { get; set; } 
        public string ReportSequence { get; set; } = null!;

        public string ReportDescription { get; set; } = null!;

        public bool IsActive { get; set; }
    }
}
