namespace DmsApi.Models
{
    public class Roles : Response
    {
        public int? Id { get; set; }
        public string? RoleName { get; set; }
        public string? RoleDisplayName { get; set; }
        public string? Description { get; set; }
        public byte? Status { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public Guid? UID { get; set; }
    }

    public class UpdateRoles
    {
        public int? RoleId { get; set; }
        public string? RoleDisplayName { get; set; }
        public int? Id { get; set; }
        public Guid? UID { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? CreatedBy { get; set; }
        public int? ManagerId { get; set; }
    }
}
