using Microsoft.EntityFrameworkCore;

namespace ComplianceAPI.Models.DataModels
{
    public class UserApproval
    {
        public long Id { get; set; }
        public long? UserId { get; set; }
        public long? HistoryId { get; set; }
        public long? ManagerId { get; set; }
        public int? ApprovalStatus { get; set; }
        public int? ApprovalType { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? UID { get; set; }
    }

    
}
