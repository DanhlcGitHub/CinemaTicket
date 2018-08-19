using CinemaBookingCore.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaBookingCore.Controllers
{
    [Route("[controller]")]
    public class CinemaController : Controller
    {
        private readonly CinemaBookingDBContext context;

        public CinemaController(CinemaBookingDBContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(context.Cinema.ToList());
        }

        [HttpGet("{cinemaId}")]
        public IActionResult Get(int cinemaId)
        {
            var cinema = context.Cinema.Where(c => c.CinemaId == cinemaId).FirstOrDefault();
            return Ok(cinema);
        }
    }
}
