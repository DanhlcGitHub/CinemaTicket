
namespace CinemaBookingCore.Data.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class Cinema
    {
        public Cinema()
        {
            this.CinemaManagers = new HashSet<CinemaManager>();
            this.Promotions = new HashSet<Promotion>();
            this.Rooms = new HashSet<Room>();
        }
    
        public int CinemaId { get; set; }
        public string CinemaName { get; set; }
        public int GroupId { get; set; }
        public string ProfilePicture { get; set; }
        public string CinemaAddress { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string OpenTime { get; set; }
        public string Introduction { get; set; }
    
        public virtual GroupCinema GroupCinema { get; set; }
        public virtual ICollection<CinemaManager> CinemaManagers { get; set; }
        public virtual ICollection<Promotion> Promotions { get; set; }
        public virtual ICollection<Room> Rooms { get; set; }
    }
}
