using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComplianceAPI.Models.DataModels
{
    [Table("TOCDocuments", Schema = "product_owner")]
    public class TOCDocuments
    {
        [Key]
        public long Id { get; set; }

        public long? TOCRegistrationId { get; set; }
        public string? DocumentName { get; set; }
        public bool? Status { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? UID { get; set; }
    }

    [Table("TOCDueDates", Schema = "product_owner")]
    public class TOCDueDates
    {
        [Key]
        public long Id { get; set; }
        public long? TOCDuesId { get; set; }
        public long? ParentTocDueDateId { get; set; }
        public string? Label { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid UID { get; set; }
        public string? FrequencyType { get; set; }
        public string? FromTrunOver { get; set; }
        public string? ToTrunOver { get; set; }
        public string? ForTheMonth { get; set; }
        public bool? Status { get; set; }
    }

    [Table("TOCDues", Schema = "product_owner")]
    public class TOCDues
    {
        [Key]
        public long Id { get; set; }
        public long? ComplianceId { get; set; }
        public long? RegulationSetupId { get; set; }
        public string? TOCRuleType { get; set; }
        [MaxLength(50)]
        public string? Frequency { get; set; }
        public string? ForTheMonth { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? UID { get; set; }
        public string? FrequencyType { get; set; }
        public string? FromTrunOver { get; set; }
        public string? ToTrunOver { get; set; }
        public bool? Status { get; set; }

        public string? SectionNameofDues { get; set; }

        public string? DuesReferenceCode { get; set; }

    }

    [Table("TOCImprisonment", Schema = "product_owner")]
    public class TOCImprisonment
    {
        [Key]
        public long Id { get; set; }
        public long? ComplianceId { get; set; }
        public long? RegulationSetupId { get; set; }
        public string? TOCRuleType { get; set; }
        [MaxLength(200)]
        public string? SectionName { get; set; }
        public int? DurationFrom { get; set; }
        public int? DurationTo { get; set; }
        public string? DurationType { get; set; }
        [MaxLength(200)]
        public string? ForWhome { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? UID { get; set; }
    }

    [Table("TOCParameter", Schema = "product_owner")]
    public class TOCParameter
    {
        [Key]
        public long Id { get; set; }
        public long? ComplianceId { get; set; }
        public long? RegulationSetupId { get; set; }
        public string? TOCRuleType { get; set; }
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

    [Table("TOCIntrestPenality", Schema = "product_owner")]
    public class TOCIntrestPenality
    {
        [Key]
        public long Id { get; set; }
        public long? ComplianceId { get; set; }
        public long? RegulationSetupId { get; set; }
        public string? TOCRuleType { get; set; }
        [MaxLength(200)]
        public string? SectionName { get; set; }
        [MaxLength(200)]
        public string? IntrestType { get; set; }
        public decimal? IntrestRate { get; set; }
        [MaxLength(200)]
        public string? IntrestRateFrequency { get; set; }
        [MaxLength(200)]
        public string? Period { get; set; }
        public decimal? PenalityRate { get; set; }
        [MaxLength(200)]
        public string? PenalityRateFrequency { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? UID { get; set; }
    }

    [Table("TOCApproval", Schema = "product_owner")]
    public class TOCApproval
    {
        [Key]
        public long Id { get; set; }
        public long HistoryId { get; set; }
        public long? ComplianceId { get; set; }
        public string? TOCRuleType { get; set; }
        public long? ManagerId { get; set; }           
        public int? ApprovalStatus { get; set; }      
        public DateTime? CreatedOn { get; set; }      
        public long? CreatedBy { get; set; }          
        public long? ModifiedBy { get; set; }         
        public DateTime? ModifiedOn { get; set; }     
        public Guid? UID { get; set; }               
    }

    [Table("TOCHistory", Schema = "product_owner")]
    public class TOCHistory
    {
        [Key]
        public long HistoryId { get; set; }
        public long? ComplianceId { get; set; }
        public long? RegulationSetupId { get; set; }
        public string? TOCRuleType { get; set; }
        public string? TOCHistoryJson { get; set; }
        public DateTime? CreatedOn { get; set; }        
        public long? CreatedBy { get; set; }            
        public long? ModifiedBy { get; set; }           
        public DateTime? ModifiedOn { get; set; }       
        public Guid? UID { get; set; }                  
    }

}
