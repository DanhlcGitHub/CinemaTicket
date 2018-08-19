namespace CinemaBookingCore.Data.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserAccount
    {
        public string UserId { get; set; }
        public string UserPassword { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
