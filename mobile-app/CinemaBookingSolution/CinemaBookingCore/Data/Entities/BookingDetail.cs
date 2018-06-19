
namespace CinemaBookingCore.Data.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class BookingDetail
    {
        public int BookingDetailId { get; set; }
        public int BookingId { get; set; }
        public int TicketId { get; set; }
    
        public virtual BookingTicket BookingTicket { get; set; }
        public virtual Ticket Ticket { get; set; }
    }
}
