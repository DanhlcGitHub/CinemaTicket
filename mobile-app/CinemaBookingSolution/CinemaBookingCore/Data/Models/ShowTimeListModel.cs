using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaBookingCore.Data.Models
{
    public class ShowTimeListModel
    {
        public String CinemaName { get; set; }
        public String CinemaGroupName { get; set; }
        public String FilmName { get; set; }
        public List<ShowTimeChildModel> ShowTimeChildModels { get; set; }
        public String GroupCinemaLogo { get; set; }
        public String FilmImg { get; set; }
    }
}
