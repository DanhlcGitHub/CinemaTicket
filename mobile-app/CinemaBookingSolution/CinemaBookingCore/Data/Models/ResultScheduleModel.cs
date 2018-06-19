using CinemaBookingCore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaBookingCore.Data.Models
{
    public class ResultScheduleModel
    {
        public int ScheduleId { get; set; }
        public String CinemaName { get; set; }
        public String DateOfWeek { get; set; }
        public String DateOfMonth { get; set; }
        public List<MovieSchedule> MovieSchedules { get; set; }

        public ResultScheduleModel()
        {

        }

        public ResultScheduleModel(int scheduleId, string cinemaName, string dateOfWeek, string dateOfMonth, List<MovieSchedule> movieSchedules)
        {
            ScheduleId = scheduleId;
            CinemaName = cinemaName;
            DateOfWeek = dateOfWeek;
            DateOfMonth = dateOfMonth;
            MovieSchedules = movieSchedules;
        }
    }
}
