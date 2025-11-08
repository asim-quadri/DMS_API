using ComplianceAPI.Models.DataModels;

namespace ComplianceAPI.Models
{
    public class RegulationStupDetails : Response
    {
        public long? Id { get; set; }
        public string? RegulationType { get; set; }
        public long? HistoryId { get; set; }
        public long? CountryId { get; set; }
        public string? CountryName { get; set; }
        public long? StateId { get; set; }
        public string? StateName { get; set; }
        public string? RegulationName { get; set; }
        public long? RegulationGroupId { get; set; }
        public string? RegulationGroupName { get; set; }
        public string? Description { get; set; }
        public long? MajorIndustryId { get; set; }
        public string? MajorIndustryName { get; set; }
        public long? MinorIndustryId { get; set; }
        public string? MinorIndustryName { get; set; }
        public long? EntityTypeId { get; set; }
        public string? EntityType { get; set; }
        public long? ParameterTypeId { get; set; }
        public string? ParameterType { get; set; }
        public string? ParameterMode { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? UID { get; set; }
        public long? ManagerId { get; set; }
        public int? Status { get; set; }
        public string? EmpId { get; set; }
        public int? ApprovalManagerId { get; set; }

        public long? ConcernedMinistryId { get; set; }
        public long? RegulatoryAuthorityId { get; set; }

        public bool? IsConcernedAndAuthorityChecked { get; set; }
        public bool? isParameterChecked { get; set; }
        public List<RegulationSetupParameterHistory>? RegulationSetupParameters { get; set; }
        public List<RegSetupComplianceParameterHistory>? RegulationSetupComplianceParameters { get; set; }
        public List<RegulationMajorIndustry>? Industry { get; set; }

        public List<RegulationSetupLegalEntityTypeDto>? LegalEntityType { get; set; }

        public ConcernedMinistry? ConcernedMinistry { get; set; }
        public RegulatoryAuthorities? RegulatoryAuthorities { get; set; }
        public string? RegulationSetupDetailsReferenceCode { get; set; }

        public string? ApprovedBy { get; set; }
        public string? AddedBy { get; set; }
        public DateTime? FinancialMonth { get; set; }
    }


    public class RegulationTOB
    {
        public long? TOBId { get; set; }
        public string? TOBName { get; set; }
    }

    public class RegulationMinorIndustry
    {
        public long? MinorIndustryId { get; set; }
        public string? MinorIndustryName { get; set; }
        public List<RegulationTOB>? TOBs { get; set; } = new List<RegulationTOB>(); // List of TOBs under this Minor Industry
    }

    public class RegulationMajorIndustry
    {
        public long? MajorIndustryId { get; set; }
        public string? MajorIndustryName { get; set; }
        public List<RegulationMinorIndustry>? MinorIndustries { get; set; } = new List<RegulationMinorIndustry>(); // List of Minor Industries under this Major Industry
    }

