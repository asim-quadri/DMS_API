using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComplianceAPI.Models.DataModels
{
    public class RefRolesHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HistoryId { get; set; }

        public long? Id { get; set; }

        [StringLength(50)]
        public string RoleName { get; set; }

        [StringLength(50)]
        public string RoleDisplayName { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        public byte? Status { get; set; }

        public DateTime? CreatedOn { get; set; }

        public long? CreatedBy { get; set; }

        public long? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public Guid? UID { get; set; }
    }
}
