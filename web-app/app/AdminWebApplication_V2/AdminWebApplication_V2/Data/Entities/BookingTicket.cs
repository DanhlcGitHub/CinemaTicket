//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AdminWebApplication_V2.Data.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class BookingTicket
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BookingTicket()
        {
            this.Tickets = new HashSet<Ticket>();
        }
    
        public int bookingId { get; set; }
        public Nullable<int> customerId { get; set; }
        public Nullable<int> paymentMethodId { get; set; }
        public string paymentCode { get; set; }
        public string qrCode { get; set; }
        public Nullable<int> quantity { get; set; }
        public Nullable<System.DateTime> bookingDate { get; set; }
    
        public virtual Customer Customer { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
