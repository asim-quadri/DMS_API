using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ComplianceAPI.Models.DataModels
{
    public class RegSetupComplianceParameterHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HistoryId { get; set; }
        public long? Id { get; set; }
        public long? RegulationSetupComplianceId { get; set; }

        public long? ParameterTypeId { get; set; }

        [MaxLength(100)]
        public string? ParameterTypeValue { get; set; }

        [MaxLength(500)]
        public string? ParameterOperator { get; set; }

        public int? Sequence { get; set; }

        public int? Status { get; set; }

        public DateTime? CreatedOn { get; set; }

        public long? CreatedBy { get; set; }

        public long? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public Guid? UID { get; set; }
    }
}