    public class RegulationSetupStateList : Response
    {
        public long? Id { get; set; }
        public string? RegulationType { get; set; }
        public long? HistoryId { get; set; }
        public long? CountryId { get; set; }
        public long? StateId { get; set; }
        public string? RegulationName { get; set; }
        public long? RegulationGroupId { get; set; }
        public string? Description { get; set; }
        public long? MajorIndustryId { get; set; }
        public long? MinorIndustryId { get; set; }
        public long? EntityTypeId { get; set; }
        public long? ParameterTypeId { get; set; }
        public string? ParameterType { get; set; }
        public string? ParameterMode { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? UID { get; set; }
        public long? ManagerId { get; set; }
        public int? Status { get; set; }
        public string? EmpId { get; set; }
        public int? ApprovalManagerId { get; set; }
    }
    public class RegulationSetupParameters : Response
    {
        public long? Id { get; set; }
        public long? MajorIndustryId { get; set; }
        public long HistoryId { get; set; }
        public long? MinorIndustryId { get; set; }
        public long? EntityTypeId { get; set; }
        public long? ParameterTypeId { get; set; }
        public string? RegulationName { get; set; }
        public string? ParameterType { get; set; }
        public string? ParameterMode { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? UID { get; set; }
        public int? ManagerId { get; set; }
        public int? Status { get; set; }
        public string? EmpId { get; set; }
    }
    public class AddRegulationStupDetails
    {
        public long? Id { get; set; }
        public string? RegulationType { get; set; }
        public long? CountryId { get; set; }
        public long? StateId { get; set; }
        public string? RegulationName { get; set; }
        public long? RegulationGroupId { get; set; }
        public string? Description { get; set; }
        public long? MajorIndustryId { get; set; }
        public long? MinorIndustryId { get; set; }
        public long? EntityTypeId { get; set; }
        public long? ParameterTypeId { get; set; }
        public string? ParameterType { get; set; }
        public string? ParameterMode { get; set; }
        public int? ManagerId { get; set; }
        public int? ApprovalManagerId { get; set; }
        public int? CreatedBy { get; set; }
        public Guid? UID { get; set; }
        public int? RoleId { get; set; }
        public int? Status { get; set; }
    }
    public class AddRegulationStupParameters
    {
        public long? Id { get; set; }
        public long? MajorIndustryId { get; set; }
        public long? MinorIndustryId { get; set; }
        public long? EntityTypeId { get; set; }
        public long? ParameterTypeId { get; set; }
        public string? RegulationName { get; set; }
        public string? ParameterType { get; set; }
        public string? ParameterMode { get; set; }
        public int? ManagerId { get; set; }
        public int? ApprovalManagerId { get; set; }
        public int? CreatedBy { get; set; }
        public Guid? UID { get; set; }
        public int? RoleId { get; set; }
        public byte? Status { get; set; }
    }
    public class GetRegulationBasicAndParameterByCountryID
    {
        public long? Id { get; set; }
        public int? CountryId { get; set; }
        public string? CountryName { get; set; }
        public Nullable<int> StateId { get; set; }
        public string? StateName { get; set; }
        public long? RegulationGroupId { get; set; }
        public string? RegulationGroupName { get; set; }
        public long? MajorIndustryId { get; set; }
        public string? MajorIndustryName { get; set; }
        public long? MinorIndustryId { get; set; }
        public string? MinorIndustryName { get; set; }
        public long? EntityTypeId { get; set; }
        public string? EntityTypeName { get; set; }
        public long? ParameterTypeId { get; set; }
        public string? RegulationName { get; set; }
        public string? ParameterType { get; set; }
        public string? ParameterMode { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? UID { get; set; }
        public int? ManagerId { get; set; }
        public byte? Status { get; set; }
        public string? EmpId { get; set; }
    }



    public class ReuglationListModle
    {
        public long? Id { get; set; }
        public string? RegulationName { get; set; }
        public string? RuleType { get; set; }
        public Guid? RegulationSetupUID { get; set; }
        public List<ComplianceListModel> Compliance { get; set; }
        public List<TOCListModel> TOC { get; set; }
        public bool? IsParameterChecked { get; set; }
        public long? majorIndustryId { get; set; }

        public string? RegulationSetupDetailsReferenceCode { get; set; }
    }

    public class ComplianceListModel
    {
        public long? Id { get; set; }
        public string? ComplianceName { get; set; }
        public Guid? ComplianceUID { get; set; }
        public string? RuleType { get; set; }
        public List<TOCListModel> TOC { get; set; }
        public List<ComplianceListModel> Compliance { get; set; } = new List<ComplianceListModel>();

        // Nullable type for ParentComplianceId
        public long? ParentComplianceId { get; set; }
    }

    public class TOCListModel
    {
        public long? Id { get; set; }
        public string? TypeOfComplianceName { get; set; }
        public string? RuleType { get; set; }
        public Guid? TypeOfComplianceUID { get; set; }
        public long? TypeOfComplianceId { get; set; }
    }

   

}
