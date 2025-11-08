using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ComplianceAPI.Models.DataModels
{
    public class RegulationSetupComplianceHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HistoryId { get; set; }

        public long? RegulationSetupId { get; set; }
        public long? ComplianceId { get; set; }

        public long? ParentComplianceId { get; set; }

        [MaxLength(200)]
        public string? ComplianceName { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        public int? Status { get; set; }

        public DateTime? CreatedOn { get; set; }

        public long? CreatedBy { get; set; }

        public long? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public Guid? UID { get; set; }

        public string? SectionName { get; set; }
        public string? RegulationSetupComplianceReferenceCode { get; set; }
    }
}
