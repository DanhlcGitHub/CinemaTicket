using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CinemaTicket.Constant
{
    public enum FilmStatus
    {
        showingMovie = 1,
        upcomingMovie = 2,
        notAvailable = -1,
    }

    public static class TicketStatus
    {
        public static String available { get { return "available"; } }
        public static String buying { get { return "buying"; } }
        public static String buyed { get { return "buyed"; } }
    }
}