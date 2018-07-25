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
    public class FilmsController : Controller
    {
        private readonly CinemaBookingDBContext context;

        public FilmsController(CinemaBookingDBContext context)
        {
            this.context = context;
        }


        [HttpGet]
        public IActionResult Get(Boolean isShowing = true)
        {
            IEnumerable<Film> films;
            if (isShowing)
            {

                films = context.Film.ToList().Where(f => f.FilmStatus == 1);
            }
            else
            {
                films = context.Film.ToList().Where(f => f.FilmStatus == 2);
            }

            return Ok(films);
        }

        [HttpGet("{filmId}")]
        public IActionResult Get(int filmId)
        {
            return Ok(context.Film.Where(f => f.FilmId == filmId).FirstOrDefault());
        }
    }
}
