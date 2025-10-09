namespace DmsApi.Models
{
    public class RegulationGroupModel: Response
    {
        public long? Id { get; set; }
        public long? RegulationGroupId { get; set; }
        public  long? CountryId { get; set; }
        public string? CountryName { get; set; }
        public int? ManagerId { get; set; }
        public string? RegulationGroupName { get; set; }
        public string? RegulationGroupCode { get; set; }
        public long? CountryRegulationGroupMappingId { get; set; }
        public string? ApprovalStatus { get; set; }
        public string? FullName { get; set; }
        public int? StatusId { get; set; }
        public string? Status { get; set; }
        public string? Type { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? UID { get; set; }

    }
}
