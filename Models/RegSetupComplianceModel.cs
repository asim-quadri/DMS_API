using ComplianceAPI.Models.DataModels;
using System.ComponentModel.DataAnnotations;

namespace ComplianceAPI.Models
{
    public class RegSetupComplianceModel : Response
    {
        public long? Id { get; set; }

        public long? HistoryId { get; set; }

        public long? ParentComplianceId { get; set; }

        public string? RegulationName { get; set; }
        public long? RegulationSetupId { get; set; }
        public long? ComplianceId { get; set; }
        public string? ComplianceName { get; set; }

        public string? Description { get; set; }

        public int? Status { get; set; }

        public DateTime? CreatedOn { get; set; }

        public long? CreatedBy { get; set; }

        public long? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public Guid? UID { get; set; }
        public Guid? RegulationSetupUID { get; set; }

        public long? ManagerId { get; set; }

        public bool? IsParameterChecked { get; set; }
        public bool? IsConcernedAndAuthorityChecked { get; set; }

        public long? ConcernedMinistryId { get; set; }
        public long? RegulatoryAuthorityId { get; set; }
        public long? MajorIndustryId { get; set; }
        public List<RegSetupComplianceParameterHistory>? Parameters { get; set; }
        public List<RegulationMajorIndustry>? Industry { get; set; }

        public List<RegulationSetupLegalEntityTypeDto>? LegalEntityType { get; set; }
        public List<RegulationTOBList>? TOBList { get; set; }

        public ConcernedMinistry? ConcernedMinistry { get; set; }
        public RegulatoryAuthorities? RegulatoryAuthorities { get; set; }
        //public List<RegulationTOBList>? TOBs { get; set; }
        public List<long>? TOBs { get; set; }
        public string? SectionName { get; set; }
        public string? RegulationSetupComplianceReferenceCode { get; set; }

    }

    public class TOBMinorIndustryRequest
    {
        public List<long>? MajorIndustryIds { get; set; }
        public List<long>? MinorIndustryIds { get; set; }
        public long CountryId { get; set; }
    }

    public class RegulationTOBList
    {
        public long TOBId { get; set; }
        public string? TOBName { get; set; }
    }
}