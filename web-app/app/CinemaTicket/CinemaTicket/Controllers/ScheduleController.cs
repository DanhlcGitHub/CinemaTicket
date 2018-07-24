using CinemaTicket.Service;
using CinemaTicket.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CinemaTicket.Controllers
{
    public class ScheduleController : Controller
    {
        //
        // GET: /Schedule/
        [HttpPost]
        public JsonResult LoadCinemaBelongToGroup()
        {
            GroupCinemaServcie gcService = new GroupCinemaServcie();
            CinemaService cService = new CinemaService();
            ShowTimeService tService = new ShowTimeService();
            FilmService fService = new FilmService();
            DateTime currentDate = DateTime.Now;
            /*string dateInput = "2018-06-08";
            DateTime currentDate = DateTime.Parse(dateInput);*/

            List<GroupCinema> groupCinemaList = gcService.GetAll();
            var obj = groupCinemaList
                .Select(item => new
                {
                    id = item.GroupId,
                    name = item.name,
                    img = item.logoImg,
                    cinemas = cService.FindBy(c => c.groupId == item.GroupId)
                                      .Select(cine => new
                                      {
                                          id = cine.cinemaId,
                                          img = cine.profilePicture,
                                          name = cine.cinemaName,
                                          address = cine.cinemaAddress,
                                          //films = new ScheduleUtility().getFilmListInSchedule(cine.cinemaId, currentDate)
                                      })
                });
            return Json(obj);
        }

        [HttpPost]
        public JsonResult LoadScheduleGroupByCinema()
        {
            GroupCinemaServcie gcService = new GroupCinemaServcie();
            CinemaService cService = new CinemaService();
            ShowTimeService tService = new ShowTimeService();
            FilmService fService = new FilmService();
            DateTime currentDate = DateTime.Now;
            /*string dateInput = "2018-06-08";
            DateTime currentDate = DateTime.Parse(dateInput);*/

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
                                          films = new ScheduleUtility().getFilmListInSchedule(cine.cinemaId, currentDate)
                                      })
                });
            return Json(obj);
        }

        [HttpPost]
        public JsonResult GetFilmByCinemaAndDate(int cinemaId)
        {
            DateTime currentDate = DateTime.Now;
            /*string dateInput = "2018-06-08";
            DateTime currentDate = DateTime.Parse(dateInput);*/
            List<Object> films = new ScheduleUtility().getFilmListInSchedule(cinemaId, currentDate);
            return Json(films);
        }

        [HttpPost]
        public JsonResult LoadScheduleGroupByCinemaForFilmDetail(int filmId)
        {
            GroupCinemaServcie gcService = new GroupCinemaServcie();
            CinemaService cService = new CinemaService();
            ShowTimeService tService = new ShowTimeService();
            MovieScheduleService scheduleService = new MovieScheduleService();
            FilmService fService = new FilmService();
            DateTime currentDate = DateTime.Now;
            /*string dateInput = "2018-06-08";
            DateTime currentDate = DateTime.Parse(dateInput);*/
            Film currentFilm = fService.FindByID(filmId);
            List<DateTime> dates = new DateUtility().getSevenDateFromNow(currentDate);
            string serverPath = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            List<GroupCinema> groupCinemaList = gcService.GetAll();
            var obj = groupCinemaList
                .Select(group => new
                {
                    name = group.name,
                    img = group.logoImg.Contains("http") ? group.logoImg : ( serverPath  + group.logoImg),
                    dates = new DateUtility().getSevenDateFromNow(currentDate)
                            .Select(selectDate => new
                            {
                                dateOfWeek = selectDate.DayOfWeek,
                                date = selectDate.Day,
                                /*cinemas =...*/
                            }),

                });
            return Json(obj);
        }

        [HttpPost]
        public JsonResult LoadScheduleGroupByCinemaForFilmDetailBackGround(int filmId)
        {
            GroupCinemaServcie gcService = new GroupCinemaServcie();
            CinemaService cService = new CinemaService();
            ShowTimeService tService = new ShowTimeService();
            MovieScheduleService scheduleService = new MovieScheduleService();
            FilmService fService = new FilmService();
            DateTime currentDate = DateTime.Now;
            /*string dateInput = "2018-06-08";
            DateTime currentDate = DateTime.Parse(dateInput);*/
            Film currentFilm = fService.FindByID(filmId);
            List<DateTime> dates = new DateUtility().getSevenDateFromNow(currentDate);
            string serverPath = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            List<GroupCinema> groupCinemaList = gcService.GetAll();
            var obj = groupCinemaList
                .Select(group => new
                {
                    name = group.name,
                    img = group.logoImg.Contains("http") ? group.logoImg : (serverPath + group.logoImg),
                    dates = new DateUtility().getSevenDateFromNow(currentDate)
                            .Select(selectDate => new
                            {
                                dateOfWeek = selectDate.DayOfWeek,
                                date = selectDate.Day,
                                cinemas = cService.getCinemaHasScheduleInCurrentDate(selectDate, filmId).FindAll(c => c.groupId == group.GroupId)
                                      .Select(cine => new
                                      {
                                          id = cine.cinemaId,
                                          img =cine.profilePicture.Contains("http") ? cine.profilePicture : (serverPath + cine.profilePicture),
                                          name = cine.cinemaName,
                                          address = cine.cinemaAddress,
                                          digTypeList = currentFilm.digTypeId.Split(';')
                                                        .Select(digType => new{
                                                            type = digType,
                                                            times = scheduleService.GetMovieScheduleForDetailFilm(cine.cinemaId,selectDate,Convert.ToInt32(digType), filmId)
                                                                    .Select(time => new
                                                                    {
                                                                        timeId = Convert.ToInt32(time.GetType().GetProperty("timeId").GetValue(time, null)),
                                                                        startTime = tService.FindByID(Convert.ToInt32(time.GetType().GetProperty("timeId").GetValue(time, null))).startTime
                                                                    })
                                                        })
                                      })
                            }),

                });
            return Json(obj);
        }
        [HttpPost]
        public JsonResult GetSubScheduleFilmDetail(string filmIdStr, string selectDateStr,string groupIdStr)
        {
            int filmId = Convert.ToInt32(filmIdStr);
            int groupId = Convert.ToInt32(groupIdStr);
            string serverPath = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            CinemaService cService = new CinemaService();
            ShowTimeService tService = new ShowTimeService();
            FilmService fService = new FilmService();
            MovieScheduleService scheduleService = new MovieScheduleService();
            Film currentFilm = fService.FindByID(filmId);
            DateTime selectDate = DateTime.Parse(selectDateStr);
            var cinemas = cService.getCinemaHasScheduleInCurrentDate(selectDate, filmId).FindAll(c => (int)c.groupId == groupId)
                                      .Select(cine => new
                                      {
                                          id = cine.cinemaId,
                                          img = cine.profilePicture.Contains("http") ? cine.profilePicture : 
                                                                (serverPath + cine.profilePicture),
                                          name = cine.cinemaName,
                                          address = cine.cinemaAddress,
                                          digTypeList = currentFilm.digTypeId.Split(';')
                                                        .Select(digType => new
                                                        {
                                                            type = digType,
                                                            times = scheduleService.GetMovieScheduleForDetailFilm(cine.cinemaId, selectDate, Convert.ToInt32(digType), filmId)
                                                                    .Select(time => new
                                                                    {
                                                                        timeId = Convert.ToInt32(time.GetType().GetProperty("timeId").GetValue(time, null)),
                                                                        startTime = tService.FindByID(Convert.ToInt32(time.GetType().GetProperty("timeId").GetValue(time, null))).startTime
                                                                    })
                                                        })
                                      });
            return Json(cinemas);
        }

        [HttpPost]
        public JsonResult GetSeventDateFromNow()
        {
            DateTime currentDate = DateTime.Now;
            /*string dateInput = "2018-06-08";
            DateTime currentDate = DateTime.Parse(dateInput);*/
            var obj = new DateUtility().getSevenDateFromNow(currentDate)
                .Select(selectDate => new
                {
                    dateOfWeek = selectDate.DayOfWeek,
                    date = selectDate.Day,
                    fullDate = String.Format("{0:yyyy/MM/dd HH:mm:ss}", selectDate),
                });
            return Json(obj);
        }
    }
}
