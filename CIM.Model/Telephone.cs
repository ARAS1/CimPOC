//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CIM.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Telephone
    {
        public int TelephoneId { get; set; }
        public string CountryCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Type { get; set; }
        public int CompanyId { get; set; }
    
        public virtual Company Company { get; set; }
    }
}
