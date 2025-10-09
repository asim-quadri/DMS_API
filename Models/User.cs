using System.Reflection;

namespace DmsApi.Models
{

    public class Login
    {
        public string? UserId { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        //public string? MobileNo { get; set; }
        //public string? DeviceId { get; set; }
        //public string? Version { get; set; }

    }

    public class Response
    {
        public int ResponseCode { get; set; }
        public string? ResponseMessage { get; set; }
        public object? ResultSet { get; set; }

    }
    public class User : Response
    {
        public int Id { get; set; }
        public string? EmpId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Mobile { get; set; }
        public string? Password { get; set; }
        public int Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int ManagerId { get; set; }
        public string? ManagerName { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public Guid? UID { get; set; }
        public string RoleDisplayName { get; set; }
        public int? RoleId { get; set; }

    }

    public class PostUser
    {
        public int? Id { get; set; }
        public string? EmpId { get; set; }

        public string? FullName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Mobile { get; set; }
        public string? Password { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? ManagerId { get; set; }
        public int? CreatedBy { get; set; }
        public Guid? UID { get; set; }
        public int? RoleId { get; set; }
        public int? Status { get; set; }
    }
    public class ForgotPassword
    {
        public string? Password { get; set; }
        public string? Email { get; set; }
        //public string? MobileNo { get; set; }
        //public string? DeviceId { get; set; }
        //public string? Version { get; set; }

    }
}
