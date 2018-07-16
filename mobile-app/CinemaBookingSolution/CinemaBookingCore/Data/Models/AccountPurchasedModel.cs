using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaBookingCore.Data.Models
{
    public class AccountPurchasedModel
    {
        public int BookingTicketId { get; set; }
        public String FilmImage { get; set; }
        public String FilmName { get; set; }
        public String GroupCinemaName { get; set; }
        public String CinemaName { get; set; }
        public String ShowTime { get; set; }
        public String Date { get; set; }
        public String RoomName { get; set; }
        public int ScheduleId { get; set; }
        public int RoomId { get; set; }
        public Double TotalPrice { get; set; }
        public String DigType { get; set; }
        public String Restricted{ get; set; }
        public String StringSeats { get; set; }
        public String Email { get; set; }
        public String Phone { get; set; }

    }
}
