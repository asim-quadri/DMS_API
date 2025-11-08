namespace ComplianceAPI.Models.DataModels
{
    public class CountryFileNamesApproval
    {
        public long Id { get; set; }

        public int CountryFileId { get; set; }

        public long ManagerId { get; set; }

        public int ApprovalStatus { get; set; }

        public DateTime? CreatedOn { get; set; }

        public long? CreatedBy { get; set; }

        public long? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public Guid? UID { get; set; }
    }
}