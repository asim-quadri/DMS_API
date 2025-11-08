namespace ComplianceAPI.Models
{
        public class Responses
        {
            public int ResponseCode { get; set; }
            public string? ResponseMessage { get; set; }
            public object? ResultSet { get; set; }

        }
        public class Parameter : Responses
        {
            public long Id { get; set; }
            public long? HistoryId { get; set; }
            public string? EmpId { get; set; }
            public string? ParameterName { get; set; }
            public string? ParameterType { get; set; }
            public byte? Status { get; set; }
            public long? ManagerId { get; set; }
            public string? ManagerName { get; set; }
            public DateTime? CreatedOn { get; set; }
            public long? CreatedBy { get; set; }
            public DateTime? ModifiedOn { get; set; }
            public long? ModifiedBy { get; set; }
            public Guid? UID { get; set; }
            public string? RoleDisplayName { get; set; }
            public string? RoleName { get; set; }
            public int? RoleId { get; set; }
            public string? ParameterReferenceCode { get; set; }


    }
    public class AddParameter
        {
            public long? Id { get; set; }
            public string? EmpId { get; set; }
            public string? ParameterName { get; set; }
            public string? ParameterType { get; set; }
            public int? ManagerId { get; set; }
            public int? ApprovalManagerId { get; set; }
            public int? CreatedBy { get; set; }
            public Guid? UID { get; set; }
            public int? RoleId { get; set; }
            public byte? Status { get; set; }
            public string? ParameterReferenceCode { get; set; }
        }
    }

