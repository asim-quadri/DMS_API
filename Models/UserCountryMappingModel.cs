namespace ComplianceAPI.Models
{
    public class UserCountryMappingModel
    {
        public int UserId { get; set; }

        public int CountryId { get; set; }

        public bool HasAccess { get; set; }
    }

    public class UserStateMappingModel
    {
        public int UserId { get; set; }

        public int StateId { get; set; }

        public bool HasAccess { get; set; }
    }

    public class UserStateMappingResponse
    {
        public int UserId { get; set; }

        public int StateId { get; set; }

        public bool HasAccess { get; set; }

        public string StateName { get; set; }
    }
}