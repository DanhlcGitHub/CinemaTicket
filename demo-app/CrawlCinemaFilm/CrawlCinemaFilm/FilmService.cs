
using CrawlCinemaFilm;
using CrawlController.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace CrawlController.Service
{
    interface IFilmService
    {
        List<Film> FindByName(string key);

        List<Film> FindByTop(int index);
    }
    public class FilmService : IFilmService
    {
        FilmRepository filmRepository = new FilmRepository();
        public List<Film> GetAll()
        {
            return filmRepository.GetAll();
        }
        public Film FindByID<E>(E id)
        {
            return filmRepository.FindByID(id);
        }
        public void Create(Film entity)
        {
            filmRepository.Create(entity);
        }
        public void Update(Film entity)
        {
            filmRepository.Update(entity);
        }
        public void Delete<E>(E id)
        {
            filmRepository.Delete(id);
        }
        public List<Film> FindBy(Expression<Func<Film, bool>> predicate)
        {
            return filmRepository.FindBy(predicate);
        }

        /* implement interface */
        public List<Film> FindByName(string key)
        {
            return filmRepository.FindBy(c => c.name.Contains(key));
        }

        public List<Film> FindByTop(int index)
        {
            return filmRepository.FindByTop(index);
        }
    }
}