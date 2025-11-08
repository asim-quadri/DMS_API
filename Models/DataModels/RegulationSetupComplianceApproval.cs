using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComplianceAPI.Models.DataModels
{
    [Table("RegulationSetupComplianceApproval", Schema = "product_owner")]
    public class RegulationSetupComplianceApproval
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public long HistoryId { get; set; }

        [Required]
        public long? ManagerId { get; set; }

        [Required]
        public int ApprovalStatus { get; set; }

        public DateTime? CreatedOn { get; set; }

        public long? CreatedBy { get; set; }

        public long? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public Guid? UID { get; set; }
    }
}
