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
            return Ok(new HomeModel { filmTopSix = context.Film.Where(f => f.FilmStatus == 1).ToList().Take(6) });
        }

    }
}
