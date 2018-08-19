using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CinemaBookingCore.Data.Entities;

namespace CinemaBookingCore.Data.Models
{
    public class HomeModel{
        

        public IEnumerable<Film> filmTopSix { get; set; }
    }
}
