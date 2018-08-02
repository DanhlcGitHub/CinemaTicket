using CinemaBookingCore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaBookingCore.Data.Models
{
    public class BookingTicketModel
    {
        public int BookingId { get; set; }
        public int CustomerId { get; set; }
        public int PaymentMethodId { get; set; }
        public String PaymentCode { get; set; }
        public int Quantity { get; set; }
        public System.DateTime BookingDate { get; set; }
        public String FilmName { get; set; }
        public String CinemaName { get; set; }
        public String RoomName { get; set; }
        public String StartTime { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
