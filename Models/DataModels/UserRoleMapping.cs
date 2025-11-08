namespace ComplianceAPI.Models.DataModels
{
    public class UserRoleMapping
    {
        public long Id { get; set; }
        public long? UserId { get; set; }
        public int? RoleId { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? UID { get; set; }
    }
}
