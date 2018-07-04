using ManagerApplication.CustomRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace ManagerApplication.Service
{
    interface ISeatService
    {
    }
    public class SeatService : ISeatService
    {
        SeatRepository seatRepository = new SeatRepository();
        public List<Seat> GetAll()
        {
            return seatRepository.GetAll();
        }
        public Seat FindByID<E>(E id)
        {
            return seatRepository.FindByID(id);
        }
        public void Create(Seat entity)
        {
            seatRepository.Create(entity);
        }
        public void Update(Seat entity)
        {
            seatRepository.Update(entity);
        }
        public void Delete<E>(E id)
        {
            seatRepository.Delete(id);
        }
        public List<Seat> FindBy(Expression<Func<Seat, bool>> predicate)
        {
            return seatRepository.FindBy(predicate);
        }
    }
}