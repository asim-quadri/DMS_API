namespace ComplianceAPI.Models.DataModels
{
    public class EntityTypes
    {
        public long? Id { get; set; }
        //public long? EntityTypeId { get; set; }
        //public long? CountryId { get; set; }
        //public string? CountryName { get; set; }
        //public int? ManagerId { get; set; }
        public string? EntityType { get; set; }
        public string? EntityTypeCode { get; set; }
        //public long? CountryEntityTypeMappingId { get; set; }
        //public string? ApprovalStatus { get; set; }
        //public string? FullName { get; set; }
        //public string? ApprovedBy { get; set; }
        //public int? StatusId { get; set; }
        public int? Status { get; set; }
        //public string? Type { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? UID { get; set; }
        public string? EntityTypeReferenceCode { get; set; }
    }
}
