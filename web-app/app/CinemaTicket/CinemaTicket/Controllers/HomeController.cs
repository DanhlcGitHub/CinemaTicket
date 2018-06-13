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

        public ActionResult FilmDetail(string filmId)
        {
            ViewBag.filmId = filmId;
            return View();//"~/Views/Home/FilmDetail.cshtml"
        }

        public ActionResult ChooseTicket(string filmId,string timeId, string cinemaId, string selectDate)
        {
            int filmIdData = Convert.ToInt32(filmId);
            int timeIdData = Convert.ToInt32(timeId);
            int cinemaIdData = Convert.ToInt32(cinemaId);
            DateTime scheduleDate = DateTime.Parse(selectDate);
            MovieScheduleService msService = new MovieScheduleService();
            MovieSchedule schedule = msService.FindMovieSchedule(filmIdData, timeIdData, cinemaIdData, scheduleDate).First();
            ViewBag.scheduleId = schedule.scheduleId;
            return View();
        }

        public ActionResult ChooseSeat(string scheduleId)
        {
            //scheduleId
            ViewBag.scheduleId = scheduleId;
            // sl ghe - loai ghe
            // sl ve
            return View();
        }
    }
}
