using CrawlCinemaFilm;
using ManagerApplication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagerApplication.BaseRepository
{
    class BaseRepository<T> : IBaseRepository<T> where T : class
    {

        public List<T> GetAll()
        {
            using (var db = new CinemaBookingDBEntities())
            {
                return db.Set<T>().ToList();
            }
        }

        public T FindByID<E>(E id)
        {
            using (var db = new CinemaBookingDBEntities())
            {
                return db.Set<T>().Find(id);
            }
        }
        public void Create(T entity)
        {
            using (var db = new CinemaBookingDBEntities())
            {
                db.Set<T>().Add(entity);
                db.SaveChanges();
            }
        }

        public void Update(T entity)
        {
            using (var db = new CinemaBookingDBEntities())
            {
                db.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void Delete<E>(E id)
        {
            using (var db = new CinemaBookingDBEntities())
            {
                var entity = db.Set<T>().Find(id);
                if (entity != null) db.Set<T>().Remove(entity);
                db.SaveChanges();
            }
        }
        public List<T> FindBy(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            using (var db = new CinemaBookingDBEntities())
            {
                return db.Set<T>().Where(predicate).ToList();
            }
        }
        public List<T> FindByTop(int index)
        {
            using (var db = new CinemaBookingDBEntities())
            {
                return db.Set<T>().Take<T>(index).ToList();
            }
        }
    }
}