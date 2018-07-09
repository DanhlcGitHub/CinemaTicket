//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ManagerApplication
{
    using System;
    using System.Collections.Generic;
    
    public partial class Cinema
    {
        public Cinema()
        {
            this.CinemaManagers = new HashSet<CinemaManager>();
            this.Rooms = new HashSet<Room>();
        }
    
        public int cinemaId { get; set; }
        public string cinemaName { get; set; }
        public Nullable<int> groupId { get; set; }
        public string profilePicture { get; set; }
        public string cinemaAddress { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string openTime { get; set; }
        public string introduction { get; set; }
    
        public virtual GroupCinema GroupCinema { get; set; }
        public virtual ICollection<CinemaManager> CinemaManagers { get; set; }
        public virtual ICollection<Room> Rooms { get; set; }
    }
}
