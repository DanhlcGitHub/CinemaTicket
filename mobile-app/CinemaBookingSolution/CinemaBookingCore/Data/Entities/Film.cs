
namespace CinemaBookingCore.Data.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class Film
    {
        public Film()
        {
            this.MovieSchedules = new HashSet<MovieSchedule>();
            this.News = new HashSet<News>();
        }
    
        public int FilmId { get; set; }
        public string Name { get; set; }
        public System.DateTime DateRelease { get; set; }
        public int Restricted { get; set; }
        public int FilmLength { get; set; }
        public double Imdb { get; set; }
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
    
        public virtual ICollection<MovieSchedule> MovieSchedules { get; set; }
        public virtual ICollection<News> News { get; set; }
    }
}
