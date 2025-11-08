using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComplianceAPI.Models.DataModels
{
    [Table("CountryRegulatoryAuthorityMapping", Schema = "product_owner")]
    public class CountryRegulatoryAuthorityMapping
    {
        [Key]
        public long Id { get; set; }

        public int CountryId { get; set; }

        public int RegulatoryAuthorityId { get; set; }

        public int Status { get; set; } = 0;

        public DateTime CreatedOn { get; set; }

        public long CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public long? ModifiedBy { get; set; }

        public string UID { get; set; } = Guid.NewGuid().ToString();

        public string? CountryRegAuthReferenceCode { get; set; }
    }
}