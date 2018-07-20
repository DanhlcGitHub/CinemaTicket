//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ManagerApplication
{
    using System;
    using System.Collections.Generic;
    
    public partial class Film
    {
        public Film()
        {
            this.MovieSchedules = new HashSet<MovieSchedule>();
        }
    
        public int filmId { get; set; }
        public string name { get; set; }
        public System.DateTime dateRelease { get; set; }
        public Nullable<int> restricted { get; set; }
        public Nullable<int> filmLength { get; set; }
        public Nullable<double> imdb { get; set; }
        public string digTypeId { get; set; }
        public string author { get; set; }
        public string movieGenre { get; set; }
        public string filmContent { get; set; }
        public string actorList { get; set; }
        public string countries { get; set; }
        public string trailerLink { get; set; }
        public string posterPicture { get; set; }
        public string additionPicture { get; set; }
        public Nullable<int> filmStatus { get; set; }
        public Nullable<int> ticketSold { get; set; }
    
        public virtual ICollection<MovieSchedule> MovieSchedules { get; set; }
    }
}
