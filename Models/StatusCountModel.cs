namespace ComplianceAPI.Models
{
    public class StatusCountModel
    {
        public int UserId { get; set; }
        public int CountryId { get; set; }
        public int StateId { get; set; }

        // Service Requests
        public int ServiceRequestAmber { get; set; }
        public int ServiceRequestRed { get; set; }
        public int ServiceRequestGreen { get; set; }

        // Billing Details
        public int BillingDetailsAmber { get; set; }
        public int BillingDetailsRed { get; set; }
        public int BillingDetailsGreen { get; set; }

        public int ServiceRequestTotal { get; set; }
        public int BillingDetailsTotal { get; set; }
    }
}
