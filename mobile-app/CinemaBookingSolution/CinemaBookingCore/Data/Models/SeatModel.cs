using CinemaBookingCore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaBookingCore.Data.Models
{
    public class SeatModel
    {
        public int SeatId { get; set; }
        public int TicketId { get; set; }
        public int TypeSeatId { get; set; }
        public int RoomId { get; set; }
        public int Px { get; set; }
        public int Py { get; set; }
        public double Price { get; set; }
        public Boolean isBooked { get; set; }
        public String TicketStatus { get; set; }
        public String Email { get; set; }
        public String Phone { get; set; }


    }
}
