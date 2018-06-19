
namespace CinemaBookingCore.Data.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class BankAccount
    {
        public string CardNumber { get; set; }
        public string OwnerName { get; set; }
        public System.DateTime ExpDate { get; set; }
        public string Email { get; set; }
        public int BankId { get; set; }
    }
}
