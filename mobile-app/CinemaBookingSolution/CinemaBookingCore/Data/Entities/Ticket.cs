namespace CinemaBookingCore.Data.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class Ticket
    {
        public Ticket()
        {
            this.BookingDetails = new HashSet<BookingDetail>();
        }
    
        public int TicketId { get; set; }
        public int ScheduleId { get; set; }
        public int SeatId { get; set; }
        public string PaymentCode { get; set; }
        public int QrCode { get; set; }
        public string TicketStatus { get; set; }
        public double Price { get; set; }

        public DateTime TicketTimeout { get; set; }

        public virtual ICollection<BookingDetail> BookingDetails { get; set; }
        public virtual MovieSchedule MovieSchedule { get; set; }
        public virtual Seat Seat { get; set; }
    }
}
