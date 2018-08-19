
namespace CinemaBookingCore.Data.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class CinemaManager
    {
        public string ManagerId { get; set; }
        public string ManagerPassword { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int CinemaId { get; set; }
    
        public virtual Cinema Cinema { get; set; }
    }
}
