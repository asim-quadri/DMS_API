namespace ComplianceAPI.Models
{
    public class ComplianceTracker
    {
        public long? ComplianceId { get; set; }
        public string? ForTheMonth { get; set; }
        public long? RegulationSetupId { get; set; }
        public long? EntityId { get; set; }
        public string? TOCRuleType { get; set; }
        public string? Frequency { get; set; }
        public DateTime DueDate { get; set; }
        public double DueAmount { get; set; }
        public double AmountPaid { get; set; }
        public DateTime? ActualDate { get; set; }
        public double PayableAmount { get; set; }
        public string? Reason { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public long? ModifiedBy { get; set; }
        public Guid? UID { get; }

    }

    public class RegulationListModel
    {
        public long? Id { get; set; }
        public string? RegulationName { get; set; }
        public string? RuleType { get; set; }
        public Guid? RegulationSetupUID { get; set; }
        public List<ListComplianceModel> Compliance { get; set; }
        public List<TOCListModel> TOC { get; set; }
        public List<ComplianceTracker> ComplianceTrackers { get; set; }
    }

    public class ListComplianceModel
    {
        public long? Id { get; set; }
        public string? ComplianceName { get; set; }
        public Guid? ComplianceUID { get; set; }
        public string? RuleType { get; set; }
        public List<TOCListModel> TOC { get; set; }
        public List<ListComplianceModel> Compliance { get; set; } = new List<ListComplianceModel>();

        // Nullable type for ParentComplianceId
        public long? ParentComplianceId { get; set; }
        public List<ComplianceTracker> ComplianceTrackers { get; set; }

    }
}
