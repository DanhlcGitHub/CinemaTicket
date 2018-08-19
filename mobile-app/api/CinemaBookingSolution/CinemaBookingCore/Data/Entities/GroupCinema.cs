
namespace CinemaBookingCore.Data.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class GroupCinema
    {
        public GroupCinema()
        {
            this.Cinemas = new HashSet<Cinema>();
            this.PartnerAccounts = new HashSet<PartnerAccount>();
            this.TypeOfSeats = new HashSet<TypeOfSeat>();
        }
    
        public int GroupId { get; set; }
        public string LogoImg { get; set; }
        public string Name { get; set; }
    
        public virtual ICollection<Cinema> Cinemas { get; set; }
        public virtual ICollection<PartnerAccount> PartnerAccounts { get; set; }
        public virtual ICollection<TypeOfSeat> TypeOfSeats { get; set; }
    }
}
