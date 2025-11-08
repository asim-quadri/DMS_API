namespace ComplianceAPI.Models
{
    public class UserAccess
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Icon { get; set; }

        public string Route { get; set; }

        public int? ParentId { get; set; }

        public int? SortOrder { get; set; }

        public bool HasAccess { get; set; }
    }

    public class SetAccessRequest
    {
        public int UserId { get; set; }

        public int MenuId { get; set; }

        public bool HasAccess { get; set; }
    }
}