using System.ComponentModel.DataAnnotations;

namespace ComplianceAPI.Models
{
    public class ServiceRequestHistory
    {
        public long? HistoryId { get; set; }

        [Required(ErrorMessage = "MajorModuleId is required.")]
        public long MajorModuleId { get; set; }

        [Required(ErrorMessage = "MinorModuleId is required.")]
        public long MinorModuleId { get; set; }

        [Required(ErrorMessage = "LevelMasterId is required.")]
        public long LevelMasterId { get; set; }

        [Required(ErrorMessage = "EntityId is required.")]
        public long EntityId { get; set; }

        [Required(ErrorMessage = "UserId is required.")]
        public long UserId { get; set; }

        [Required(ErrorMessage = "OrganizationId is required.")]
        public long OrganizationId { get; set; }

        [Required(ErrorMessage = "Subject is required.")]
        [StringLength(200, ErrorMessage = "Subject cannot exceed 200 characters.")]
        public string Subject { get; set; } = null!;

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(20000, ErrorMessage = "Description cannot exceed 2000 characters.")]
        public string Description { get; set; } = null!;

        public long? ManagerId { get; set; } = 0;

        public int? Status { get; set; } = 0;

        [Required(ErrorMessage = "CreatedOn is required.")]
        public DateTime CreatedOn { get; set; }

        public long? CreatedBy { get; set; }
    }
}