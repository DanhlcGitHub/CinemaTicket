namespace CinemaBookingCore.Data.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class TypeOfSeat
    {
        
    
        public int TypeSeatId { get; set; }
        public string TypeName { get; set; }
        public int Capacity { get; set; }
        public int GroupId { get; set; }
        public double Price { get; set; }
    
        public virtual GroupCinema GroupCinema { get; set; }
    }
}
