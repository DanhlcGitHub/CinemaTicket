using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaBookingCore.Data.Models
{
    public class TicketModel
    {
        public int TicketId { get; set; }
        public Nullable<int> BookingId { get; set; }
        public int ScheduleId { get; set; }
        public int SeatId { get; set; }
        public string QrCode { get; set; }
        public string TicketStatus { get; set; }
        public double Price { get; set; }
        public String SeatPosition { get; set; }
        public int CinemaId { get; set; }
        public int IndexDate { get; set; }
        public int FilmId { get; set; }
    }
}
