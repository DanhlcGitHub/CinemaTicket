//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CinemaTicket
{
    using System;
    using System.Collections.Generic;
    
    public partial class CinemaManager
    {
        public string managerId { get; set; }
        public string managerPassword { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public Nullable<int> cinemaId { get; set; }
    
        public virtual Cinema Cinema { get; set; }
    }
}
