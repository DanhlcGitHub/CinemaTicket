using ManagerApplication.BaseRepository;
using ManagerApplication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagerApplication.CustomRepository
{
    interface ICustomerRepository
    {
        int createCustomer(Customer cus);
    }
    class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {

        public int createCustomer(Customer cus)
        {
            using (var db = new CinemaBookingDBEntities())
            {
                db.Set<Customer>().Add(cus);
                db.SaveChanges();
                return cus.customerId;
            }
        }
    }
}