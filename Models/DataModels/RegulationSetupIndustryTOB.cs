using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ComplianceAPI.Models.DataModels
{
    public class RegulationSetupIndustryTOB
    {
        public long Id { get; set; }
        public long? RegulationSetupId { get; set; }
        public long? ComplianceId { get; set; }
        public long? TOCId { get; set; }

        public long? TocRegisterationId { get; set; }
        public string? TOCRuleType { get; set; }
        public long? MajorIndustryId { get; set; }
        public long? MinorIndustryId { get; set; }
        public long? TOBId { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? UID { get; set; }
    }

    public class RegulationSetupIndustryTOBHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HistoryId { get; set; }
        public long? RegulationSetupId { get; set; }
        public long? ComplianceId { get; set; }
        public long? TOCId { get; set; }
        public string? TOCRuleType { get; set; }
        public long? MajorIndustryId { get; set; }
        public long? MinorIndustryId { get; set; }
        public long? TOBId { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? UID { get; set; }
    }
}
