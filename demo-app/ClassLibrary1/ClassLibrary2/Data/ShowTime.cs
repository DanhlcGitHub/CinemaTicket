//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClassLibrary2.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class ShowTime
    {
        public ShowTime()
        {
            this.MovieSchedules = new HashSet<MovieSchedule>();
        }
    
        public int timeId { get; set; }
        public string startTime { get; set; }
        public string endTime { get; set; }
    
        public virtual ICollection<MovieSchedule> MovieSchedules { get; set; }
    }
}
