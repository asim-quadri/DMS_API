using System.Reflection;

namespace ComplianceAPI.Models
{
    public class Roles : Response
    {
        public long? Id { get; set; }
        public long? HistoryId { get; set; }
        public long? ManagerId { get; set; }
        public string? RoleName { get; set; }
        public string? RoleDisplayName { get; set; }
        public string? Description { get; set; }
        public string? ManagerName { get; set; }
        public byte? Status { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public long? ModifiedBy { get; set; }
        public Guid? UID { get; set; }
    }

    public class UpdateRoles
    {
        public int? RoleId { get; set; }
        public string? RoleDisplayName { get; set; }
        public int? Id { get; set; }
        public Guid? UID { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public long? CreatedBy { get; set; }
        public int? ManagerId { get; set; }
    }
}
