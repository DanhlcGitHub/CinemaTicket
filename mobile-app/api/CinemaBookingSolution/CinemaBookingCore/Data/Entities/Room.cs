namespace CinemaBookingCore.Data.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class Room
    {
        public Room()
        {
            this.MovieSchedules = new HashSet<MovieSchedule>();
            this.Seats = new HashSet<Seat>();
        }
    
        public int RoomId { get; set; }
        public int CinemaId { get; set; }
        public int Capacity { get; set; }
        public string Name { get; set; }
        public int DigTypeId { get; set; }
        public int MatrixSizeX { get; set; }
        public int MatrixSizeY { get; set; }
        public virtual Cinema Cinema { get; set; }
        public virtual DigitalType DigitalType { get; set; }
        public virtual ICollection<MovieSchedule> MovieSchedules { get; set; }
        public virtual ICollection<Seat> Seats { get; set; }
    }
}
