using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ComplianceAPI.Models.DataModels
{
    public class UserProductApproval
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public long UserId { get; set; }

        [Required]
        public long ManagerId { get; set; }

        [Required]
        public long ProductMappingId { get; set; }

        [Required]
        public int ApprovalStatus { get; set; }

        [Required]
        public int ApprovalType { get; set; }

        public DateTime? CreatedOn { get; set; }

        public long? CreatedBy { get; set; }

        public long? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public Guid? UID { get; set; }
    }
}
