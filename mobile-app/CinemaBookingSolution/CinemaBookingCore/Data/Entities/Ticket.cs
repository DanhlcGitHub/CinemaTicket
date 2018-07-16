namespace CinemaBookingCore.Data.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class Ticket
    {
        public int TicketId { get; set; }
        public Nullable<int> BookingId { get; set; }
        public int ScheduleId { get; set; }
        public int SeatId { get; set; }
        public string PaymentCode { get; set; }
        public string QrCode { get; set; }
        public string TicketStatus { get; set; }
        public double Price { get; set; }
        public BookingTicket BookingTicket { get; set; }
        public virtual MovieSchedule MovieSchedule { get; set; }
        public virtual Seat Seat { get; set; }
    }
}
