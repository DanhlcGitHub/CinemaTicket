using CinemaTicket.BaseRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CinemaTicket.CustomRepository
{
    interface IBookingTicketRepository
    {
        int CreateOrder(BookingTicket entity);
    }
    class BookingTicketRepository : BaseRepository<BookingTicket>, IBookingTicketRepository
    {

        public int CreateOrder(BookingTicket entity)
        {
            using (var db = new CinemaBookingDBEntities())
            {
                db.Set<BookingTicket>().Add(entity);
                db.SaveChanges();
                return entity.bookingId;
            }
        }
    }
}