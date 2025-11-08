namespace ComplianceAPI.Models
{
    public class CountryConcernedMinistryMapping
    {
        public long? Id { get; set; }

        public int CountryId { get; set; }

        public int ConcernedMinistryId { get; set; }

        public int Status { get; set; } = 0;

        public DateTime CreatedOn { get; set; }

        public long? ManagerId { get; set; }

        public long CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public long? ModifiedBy { get; set; }

        public string UID { get; set; } = Guid.NewGuid().ToString();

        public string? CountryConcernedMinistryReferenceCode { get; set; }

        public string? UserName { get; set; }

        public string? ApproveStatus { get; set; }

        public string? ManagerName { get; set; }

        public string? CountryName { get; set; }

        public string? ConcernedMinistryName { get; set; }
    }
}