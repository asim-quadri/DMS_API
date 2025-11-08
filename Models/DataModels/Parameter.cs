namespace ComplianceAPI.Models.DataModels
{
    public class Parameters
    {
        public long Id { get; set; }
        public string? EmpId { get; set; }
        public string? ParameterName { get; set; }
        public string? ParameterType { get; set; }
        public byte? Status { get; set; }
        public long? ManagerId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? UID { get; set; }
        //public int? RoleId { get; set; }
        public string? ParameterReferenceCode { get; set; }

    }
}
