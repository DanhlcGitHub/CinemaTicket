using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VietCineAdmin.Data.Entities;
using VietCineAdmin.Repositories;

namespace VietCineAdmin.Services
{
    interface ICinemaManagerService
    {
    }
    public class CinemaManagerService : ICinemaManagerService
    {
        CinemaManagerRepository CinemaManagerRepository = new CinemaManagerRepository();
        public List<CinemaManager> GetAll()
        {
            return CinemaManagerRepository.GetAll();
        }
        public CinemaManager FindByID<E>(E id)
        {
            return CinemaManagerRepository.FindByID(id);
        }
        public void Create(CinemaManager entity)
        {
            CinemaManagerRepository.Create(entity);
        }
        public void Update(CinemaManager entity)
        {
            CinemaManagerRepository.Update(entity);
        }
        public void Delete<E>(E id)
        {
            CinemaManagerRepository.Delete(id);
        }
        public List<CinemaManager> FindBy(Expression<Func<CinemaManager, bool>> predicate)
        {
            return CinemaManagerRepository.FindBy(predicate);
        }
    }
}
