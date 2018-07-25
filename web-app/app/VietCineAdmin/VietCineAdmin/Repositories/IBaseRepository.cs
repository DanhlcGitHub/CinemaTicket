using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CrawlCinemaFilm.Repositories
{
    interface IBaseRepository<T> where T : class
    {
        List<T> GetAll();
        T FindByID<E>(E id);
        void Create(T entity);
        void Update(T entity);
        void Delete<E>(E id);
        List<T> FindBy(Expression<Func<T, bool>> predicate);

        List<T> FindByTop(int rank);
    }
}
