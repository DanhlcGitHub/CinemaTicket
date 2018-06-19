namespace CinemaBookingCore.Data.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class ShowTime
    {
        public ShowTime()
        {
            this.MovieSchedules = new HashSet<MovieSchedule>();
        }
    
        public int TimeId { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    
        public virtual ICollection<MovieSchedule> MovieSchedules { get; set; }
    }
}
