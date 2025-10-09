namespace DmsApi.Models
{
    public class AccessModel
    {
        public int? Id { get; set; }
        public int? UserId { get; set; }
        public long? CountryId { get; set; }
        public long? StateId { get; set; }
        public long? CountryStateMappingId { get; set; }
        public int? ProductId { get; set; }
        public long? RegulationGroupId { get; set; }
        public long? MajorIndustryId { get; set; }
        public long? MinorIndustryId { get; set; }
        public long? CountryMajorIndustryMappingId { get; set; }
        public long? MajorMinorIndustryMappingId { get; set; }
        public int? Status { get; set; }
        public int? Enable { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public Guid? UID { get; set;}
        public Guid? UserUID { get; set; }
        public int? ManagerId { get; set;}
        public int? ApprovalType { get; set; }
        public int? ApprovalTypeId { get; set; }
        public int? ProductMappingId { get; set; }
        public long? CountryRegulationGroupMappingId { get; set; }
        public int? ApprovalStatus { get;}
        public string? ProductName { get; set; }

    }

}
