namespace ComplianceAPI.Models
{
    public class RegulationComplianceWithCountryModel : Response
    {
        public string Country { get; set; } = string.Empty;

        public string RegulationCode { get; set; } = string.Empty;

        public string RegulationName { get; set; } = string.Empty;

        public string RegulationGroupName { get; set; } = string.Empty;

        public string ParentComplaincecode { get; set; } = string.Empty;

        public string ParentComplianceName { get; set; } = string.Empty;

        public string ComplianceCode { get; set; } = string.Empty;

        public string ComplianceName { get; set; } = string.Empty;

        public string ComplianceSection { get; set; } = string.Empty;

        public string Compliancetype { get; set; } = string.Empty;

        public int? Compliancestatus { get; set; }

        public DateTime? ComplianceEffectiveDate { get; set; }

        public DateTime? ComplianceInactiveDate { get; set; } 
        public string ApplicableByParameter { get; set; } = string.Empty;

        public string parametervalue { get; set; } = string.Empty;

        public DateTime? CreatedOn { get; set; }

        public string AddedBy { get; set; } = string.Empty;

        public string ApprovedBy { get; set; } = string.Empty;




    }
}
