using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComplianceAPI.Models.DataModels
{
    [Table("ServiceRequest", Schema = "client")]
    public class ServiceRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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

        //[NotMapped]
        //public string? ApprovedStatus { get; set; }

        public DateTime CreatedOn { get; set; }

        public long? CreatedBy { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime? ExpectedDate { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public long? ModifiedBy { get; set; }

        public string? Comments { get; set; }
    }
}