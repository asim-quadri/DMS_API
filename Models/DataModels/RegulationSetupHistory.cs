using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ComplianceAPI.Models.DataModels
{
    public class RegulationSetupHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long? HistoryId { get; set; }
        public long? RegulationId { get; set; }
        //public string? EmpId { get; set; }
        public int? CountryId { get; set; }
        public long StateId { get; set; }
        public string? RegulationName { get; set; }
        public string? RegulationType { get; set; }
        public long? RegulationGroupId { get; set; }
        public string? Description { get; set; }
        public long? MajorIndustryId { get; set; }
        public long? MinorIndustryId { get; set; }

        public long? ConcernedMinistryId { get; set; }

        public long? RegulatoryAuthorityId { get; set; }
        public long? EntityTypeId { get; set; }
        public int? Status { get; set; }

        public bool? IsConcernedAndAuthorityChecked { get; set; }
        public Boolean? IsParameterChecked { get; set; }
        public long? ManagerId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? UID { get; set; }
        public string? RegulationSetupDetailsReferenceCode { get; set; }
    }
}
