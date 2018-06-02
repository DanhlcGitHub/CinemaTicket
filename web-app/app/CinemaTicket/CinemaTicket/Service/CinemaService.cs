using CinemaTicket.CustomRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace CinemaTicket.Service
{
    interface ICinemaService
    {
    }
    public class CinemaService : ICinemaService
    {
        CinemaRepository cinemaRepository = new CinemaRepository();
        public List<Cinema> GetAll()
        {
            return cinemaRepository.GetAll();
        }
        public Cinema FindByID<E>(E id)
        {
            return cinemaRepository.FindByID(id);
        }
        public void Create(Cinema entity)
        {
            cinemaRepository.Create(entity);
        }
        public void Update(Cinema entity)
        {
            cinemaRepository.Update(entity);
        }
        public void Delete<E>(E id)
        {
            cinemaRepository.Delete(id);
        }
        public List<Cinema> FindBy(Expression<Func<Cinema, bool>> predicate)
        {
            return cinemaRepository.FindBy(predicate);
        }
    }
}