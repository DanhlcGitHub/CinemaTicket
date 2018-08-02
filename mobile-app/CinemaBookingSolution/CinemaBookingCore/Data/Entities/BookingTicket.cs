
namespace CinemaBookingCore.Data.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class BookingTicket
    {
        public BookingTicket()
        {
            this.Tickets = new HashSet<Ticket>();
        }
    
        public int BookingId { get; set; }
        public int CustomerId { get; set; }
        public int PaymentMethodId { get; set; }
        public String PaymentCode { get; set; }
        public int Quantity { get; set; }
        public System.DateTime BookingDate { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
