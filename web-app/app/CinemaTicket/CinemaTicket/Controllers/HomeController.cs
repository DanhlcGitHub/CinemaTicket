using CinemaTicket.Constant;
using CinemaTicket.Service;
using CinemaTicket.Utility;
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
            //
            List<Film> filmList = filmService.FindBy(f => f.filmStatus == (int)FilmStatus.showingMovie);// 
            var dictionaryRank = new Dictionary<Film, int>();
            foreach (var item in filmList)
            {
                int rank = RankingUtility.getFilmRank(item);
                dictionaryRank.Add(item, rank);
            }
            var items = from pair in dictionaryRank
                        orderby pair.Value descending
                        select pair;

            List<Film> hightlightFilm = new List<Film>();
            foreach (KeyValuePair<Film, int> pair in items.Take(4))
            {
                hightlightFilm.Add(pair.Key);
            }

            ViewBag.hightLight = hightlightFilm;
            return View();
        }

        public ActionResult FilmDetail(string filmId)
        {
            ViewBag.filmId = filmId;
            return View();//"~/Views/Home/FilmDetail.cshtml"
        }

 
        public ActionResult ChooseTicketAndSeat(string scheduleIdStr)
        {
            /* */
            int scheduleId = Convert.ToInt32(scheduleIdStr);
            ViewBag.scheduleId = scheduleId;
            return View();
        }

        public JsonResult CheckDupplicateRoom(string filmId, string timeId, string cinemaId, string selectDate)
        {
            int filmIdData = Convert.ToInt32(filmId);
            int timeIdData = Convert.ToInt32(timeId);
            int cinemaIdData = Convert.ToInt32(cinemaId);
            string startTime = new ShowTimeService().FindByID(timeIdData).startTime;
            //DateTime scheduleDate = DateTime.Parse(selectDate);
            DateTime dateSelect = DateTime.Parse(selectDate);
            string dateInput = dateSelect.Year + "-" + dateSelect.Month + "-" + dateSelect.Day + " " + startTime;//21:30
            DateTime scheduleDate = DateTime.Parse(dateInput);

            MovieScheduleService msService = new MovieScheduleService();
            List<MovieSchedule> aList = msService.FindMovieSchedule(filmIdData, timeIdData, cinemaIdData, scheduleDate);
            if (aList != null && aList.Count >= 0)
            {
                var obj = aList
                .Select(item => new
                {
                   scheduleId = item.scheduleId,
                   filmId = item.filmId,
                   timeId = item.timeId,
                   roomId = item.roomId,
                   cinemaName = new CinemaService().FindByID(cinemaIdData).cinemaName,
                   startTime = new ShowTimeService().FindByID(item.timeId).startTime,
                   roomName = new RoomService().FindByID(item.roomId).name,
                   availableSeat = new RoomService().FindByID(item.roomId).capacity - 
                              new TicketService().FindBy(t => t.scheduleId == item.scheduleId && t.ticketStatus != "available").Count,
                });
                return Json(obj);
            }
            return null;
        }

        public JsonResult CheckDupplicateRoomToday(string filmId, string timeId, string cinemaId)
        {
            int filmIdData = Convert.ToInt32(filmId);
            int timeIdData = Convert.ToInt32(timeId);
            int cinemaIdData = Convert.ToInt32(cinemaId);
            string startTime = new ShowTimeService().FindByID(timeIdData).startTime;
            //DateTime scheduleDate = DateTime.Parse(selectDate);
            DateTime dateSelect = DateTime.Today;
            string dateInput = dateSelect.Year + "-" + dateSelect.Month + "-" + dateSelect.Day + " " + startTime;//21:30
            DateTime scheduleDate = DateTime.Parse(dateInput);

            MovieScheduleService msService = new MovieScheduleService();
            List<MovieSchedule> aList = msService.FindMovieSchedule(filmIdData, timeIdData, cinemaIdData, scheduleDate);
            if (aList != null && aList.Count >= 0)
            {
                var obj = aList
                .Select(item => new
                {
                    scheduleId = item.scheduleId,
                    filmId = item.filmId,
                    timeId = item.timeId,
                    roomId = item.roomId,
                    cinemaName = new CinemaService().FindByID(cinemaIdData).cinemaName,
                    startTime = new ShowTimeService().FindByID(item.timeId).startTime,
                    roomName = new RoomService().FindByID(item.roomId).name,
                    availableSeat = new RoomService().FindByID(item.roomId).capacity -
                               new TicketService().FindBy(t => t.scheduleId == item.scheduleId && t.ticketStatus != "available").Count,
                });
                return Json(obj);
            }
            return null;
        }

        public ActionResult ChooseTicketAndSeatToday(string filmId, string timeId, string cinemaId)
        {
            int filmIdData = Convert.ToInt32(filmId);
            int timeIdData = Convert.ToInt32(timeId);
            int cinemaIdData = Convert.ToInt32(cinemaId);
            string startTime = new ShowTimeService().FindByID(timeIdData).startTime;

            DateTime today = DateTime.Today; 
            string dateInput = today.Year + "-" + today.Month + "-" + today.Day + " " + startTime;//21:30
            DateTime scheduleDate = DateTime.Parse(dateInput);
            MovieScheduleService msService = new MovieScheduleService();
            List<MovieSchedule> aList = msService.FindMovieSchedule(filmIdData, timeIdData, cinemaIdData, scheduleDate);
            MovieSchedule schedule = null;
            if(aList!=null && aList.Count != 0){
                schedule = aList.First();
                ViewBag.scheduleId = schedule.scheduleId;
                return View("~/Views/Home/ChooseTicketAndSeat.cshtml");
            }
            else
            {
                return View("~/Views/Home/Error404.cshtml");
            }
        }

        public ActionResult BackToChooseTicket(string scheduleId)
        {
            ViewBag.scheduleId = scheduleId;
            return View("~/Views/Home/ChooseTicket.cshtml");
        }
    }
}
