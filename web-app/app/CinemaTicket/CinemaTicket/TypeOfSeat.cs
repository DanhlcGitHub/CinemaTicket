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
    
    public partial class TypeOfSeat
    {
        public TypeOfSeat()
        {
            this.Seats = new HashSet<Seat>();
        }
    
        public int typeSeatId { get; set; }
        public string typeName { get; set; }
        public Nullable<int> capacity { get; set; }
        public Nullable<int> groupId { get; set; }
        public Nullable<double> price { get; set; }
    
        public virtual GroupCinema GroupCinema { get; set; }
        public virtual ICollection<Seat> Seats { get; set; }
    }
}
