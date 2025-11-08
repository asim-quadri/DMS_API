namespace ComplianceAPI.Models
{
    public class MapEntityDTO
    {
        public int countryId{get;set;}
        public required int[] arrEntityId { get; set; }
    }
    public class EntityStatusUpdateDTO
    {
        public bool isAccept{get;set;}
        public required int[] arrEntityId { get; set; }
    }
}