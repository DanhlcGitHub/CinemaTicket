//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AdminWebApplication_V2.Data.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class PartnerAccount
    {
        public string partnerId { get; set; }
        public string partnerPassword { get; set; }
        public string partnerName { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public Nullable<int> groupOfCinemaId { get; set; }
        public Nullable<bool> isAvailable { get; set; }
    
        public virtual GroupCinema GroupCinema { get; set; }
    }
}
