
namespace CinemaBookingCore.Data.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class Film
    {
        public Film()
        {
            this.MovieSchedules = new HashSet<MovieSchedule>();
        }
    
        public int FilmId { get; set; }
        public string Name { get; set; }
        public System.DateTime DateRelease { get; set; }
        public int Restricted { get; set; }
        public int FilmLength { get; set; }
        public Nullable<Double> Imdb { get; set; }
        public string DigTypeId { get; set; }
        public string Author { get; set; }
        public string MovieGenre { get; set; }
        public string FilmContent { get; set; }
        public string ActorList { get; set; }
        public string Countries { get; set; }
        public string TrailerLink { get; set; }
        public string PosterPicture { get; set; }
        public string AdditionPicture { get; set; }
        public int FilmStatus { get; set; }
        public Nullable<int> TicketSold { get; set; }

        public virtual ICollection<MovieSchedule> MovieSchedules { get; set; }
        
    }
}
