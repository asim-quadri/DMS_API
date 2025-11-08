namespace ComplianceAPI.Models
{
    public class MenuOptions
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Icon { get; set; }

        public string Route { get; set; }

        public int? ParentId { get; set; }

        public int? SortOrder { get; set; }

        public int MenuId { get; set; }

        public bool CanView { get; set; }
    }

    public class UserAccessItem
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Route { get; set; }

        public int? ParentId { get; set; }
    }
}