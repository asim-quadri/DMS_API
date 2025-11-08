using Microsoft.AspNetCore.Http.HttpResults;

namespace ComplianceAPI.Models
{
    public class Products
    {

        public int? Id { get; set; }
        public long UserId { get; set; }
        public string? ProductName { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public long? ModifiedBy { get; set; }
        public Guid? UID { get; set; }
    }


    public class UserProductMapping
    {
        public long? Id { get; set; }
        public long? UserId { get; set; }
        public int? ProductId { get; set; }
        public int? Status { get; set; }
        public int? Enable { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public long? ModifiedBy { get; set; }
        public Guid? UID { get; set; }
    }
}
