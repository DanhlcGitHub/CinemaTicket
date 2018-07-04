using ManagerApplication.CustomRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace ManagerApplication.Service
{
    interface ITicketService
    {
    }
    public class TicketService : ITicketService
    {
        TicketRepository ticketRepository = new TicketRepository();
        public List<Ticket> GetAll()
        {
            return ticketRepository.GetAll();
        }
        public Ticket FindByID<E>(E id)
        {
            return ticketRepository.FindByID(id);
        }
        public void Create(Ticket entity)
        {
            ticketRepository.Create(entity);
        }
        public void Update(Ticket entity)
        {
            ticketRepository.Update(entity);
        }
        public void Delete<E>(E id)
        {
            ticketRepository.Delete(id);
        }
        public List<Ticket> FindBy(Expression<Func<Ticket, bool>> predicate)
        {
            return ticketRepository.FindBy(predicate);
        }
    }
}