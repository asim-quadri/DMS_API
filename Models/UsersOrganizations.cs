namespace ComplianceAPI.Models
{    
    public class UsersOrganizations
    {
        public long UserId { get; set; }
        public long OrganizationId { get; set; }
        public bool HasAccess { get; set; }
    }
}
