//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClassLibrary1.Data
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
    
        public int roomId { get; set; }
        public Nullable<int> cinemaId { get; set; }
        public Nullable<int> capacity { get; set; }
        public string name { get; set; }
        public Nullable<int> digTypeId { get; set; }
        public Nullable<int> matrixSizeX { get; set; }
        public Nullable<int> matrixSizeY { get; set; }
    
        public virtual Cinema Cinema { get; set; }
        public virtual DigitalType DigitalType { get; set; }
        public virtual ICollection<MovieSchedule> MovieSchedules { get; set; }
        public virtual ICollection<Seat> Seats { get; set; }
    }
}
