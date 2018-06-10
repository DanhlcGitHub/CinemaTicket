using CinemaTicket.CustomRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace CinemaTicket.Service
{
    interface ICustomerService
    {
    }
    public class CustomerService : ICustomerService
    {
        CustomerRepository CustomerRepository = new CustomerRepository();
        public List<Customer> GetAll()
        {
            return CustomerRepository.GetAll();
        }
        public Customer FindByID<E>(E id)
        {
            return CustomerRepository.FindByID(id);
        }
        public void Create(Customer entity)
        {
            CustomerRepository.Create(entity);
        }
        public void Update(Customer entity)
        {
            CustomerRepository.Update(entity);
        }
        public void Delete<E>(E id)
        {
            CustomerRepository.Delete(id);
        }
        public List<Customer> FindBy(Expression<Func<Customer, bool>> predicate)
        {
            return CustomerRepository.FindBy(predicate);
        }
    }
}