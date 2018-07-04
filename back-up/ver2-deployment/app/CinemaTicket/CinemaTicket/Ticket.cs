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
    
    public partial class Ticket
    {
        public int ticketId { get; set; }
        public Nullable<int> bookingId { get; set; }
        public Nullable<int> scheduleId { get; set; }
        public Nullable<int> seatId { get; set; }
        public string paymentCode { get; set; }
        public string qrCode { get; set; }
        public string ticketStatus { get; set; }
        public Nullable<double> price { get; set; }
        public string resellDescription { get; set; }
    
        public virtual BookingTicket BookingTicket { get; set; }
        public virtual MovieSchedule MovieSchedule { get; set; }
        public virtual Seat Seat { get; set; }
    }
}
