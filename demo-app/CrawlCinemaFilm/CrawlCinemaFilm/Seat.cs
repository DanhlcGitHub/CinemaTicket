//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CrawlCinemaFilm
{
    using System;
    using System.Collections.Generic;
    
    public partial class Seat
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Seat()
        {
            this.Tickets = new HashSet<Ticket>();
        }
    
        public int seatId { get; set; }
        public Nullable<int> typeSeatId { get; set; }
        public Nullable<int> roomId { get; set; }
        public Nullable<int> px { get; set; }
        public Nullable<int> py { get; set; }
        public Nullable<int> locationX { get; set; }
        public Nullable<int> locationY { get; set; }
    
        public virtual Room Room { get; set; }
        public virtual TypeOfSeat TypeOfSeat { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}