
namespace CinemaBookingCore.Data.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class News
    {
        public int NewsId { get; set; }
        public int FilmId { get; set; }
        public string UrlDocument { get; set; }
    
        public virtual Film Film { get; set; }
    }
}
