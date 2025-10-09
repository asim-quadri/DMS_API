namespace DmsApi.Models
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

    public class Entity
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public string EntityName { get; set; }
        public int OrganizationId { get; set; }
        public int CountryId { get; set; }
        public int StateId { get; set; }
        public int City { get; set; }
        public string Address { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
    }



}