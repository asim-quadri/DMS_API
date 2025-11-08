using System;

namespace ComplianceAPI.Models
{
    public class PendingApproval
    {
        public long? HistoryId { get; set; }
        public string? FullName { get; set; }
        public string? EmpId { get; set; }
        public string? RoleName { get; set; }
        public string? RoleDisplayName { get; set; }
        public string? ApprovalType { get; set; }
        public int? ApprovalTypeId { get; set; }
        public long? ApproverManager { get; set; }
        public string? Status { get; set; }
        public string? PointofContact { get; set; }
        public string? approvedBy { get; set; }
        public Guid? ApproverUID { get; set; }
        public int? ApprovalStatus { get;set; }
        public Guid? UserUID { get; set; }
        public long? UserId { get; set; }
        public long? ProductMappingId { get; set; }
        public int? ProductID { get; set;}
        public long? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? ParameterName { get; set; }
        public string? ParameterType { get; set; }
        public string? RegulationName { get; set; }
        public string? RegulationType { get; set; }
        public string? TOBName { get; set; }
        //common property
        public string? DisplayName { get; set; }
    }
}
