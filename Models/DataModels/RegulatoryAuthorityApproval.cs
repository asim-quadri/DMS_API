using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComplianceAPI.Models.DataModels
{
    [Table("RegulatoryAuthorityApproval", Schema = "product_owner")]
    public class RegulatoryAuthorityApproval
    {
        [Key]
        public long Id { get; set; }

        public long RegulatoryAuthorityId { get; set; }

        public long ManagerId { get; set; }

        public int ApprovalStatus { get; set; }

        public DateTime CreatedOn { get; set; }

        public long CreatedBy { get; set; }

        public long? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public Guid? UID { get; set; }
    }
}