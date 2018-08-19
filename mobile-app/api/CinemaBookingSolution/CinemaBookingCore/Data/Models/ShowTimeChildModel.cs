using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaBookingCore.Data.Models
{
    public class ShowTimeChildModel
    {
        public int TimeId { get; set; }
        public int ScheduleId { get; set; }
        public String TimeStart { get; set; }
        public String TimeEnd { get; set; }
        public String Type { get; set; }
        public String Price { get; set; }
        public int FilmId { get; set; }
        public int RoomId { get; set; }
        public int GroupId { get; set; }
        public int Row { get; set; }
        public int Col { get; set; }
        public String Datetime { get; set; }


    }
}
