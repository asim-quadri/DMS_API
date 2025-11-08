using ComplianceAPI.Models.DataModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComplianceAPI.Models
{
    public class MinorModule
    {
        public long? Id { get; set; }

        [Required]
        public string MinorModuleName { get; set; } = null!;

        [ForeignKey("MajorModule")]
        public long? MajorModuleId { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedOn { get; set; }

        public MajorModule? MajorModule { get; set; }
    }
}