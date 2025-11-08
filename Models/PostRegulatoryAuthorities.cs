using System.ComponentModel.DataAnnotations;

namespace ComplianceAPI.Models
{
    public class PostRegulatoryAuthorities
    {
        public long? Id { get; set; }

        [Required(ErrorMessage = "Regulatory Authority Code is required")]
        public string? RegulatoryAuthorityCode { get; set; }

        public string RegulatoryAuthorityName { get; set; } = null!;

        public string? RegulatoryAuthorityReferenceCode { get; set; }

        public long ManagerId { get; set; }

        public long CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public int Status { get; set; }

        public long? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string? UID { get; set; }

        public string? UserName { get; set; }

        public string? ManagerName { get; set; }

        public string? ApproveStatus { get; set; }
    }

    public class RegulatoryAuthorities
    {
        public long? Id { get; set; }

        public string? RegulatoryAuthorityCode { get; set; }

        public string RegulatoryAuthorityName { get; set; } = null!;

        public string? RegulatoryAuthorityReferenceCode { get; set; }

        public long CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public int Status { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string? UID { get; set; }
    }
}