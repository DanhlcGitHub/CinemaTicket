using ManagerApplication.CustomRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace ManagerApplication.Service
{
    interface IBookingTicketService
    {
        int CreateOrder(BookingTicket entity);
    }
    public class BookingTicketService : IBookingTicketService
    {
        BookingTicketRepository bookingTicketRepository = new BookingTicketRepository();
        public List<BookingTicket> GetAll()
        {
            return bookingTicketRepository.GetAll();
        }
        public BookingTicket FindByID<E>(E id)
        {
            return bookingTicketRepository.FindByID(id);
        }
        public void Create(BookingTicket entity)
        {
            bookingTicketRepository.Create(entity);
        }
        public void Update(BookingTicket entity)
        {
            bookingTicketRepository.Update(entity);
        }
        public void Delete<E>(E id)
        {
            bookingTicketRepository.Delete(id);
        }
        public List<BookingTicket> FindBy(Expression<Func<BookingTicket, bool>> predicate)
        {
            return bookingTicketRepository.FindBy(predicate);
        }

        public int CreateOrder(BookingTicket entity)
        {
            return bookingTicketRepository.CreateOrder(entity);
        }
    }
}