using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComplianceAPI.Models.DataModels
{
    [Table("ConcernedMinistry", Schema = "product_owner")]
    public class ConcernedMinistry
    {
        [Key]
        public long Id { get; set; }

        public string ConcernedMinistryCode { get; set; } = null!;

        public string ConcernedMinistryName { get; set; } = null!;

        public string? ConcernedMinistryReferenceCode { get; set; }

        public long CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public long? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public int Status { get; set; } = 0;

        public string UID { get; set; } = Guid.NewGuid().ToString();
    }
}