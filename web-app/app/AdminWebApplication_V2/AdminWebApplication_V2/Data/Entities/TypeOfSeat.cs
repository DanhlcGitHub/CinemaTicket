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
    
    public partial class TypeOfSeat
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TypeOfSeat()
        {
            this.Seats = new HashSet<Seat>();
        }
    
        public int typeSeatId { get; set; }
        public string typeName { get; set; }
        public Nullable<int> capacity { get; set; }
        public Nullable<int> groupId { get; set; }
        public Nullable<bool> isPrimary { get; set; }
        public Nullable<double> price { get; set; }
    
        public virtual GroupCinema GroupCinema { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Seat> Seats { get; set; }
    }
}
