namespace ComplianceAPI.Models.DataModels
{
    public class CountryFileNames
    {
        public int? Id { get; set; }

        public int CountryId { get; set; }

        public string FileName { get; set; }

        public int? Status { get; set; }

        public DateTime? CreatedOn { get; set; }

        public long? CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public long? ModifiedBy { get; set; }

        public Guid? UID { get; }
    }
}
