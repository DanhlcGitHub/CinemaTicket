
namespace CinemaBookingCore.Data.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class BookingTicket
    {
        public BookingTicket()
        {
            this.BookingDetails = new HashSet<BookingDetail>();
        }
    
        public int BookingId { get; set; }
        public int CustomerId { get; set; }
        public int PaymentMethodId { get; set; }
        public int Quantity { get; set; }
        public System.DateTime BookingDate { get; set; }
    
        public virtual ICollection<BookingDetail> BookingDetails { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
