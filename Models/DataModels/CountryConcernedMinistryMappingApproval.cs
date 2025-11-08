using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComplianceAPI.Models.DataModels
{
    [Table("CountryConcernedMinistryMappingApproval", Schema = "product_owner")]
    public class CountryConcernedMinistryMappingApproval
    {
        [Key]
        public long Id { get; set; }

        public long CountryConcernedMinistryMappingId { get; set; }

        public long ManagerId { get; set; }

        public int ApproveStatus { get; set; } = 0;

        public DateTime CreatedOn { get; set; }

        public long CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public long? ModifiedBy { get; set; }

        public string? UID { get; set; }
    }
}