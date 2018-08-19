
namespace CinemaBookingCore.Data.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class PartnerAccount
    {
        public string PartnerId { get; set; }
        public string PartnerPassword { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int GroupOfCinemaId { get; set; }
    
        public virtual GroupCinema GroupCinema { get; set; }
    }
}
