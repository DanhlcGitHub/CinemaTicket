using ManagerApplication.CustomRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace ManagerApplication.Service
{
    interface IShowTimeService
    {
    }
    public class ShowTimeService : IShowTimeService
    {
        ShowTimeRepository showTimeRepository = new ShowTimeRepository();
        public List<ShowTime> GetAll()
        {
            return showTimeRepository.GetAll();
        }
        public ShowTime FindByID<E>(E id)
        {
            return showTimeRepository.FindByID(id);
        }
        public void Create(ShowTime entity)
        {
            showTimeRepository.Create(entity);
        }
        public void Update(ShowTime entity)
        {
            showTimeRepository.Update(entity);
        }
        public void Delete<E>(E id)
        {
            showTimeRepository.Delete(id);
        }
        public List<ShowTime> FindBy(Expression<Func<ShowTime, bool>> predicate)
        {
            return showTimeRepository.FindBy(predicate);
        }
    }
}