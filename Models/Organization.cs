namespace Dms_Api.Models
{
    public class Organization
    {
            public int Id { get; set; }
            public string OrganizationName { get; set; }
            public string Country { get; set; }
            public string State { get; set; }
            public string City { get; set; }
            public string Address { get; set; }
            public string emailAddress { get; set; }
            public string password { get; set; }


        //public string Pincode { get; set; }
        //public DateTime CreatedOn { get; set; }
        //public string CreatedBy { get; set; }
        //public string ModifiedBy { get; set; }
        //public DateTime? ModifiedOn { get; set; }

    }
}
