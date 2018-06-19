



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CinemaBookingCore.Data.Entities;
using CinemaBookingCore.Data.Models;

namespace CinemaBookingCore.Data.Models
{
    public class SeatCollectionModel
    {
        public int ScheduleId { get; set; }
        public List<Ticket> TicketModels { get; set; }
        public Boolean isSuccesBookingTicket { get; set; }
    }
}
