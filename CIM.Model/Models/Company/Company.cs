
using CIM.Model.Models.Enumeration;
using CIM.Model.Models.General;
using Microsoft.Build.Framework;

namespace CIM.Model.Models.Company
{
    public class Company
    {
        public int CompanyId { get; set; }
        [Required]
        public string CompanyName { get; set; }
        [Required]
        public string CompanyRegistrationNumber { get; set; }
        [Required]
        public Address CompanyAddress { get; set; }
        [Required]
        public Telephone CompanyTelephone { get; set; }
        [Required]
        public CompanyType CompanyType { get; set; }
        [Required]
        public AreasOfOperation AreasOfOfOperation { get; set; }
        public Rating Rating { get; set; }
    }
}
