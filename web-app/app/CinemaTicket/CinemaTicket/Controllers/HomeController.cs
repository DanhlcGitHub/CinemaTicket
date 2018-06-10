using CinemaTicket.Constant;
using CinemaTicket.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CinemaTicket.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            FilmService filmService = new FilmService();
            List<Film> hightLight = filmService.FindByTop(4);
            ViewBag.hightLight = hightLight;
            return View();
        }

        [HttpPost]
        public JsonResult LoadAvailableFilm()
        {
            int x = (int)FilmStatus.showingMovie;
            FilmService filmService = new FilmService();
            List<Film> filmList = filmService.FindBy(f => f.filmStatus != (int)FilmStatus.notAvailable);// 
            var obj = filmList
                .Select(item => new
                {
                    name = item.name,
                    filmStatus = item.filmStatus,
                    trailerUrl = item.trailerLink,
                    imdb = item.imdb,
                    dateRelease = item.dateRelease,
                    restricted = item.restricted,
                    img = item.additionPicture.Split(';')[0],
                    length = item.filmLength,
                    star = new string[(int)Math.Ceiling((double)item.imdb / 2)]
                });
            return Json(obj);
        }

        [HttpPost]
        public JsonResult LoadCinema()
        {
            GroupCinemaServcie gcService = new GroupCinemaServcie();
            CinemaService cService = new CinemaService();
            ShowTimeService tService = new ShowTimeService();
            FilmService fService = new FilmService();
            //DateTime currentDate = DateTime.Today;
            string dateInput = "2018-06-08";
            DateTime currentDate  = DateTime.Parse(dateInput);

            List<GroupCinema> groupCinemaList = gcService.GetAll();
            var obj = groupCinemaList
                .Select(item => new
                {
                    name = item.name,
                    img = item.logoImg,
                    cinemas = cService.FindBy(c => c.groupId == item.GroupId)
                                      .Select(cine => new
                                                      {
                                                          id = cine.cinemaId,
                                                          img = cine.profilePicture,
                                                          name = cine.cinemaName,
                                                          address = cine.cinemaAddress,
                                                          films = getFilmListInSchedule(cine.cinemaId, currentDate)
                                                      })
                });
            return Json(obj);
        }

        [HttpPost]
        public JsonResult Test()
        {
            DateTime currentDate = DateTime.Today;
            int cinemaId = 1;
            MovieScheduleService scheduleService = new MovieScheduleService();
            List<Object> list = scheduleService.getMovieScheduleOfCinema(cinemaId,currentDate);
            return Json(list);
        }

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
                Film aFilm = findFilmInList(filmList,filmId);
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
