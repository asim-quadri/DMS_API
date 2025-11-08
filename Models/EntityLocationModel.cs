namespace ComplianceAPI.Models
{
    public class EntityLocationModel
    {
        public long Id { get; set; }
        public string EntityName { get; set; }
        public long? OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public string CountryId { get; set; }
        public string CountryName { get; set; }
        public long? StateId { get; set; }
        public string StateName { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Pin { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int? BillingDetailsMaxColorCode { get; set; } // "1", "2", "3", "Red", "Amber", "Green".
        public string BillingDetailsMaxColorName { get; set; } // "1", "2", "3", "Red", "Amber", "Green".
        public int? ServiceRequestMaxColorCode { get; set; } // "1", "2", "3", "Red", "Amber", "Green".
        public string ServiceRequestMaxColorName { get; set; } // "1", "2", "3", "Red", "Amber", "Green".
    }
}
