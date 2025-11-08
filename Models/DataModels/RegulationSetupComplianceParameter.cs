using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ComplianceAPI.Models.DataModels
{
    public class RegulationSetupComplianceParameter
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long? RegulationSetupComplianceId { get; set; }

        public long? ParameterTypeId { get; set; }

        [StringLength(100)]
        public string? ParameterTypeValue { get; set; }

        [StringLength(500)]
        public string? ParameterOperator { get; set; }

        public int? Sequence { get; set; }

        public int? Status { get; set; }

        public DateTime? CreatedOn { get; set; }

        public long? CreatedBy { get; set; }

        public long? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid UID { get; set; }
    }
}
