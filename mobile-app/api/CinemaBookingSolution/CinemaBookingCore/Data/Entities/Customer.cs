
namespace CinemaBookingCore.Data.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class Customer
    {
        public Customer()
        {
            this.BookingTickets = new HashSet<BookingTicket>();
        }
    
        public int CustomerId { get; set; }
        public string UserId { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    
        public virtual ICollection<BookingTicket> BookingTickets { get; set; }
    }
}
