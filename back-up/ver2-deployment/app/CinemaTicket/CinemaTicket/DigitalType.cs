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
    
    public partial class DigitalType
    {
        public DigitalType()
        {
            this.Rooms = new HashSet<Room>();
        }
    
        public int digTypeId { get; set; }
        public string name { get; set; }
    
        public virtual ICollection<Room> Rooms { get; set; }
    }
}
