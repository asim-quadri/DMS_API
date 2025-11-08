using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComplianceAPI.Models.DataModels
{
    [Table("CountryConcernedMinistryMapping", Schema = "product_owner")]
    public class CountryConcernedMinistryMapping
    {
        [Key]
        public long Id { get; set; }

        public int CountryId { get; set; }

        public int ConcernedMinistryId { get; set; }

        public int Status { get; set; } = 0;

        public DateTime CreatedOn { get; set; }

        public long CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public long? ModifiedBy { get; set; }

        public string UID { get; set; } = Guid.NewGuid().ToString();

        public string? CountryConMinReferenceCode { get; set; }
    }
}