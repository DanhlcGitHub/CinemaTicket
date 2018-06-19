
namespace CinemaBookingCore.Data.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class MovieSchedule
    {
        public MovieSchedule()
        {
            this.Tickets = new HashSet<Ticket>();
        }
    
        public int ScheduleId { get; set; }
        public int FilmId { get; set; }
        public int TimeId { get; set; }
        public int RoomId { get; set; }
        public DateTime ScheduleDate { get; set; }
    
        public virtual Film Film { get; set; }
        public virtual Room Room { get; set; }
        public virtual ShowTime ShowTime { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
