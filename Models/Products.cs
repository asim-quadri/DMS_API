using Microsoft.AspNetCore.Http.HttpResults;

namespace DmsApi.Models
{
    public class Products
    {

        public int? Id { get; set; }
        public long UserId { get; set; }
        public string? ProductName { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public Guid? UID { get; set; }
    }


    public class UserProductMapping
    {
        public int? Id { get; set; }
        public int? UserId { get; set; }
        public int? ProductId { get; set; }
        public int? Status { get; set; }
        public int? Enable { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public Guid? UID { get; set; }
    }
}
