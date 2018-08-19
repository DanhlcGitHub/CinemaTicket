using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CinemaBookingCore.Data;
using CinemaBookingCore.Data.Models;
using CinemaBookingCore.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CinemaBookingCore.Controllers
{
    [Route("[controller]")]
    public class GroupCinemaController : Controller
    {
        private readonly CinemaBookingDBContext context;

        public GroupCinemaController(CinemaBookingDBContext context)
        {
            this.context = context;
        }


        [HttpGet]
        public IActionResult Get()
        {
            var groupCinema = context.GroupCinema.Include(g => g.Cinemas).ToList();
           
            return Ok(groupCinema);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok(context.Film.Where(f => f.FilmId ==id).FirstOrDefault());
        }

    }
}
