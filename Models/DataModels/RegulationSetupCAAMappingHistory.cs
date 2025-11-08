using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComplianceAPI.Models.DataModels
{
    [Table("RegulationSetupCAAMappingHistory", Schema = "product_owner")]
    public class RegulationSetupCAAMappingHistory
    {
        [Key]
        public long Id { get; set; }
        public long? RegulationSetupId { get; set; }
        public long? ComplianceId { get; set; }

        public string? TOCRuleType { get; set; }
        public long? TocId { get; set; }
        public long? ConcernedMinistryId { get; set; }
        public long? RegulatoryAuthorityId { get; set; }
    }
}
