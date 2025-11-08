using System.ComponentModel.DataAnnotations.Schema;

namespace ComplianceAPI.Models.DataModels
{
    [Table("CountryApproval", Schema = "product_owner")]
    public class CountryApproval
    {
        public long Id { get; set; }

        public long CountryId { get; set; }

        public long ManagerId { get; set; }

        public string? ApprovalStatus { get; set; }

        public DateTime CreatedOn { get; set; }

        public long? CreatedBy { get; set; }

        public long? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public Guid UID { get; set; }
    }
}