using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CinemaBookingCore.Data.Entities;

namespace CinemaBookingCore.Data.Models
{
    public class FilmScheduleModel
    {
        public String Date { get; set; }
        public String Day { get; set; }
        public String DateOfWeek { get; set; }
        public List<DateScheduleModel> DateScheduleModels { get; set; }
    }
}
