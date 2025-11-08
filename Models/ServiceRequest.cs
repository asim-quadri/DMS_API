using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComplianceAPI.Models
{
    [Table("ServiceRequest", Schema = "client")]
    public class ServiceRequest
    {
        public long Id { get; set; }

        public long MajorModuleId { get; set; }

        public long MinorModuleId { get; set; }

        public long LevelMasterId { get; set; }

        public long EntityId { get; set; }

        public long UserId { get; set; }

        public long OrganizationId { get; set; }

        [Required]
        public string Subject { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        public int? Status { get; set; }

        public DateTime CreatedOn { get; set; }

        public long? CreatedBy { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime? ExpectedDate { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public long? ModifiedBy { get; set; }

        public string? EntityName { get; set; }

        public string? OrganizationName { get; set; }

        public string? UserName { get; set; }

        public string? ExpDate { get; set; }

        public string? CreatedDate { get; set; }

        public string? MinorModuleName { get; set; }

        public string? MajorModuleName { get; set; }

        public string? ApprovedStatus { get; set; }

        public string? LevelType { get; set; }

        public string? Comments { get; set; }
    }
}