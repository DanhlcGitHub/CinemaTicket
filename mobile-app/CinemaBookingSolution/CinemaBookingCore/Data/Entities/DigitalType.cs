
namespace CinemaBookingCore.Data.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class DigitalType
    {
        public DigitalType()
        {
            this.Rooms = new HashSet<Room>();
        }
    
        public int DigTypeId { get; set; }
        public string Name { get; set; }
    
        public virtual ICollection<Room> Rooms { get; set; }
    }
}
