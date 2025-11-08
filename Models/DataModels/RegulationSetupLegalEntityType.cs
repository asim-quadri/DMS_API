using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComplianceAPI.Models.DataModels
{
    public class RegulationSetupLegalEntityType
    {

        public long Id { get; set; }
        public long? RegulationSetupId { get; set; }
        public long? ComplianceId { get; set; }
        public long? TocRegisterationId { get; set; }

        public long? TocId { get; set; }
        public long? LegalEntityType { get; set; }
        //public string? EntityName { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid UID { get; set; }
    }

    public class RegulationSetupLegalEntityTypeHistory
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HistoryId { get; set; }
        public long? RegulationSetupId { get; set; }
        public long? ComplianceId { get; set; }
        public long? LegalEntityType { get; set; }
        public long? TocId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid UID { get; set; }
    }
}
