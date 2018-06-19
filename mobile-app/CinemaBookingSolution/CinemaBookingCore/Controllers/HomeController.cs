using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CinemaBookingCore.Data;
using CinemaBookingCore.Data.Models;
using CinemaBookingCore.Data.Entities;

namespace CinemaBookingCore.Controllers
{
    [Route("[controller]")]
    public class HomeController : Controller
    {
        private readonly CinemaBookingDBContext context;

        public HomeController(CinemaBookingDBContext context)
        {
            this.context = context;
        }


        [HttpGet]
        public IActionResult Get()
        {
            var films = context.Film.ToList().Take(6);
            var newsList = context.News.Take(8).ToList();
            var home = new HomeModel();
            home.filmTopSix = films;
            home.newTopEight = newsList;
            return Ok(home);
        }

    }
}
