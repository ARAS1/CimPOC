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
    
    public partial class Company
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string CompanyRegistrationNumber { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
        public string AreasOfOperation { get; set; }
    
        public virtual Address Address1 { get; set; }
        public virtual Telephone Telephones { get; set; }
    }
}
