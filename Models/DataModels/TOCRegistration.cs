using System.ComponentModel.DataAnnotations;

namespace ComplianceAPI.Models.DataModels
{
    public class TOCRegistration
    {
        [Key]
        public long Id { get; set; }
        public long? ComplianceId { get; set; }
        public long? RegulationSetupId { get; set; }

        public string? RegistrationName { get; set; }
        public string? Description { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid UID { get; set; }

        public string? SectionNameOfRegister { get; set; }
        public string? RegulationSetupTypeOfComplianceRegisterRC { get; set; }
    }
}
