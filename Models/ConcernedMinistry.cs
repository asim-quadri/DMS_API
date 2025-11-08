namespace ComplianceAPI.Models
{
    public class ConcernedMinistry
    {
        public long? Id { get; set; }

        public string ConcernedMinistryCode { get; set; } = null!;

        public string ConcernedMinistryName { get; set; } = null!;

        public string? ConcernedMinistryReferenceCode { get; set; }

        public long CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public int Status { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string? UID { get; set; }
    }
}