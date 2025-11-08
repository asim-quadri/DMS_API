using System.ComponentModel.DataAnnotations;

namespace ComplianceAPI.Models
{
    public class CountryRegulatoryAuthorityMapping
    {
        public long? Id { get; set; }

        public int CountryId { get; set; }

        public int RegulatoryAuthorityId { get; set; }

        public int Status { get; set; } = 0;

        public DateTime CreatedOn { get; set; }

        public long? ManagerId { get; set; }

        public long CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public long? ModifiedBy { get; set; }

        public string UID { get; set; }

        public string? CountryRegAuthReferenceCode { get; set; }

        public string? UserName { get; set; }
        public string? ApproveStatus { get; set; }
        public string? ManagerName { get; set; }

        public string? CountryName { get; set; }

        public string? RegAuthName { get; set; }
    }
}