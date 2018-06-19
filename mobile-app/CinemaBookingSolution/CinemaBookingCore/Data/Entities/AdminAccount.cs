
namespace CinemaBookingCore.Data.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class AdminAccount
    {
        public string AdminId { get; set; }
        public string AdminPassword { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
