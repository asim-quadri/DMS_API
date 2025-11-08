namespace ComplianceAPI.Models
{
    public class ProductType
    {
        public int Id { get; set; }
        public Guid UID { get; set; }
        public string TypeOfProduct { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool IsActive { get; set; }
    }
}
