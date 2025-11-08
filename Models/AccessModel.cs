namespace ComplianceAPI.Models
{
    public class AccessModel
    {
        public int? Id { get; set; }
        public long? UserId { get; set; }
        public long? HistoryId { get; set; }
        public long? CountryId { get; set; }
        public long? StateId { get; set; }
        public long? CountryStateMappingId { get; set; }
        public int? ProductId { get; set; }
        public long? RegulationGroupId { get; set; }
        public long? IndustryMappingId { get; set; }
        public long? MajorIndustryId { get; set; }
        public long? MinorIndustryId { get; set; }
        public long? CountryMajorIndustryMappingId { get; set; }
        public long? MajorMinorIndustryMappingId { get; set; }
        public int? Status { get; set; }
        public int? Enable { get; set; }
        public long? CreatedBy { get; set; }
        public long? ModifiedBy { get; set; }
        public Guid? UID { get; set;}
        public Guid? UserUID { get; set; }
        public long? ManagerId { get; set;}
        public int? ApprovalType { get; set; }
        public int? ApprovalTypeId { get; set; }
        public long? ProductMappingId { get; set; }
        public long? CountryRegulationGroupMappingId { get; set; }
        public int? ApprovalStatus { get; set; }
        public string? ProductName { get; set; }
        public long? EntityTypeId { get; set; }
        public long? CountryEntityTypeMappingId { get; set; }
        public long? OrganizationId { get; set; }
        public long? ComplianceId { get; set; }
        public string? TOCRuleType { get; set; }
        public long? EntityId { get; set; }
        public long? BillingDetailId { get; set; }
        public long? TOBMappingId { get; set; }
        public long? tobHistoryId { get; set; }
        public string? ReferenceCode { get; set; }
    }

}
