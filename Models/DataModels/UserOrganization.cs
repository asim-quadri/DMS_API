namespace ComplianceAPI.Models.DataModels
{
    public class UserOrganization
    {
        public int Id { get; set; }
        public long UserId { get; set; }
        public long OrganizationId { get; set; }
        public bool HasAccess { get; set; }
    }

}
