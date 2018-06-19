using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CinemaBookingCore.Data.Entities;

namespace CinemaBookingCore.Data.Models
{
    public class ChoiceSeatsModel
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public List<Seat> Seats {get; set;} 
    }
}
