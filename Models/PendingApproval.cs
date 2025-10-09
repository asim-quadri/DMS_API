using System;

namespace DmsApi.Models
{
    public class PendingApproval
    {
        public string? FullName { get; set; }
        public string? EmpId { get; set; }
        public string? RoleDisplayName { get; set; }
        public string? ApprovalType { get; set; }
        public int? ApprovalTypeId { get; set; }
        public int? ApproverManager { get; set; }
        public string? Status { get; set; }
        public string? PointofContact { get; set; }
        public Guid? ApproverUID { get; set; }
        public int? ApprovalStatus { get;set; }
        public Guid? UserUID { get;}
        public int? UserId { get; }
        public int? ProductMappingId { get; set; }
        public int? ProductID { get; set;}
        public int? CreatedBy { get; set; }
    }
}
