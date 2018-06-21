using ManagerApplication.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ManagerApplication.Service;

namespace ManagerApplication.Utility
{
    public class ScheduleUtility
    {
        public List<Object> getFilmListInSchedule(int cinemaId, DateTime currentDate)
        {
            List<Object> returnList = new List<Object>();
            MovieScheduleService scheduleService = new MovieScheduleService();
            List<Object> list = scheduleService.getMovieScheduleOfCinema(cinemaId, currentDate);
            FilmService fService = new FilmService();
            ShowTimeService tService = new ShowTimeService();
            Object c;
            List<Film> filmList = fService.FindBy(f => f.filmStatus != (int)FilmStatus.notAvailable);
            List<ShowTime> showTimeList = tService.GetAll();

            int currentFilmId = -1;
            List<Object> myTimeList = null;
            foreach (var item in list)
            {
                int filmId = Convert.ToInt32(item.GetType().GetProperty("filmId").GetValue(item, null));
                Film aFilm = findFilmInList(filmList, filmId);
                int timeId = Convert.ToInt32(item.GetType().GetProperty("timeId").GetValue(item, null));
                ShowTime aTime = findShowTimeInList(showTimeList, timeId);

                if (filmId != currentFilmId)
                {
                    currentFilmId = filmId;
                    myTimeList = new List<Object>();
                    Object timeObj = new
                    {
                        id = aTime.timeId,
                        startTime = aTime.startTime,
                        endTime = aTime.endTime,
                    };
                    myTimeList.Add(timeObj);
                    c = new
                    {
                        filmId = aFilm.filmId,
                        filmName = aFilm.name,
                        img = aFilm.additionPicture.Split(';')[0],
                        length = aFilm.filmLength,
                        imdb = aFilm.imdb,
                        restricted = aFilm.restricted,
                        digitalType = aFilm.digTypeId,
                        timeList = myTimeList,
                    };
                    returnList.Add(c);
                }
                else
                {
                    myTimeList.Add(new
                    {
                        id = aTime.timeId,
                        startTime = aTime.startTime,
                        endTime = aTime.endTime,
                    });
                }
            }
            return returnList;
        }

        public Film findFilmInList(List<Film> filmList, int filmId)
        {
            return filmList.FirstOrDefault(film => film.filmId == filmId);
        }

        public ShowTime findShowTimeInList(List<ShowTime> showtimeList, int timeId)
        {
            return showtimeList.FirstOrDefault(time => time.timeId == timeId);
        }
    }
}