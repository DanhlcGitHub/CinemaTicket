using CinemaBookingCore.Data;
using CinemaBookingCore.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaBookingCore.Controllers
{
    [Route("[controller]")]
    public class BookingTicketController : Controller
    {
        private readonly CinemaBookingDBContext context;

        public BookingTicketController(CinemaBookingDBContext context)
        {
            this.context = context;
        }
    }
}
