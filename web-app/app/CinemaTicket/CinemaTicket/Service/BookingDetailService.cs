using CinemaTicket.CustomRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace CinemaTicket.Service
{
    interface IBookingDetailService
    {
    }
    public class BookingDetailService : IBookingDetailService
    {
        BookingDetailRepository bookingDetailRepository = new BookingDetailRepository();
        public List<BookingDetail> GetAll()
        {
            return bookingDetailRepository.GetAll();
        }
        public BookingDetail FindByID<E>(E id)
        {
            return bookingDetailRepository.FindByID(id);
        }
        public void Create(BookingDetail entity)
        {
            bookingDetailRepository.Create(entity);
        }
        public void Update(BookingDetail entity)
        {
            bookingDetailRepository.Update(entity);
        }
        public void Delete<E>(E id)
        {
            bookingDetailRepository.Delete(id);
        }
        public List<BookingDetail> FindBy(Expression<Func<BookingDetail, bool>> predicate)
        {
            return bookingDetailRepository.FindBy(predicate);
        }
    }
}