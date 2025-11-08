using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComplianceAPI.Models.DataModels
{
    [Table("MinorModule", Schema = "client")]
    public class MinorModule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public string MinorModuleName { get; set; } = null!;

        public long MajorModuleId { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedOn { get; set; }

    }
}