namespace ComplianceAPI.Models.DataModels
{
    public class RefRole
    {
        public int Id { get; set; }
        public long? ManagerId { get; set; }
        public string? RoleName { get; set; }
        public string? RoleDisplayName { get; set; }
        public string? Description { get; set; }
        public byte? Status { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? UID { get; set; }
    }
}
