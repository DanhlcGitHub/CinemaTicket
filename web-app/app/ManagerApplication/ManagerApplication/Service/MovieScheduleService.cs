using ManagerApplication.CustomRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace ManagerApplication.Service
{
    interface IMovieScheduleService
    {
        List<Object> getMovieScheduleOfCinema(int cinemaId, DateTime currentDate);
        List<Object> GetMovieScheduleForDetailFilm(int cinemaId, DateTime currentDate, int digTypeId, int filmId);

        List<MovieSchedule> FindMovieSchedule(int filmId, int timeId, int cinemaId, DateTime scheduleDate);
    }
    public class MovieScheduleService : IMovieScheduleService
    {
        MovieScheduleRepository movieScheduleRepository = new MovieScheduleRepository();
        public List<MovieSchedule> GetAll()
        {
            return movieScheduleRepository.GetAll();
        }
        public MovieSchedule FindByID<E>(E id)
        {
            return movieScheduleRepository.FindByID(id);
        }
        public void Create(MovieSchedule entity)
        {
            movieScheduleRepository.Create(entity);
        }
        public void Update(MovieSchedule entity)
        {
            movieScheduleRepository.Update(entity);
        }
        public void Delete<E>(E id)
        {
            movieScheduleRepository.Delete(id);
        }
        public List<MovieSchedule> FindBy(Expression<Func<MovieSchedule, bool>> predicate)
        {
            return movieScheduleRepository.FindBy(predicate);
        }


        public List<Object> getMovieScheduleOfCinema(int cinemaId, DateTime currentDate)
        {
            return movieScheduleRepository.getMovieScheduleOfCinema(cinemaId, currentDate);
        }

        public List<Object> GetMovieScheduleForDetailFilm(int cinemaId, DateTime currentDate, int digTypeId, int filmId)
        {
            return movieScheduleRepository.GetMovieScheduleForDetailFilm(cinemaId, currentDate, digTypeId, filmId);
        }


        public List<MovieSchedule> FindMovieSchedule(int filmId, int timeId, int cinemaId, DateTime scheduleDate)
        {
            return movieScheduleRepository.FindMovieSchedule(filmId, timeId, cinemaId, scheduleDate);
        }
    }
}