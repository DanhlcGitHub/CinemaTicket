﻿using CinemaTicket.Constant;
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

 
        public ActionResult ChooseTicketAndSeat(string filmId, string timeId, string cinemaId, string selectDate)
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
            MovieSchedule schedule = msService.FindMovieSchedule(filmIdData, timeIdData, cinemaIdData, scheduleDate).First();
            ViewBag.scheduleId = schedule.scheduleId;
            return View();
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
            MovieSchedule schedule = msService.FindMovieSchedule(filmIdData, timeIdData, cinemaIdData, scheduleDate).First();
            ViewBag.scheduleId = schedule.scheduleId;
            return View("~/Views/Home/ChooseTicketAndSeat.cshtml");
        }

        public ActionResult BackToChooseTicket(string scheduleId)
        {
            ViewBag.scheduleId = scheduleId;
            return View("~/Views/Home/ChooseTicket.cshtml");
        }
    }
}
