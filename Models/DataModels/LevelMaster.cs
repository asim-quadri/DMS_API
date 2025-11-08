using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComplianceAPI.Models.DataModels
{
    [Table("LevelMaster", Schema = "client")]
    public class LevelMaster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string LevelType { get; set; } = null!;

        public bool IsActive { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}