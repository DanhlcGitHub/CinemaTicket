using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaBookingCore.Data.Models
{
    public class CinemaScheduleModel
    {
        public String Date { get; set; }
        public String Day { get; set; }
        public String DateOfWeek { get; set; }
        public List<DateScheduleModel> DateScheduleModels { get; set; }
    }
}
