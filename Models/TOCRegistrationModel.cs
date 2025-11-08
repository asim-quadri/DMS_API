using ComplianceAPI.Models.DataModels;
using System.ComponentModel.DataAnnotations;

namespace ComplianceAPI.Models
{
    public class TOCModel:Response
    {
        public long ComplianceId { get; set; }
        public TOCRegistrationModel? TOCRegistration { get; set; }
        public List<TOCRules>? TOCRules { get; set; }
        public DateTime? CreatedOn { get; set; }

        public long? CreatedBy { get; set; }

        public long? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public long? ManagerId { get; set; }
    }

    public class TOCRegistrationModel : TOCRegistration
    {
        public long Id { get; set; }
        public long HistoryId { get; set; }
        public string? RuleType { get; set; }
        public List<TOCDocuments>? TOCDocument { get; set; }
        public List<TOCParameter>? TOCParameter { get; set; }
        public long? ManagerId { get; set; }
        public int ResponseCode { get; set; }
        public string? ResponseMessage { get; set; }
        public object? ResultSet { get; set; }
        public List<long>? TOBs { get; set; }

        public List<RegulationTOBList>? TOBList { get; set; }

        public List<RegulationMajorIndustry>? Industry { get; set; }

        public List<RegulationSetupLegalEntityTypeDto>? LegalEntityType { get; set; }

        public ConcernedMinistry? ConcernedMinistry { get; set; }

        public long? ConcernedMinistryId { get; set; }
        public long? RegulatoryAuthorityId { get; set; }
        public RegulatoryAuthorities? RegulatoryAuthorities { get; set; }

    }

    public class TOCRules:Response
    {
        public string? RuleType { get; set; }
        public long? ComplianceId { get; set; }
        public long? RegulationSetupId { get; set; }
        public long? Id { get; set; }
        public long? HistoryId { get; set; }
        public List<TOCDuesModel>? TOCDues { get; set; }
        public List<TOCIntrestPenality>? TOCIntrestPenality { get; set; }
        public List<TOCImprisonment>? TOCImprisonment { get; set; }
        public List<TOCParameter>? TOCParameter { get; set; }
        public DateTime? CreatedOn { get; set; }

        public long? CreatedBy { get; set; }

        public long? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public long? ManagerId { get; set; }
        public List<long>? TOBs { get; set; }
        public List<RegulationTOBList>? TOBList { get; set; }
        public List<RegulationMajorIndustry>? Industry { get; set; }
        public List<RegulationSetupLegalEntityType>? LegalEntityType { get; set; }

        public long? ConcernedMinistryId { get; set; }

        public long? RegulatoryAuthorityId { get; set; }
        public ConcernedMinistry? ConcernedMinistry { get; set; }
        public RegulatoryAuthorities? RegulatoryAuthorities { get; set; }
    }

    public class TOCDuesModel : TOCDues
    {
        //public string? FrequencyType { get; set; }
        //public string? FromTrunOver { get; set; }
        //public string? ToTrunOver { get; set; }
        public DateTime? DueDate { get; set; }
        public List<TOCDueDatesModel>? TOCDueDates { get; set; }
    }

    public class TOCDueDatesModel : TOCDueDates
    {
        public DateTime? DueDate { get; set; }
        public List<TOCDueDatesModel>? TocDueMonths { get; set; }
        //public string? Label { get; set; }
        //public string? FrequencyType { get; set; }
        //public string? FromTrunOver { get; set; }
        //public string? ToTrunOver { get; set; }
        //public string? ForTheMonth { get; set; }


    }
    public class TOCHistoryWithTOBList
    {
        public TOCHistory History { get; set; }
        public List<RegulationTOBList> TOBList { get; set; }
    }
}
