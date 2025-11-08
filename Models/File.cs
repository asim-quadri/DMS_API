namespace ComplianceAPI.Models
{
    public class Folder
    {
        public long Id { get; set; }
        public string FolderName { get; set; }
        public long? UserId { get; set; }
        public bool IsParent { get; set; }
        public long ParentId { get; set; }
        public long? EntityId { get; set; }
        public string? mtype { get; set; } = "Dms";
        //public DateTime CreatedOn { get; set; }
    }
    public class FolderTreeNode
    {
        public string Label { get; set; }
        public long Id { get; set; }
        public long ParentId { get; set; } = 0;
        public bool Expanded { get; set; }
        public List<FolderTreeNode> Children { get; set; }
    }

    public class FileDetail
    {
        //public long Id { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string FilePath { get; set; }
        public int FolderId { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string FolderName { get; set; }
        public string CreatedOn { get; set; }
    }
}
