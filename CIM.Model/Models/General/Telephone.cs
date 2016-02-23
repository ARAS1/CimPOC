using CIM.Model.Models.Enumeration;

namespace CIM.Model.Models.General
{
    public class Telephone
    {
        public int Id { get; set; }
        public string CountryCode { get; set; }
        public string PhoneNumber { get; set; }
        public PhoneType PhoneType{ get; set;}
    }
}
