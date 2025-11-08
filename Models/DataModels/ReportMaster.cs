using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComplianceAPI.Models.DataModels
{
    [Table("ReportMaster", Schema = "product_owner")]
    public class ReportMaster
    {
        [Key]
        public long Id { get; set; } 
        public string ReportSequence { get; set; } = null!;

        public string ReportDescription { get; set; } = null!;

        public bool IsActive { get; set; }

    }
}
