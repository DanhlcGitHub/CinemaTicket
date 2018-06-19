namespace CinemaBookingCore.Data.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class Seat
    {
        public Seat()
        {
            this.Tickets = new HashSet<Ticket>();
        }
    
        public int SeatId { get; set; }
        public int TypeSeatId { get; set; }
        public int RoomId { get; set; }
        public int Px { get; set; }
        public int Py { get; set; }
        public int LocationX { get; set; }
        public int LocationY { get; set; }
    
        public virtual Room Room { get; set; }
        public virtual TypeOfSeat TypeOfSeat { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
