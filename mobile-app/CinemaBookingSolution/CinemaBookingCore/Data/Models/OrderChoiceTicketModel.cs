using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaBookingCore.Controllers
{
    public class OrderChoiceTicketModel
    {
        public String GroupCinemaName { get; set; }
        public String CinemaName { get; set; }
        public String TimeShow { get; set; }
        public String RoomName { get; set; }
        public String FilmName { get; set; }
        public String Restricted { get; set; }
        public String FilmLength { get; set; }
        public String DigType { get; set; }
        public String FilmImage { get; set; }
        public List<TypeOfSeatModel> TypeOfSeats { get; set; }
        
    }
}
