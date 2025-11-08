using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComplianceAPI.Models.DataModels
{
    [Table("RegulatoryAuthorities", Schema = "product_owner")]
    public class RegulatoryAuthorities
    {
        [Key]
        public long Id { get; set; }

        public string? RegulatoryAuthorityCode { get; set; }

        public string RegulatoryAuthorityName { get; set; } = null!;

        public string? RegulatoryAuthorityReferenceCode { get; set; }

        public long CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public long? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public int Status { get; set; } = 0;

        public string UID { get; set; } = Guid.NewGuid().ToString();
    }
}