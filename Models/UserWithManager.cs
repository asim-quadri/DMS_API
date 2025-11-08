namespace ComplianceAPI.Models
{
    public class UserWithManager
    {
        public long UserId { get; set; }
        public string? UserName { get; set; }
        public long ManagerId { get; set; }
        public string? ManagerName { get; set; }
        public int? UserRoleId { get; set; }
        public bool IsSuperAdmin { get; set; }
    }

}
