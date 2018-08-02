using CinemaBookingCore.Constant;
using CinemaBookingCore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CinemaBookingCore.Utility
{
    public class RankingUtility
    {
        public static int getFilmRank(Film aFilm)
        {
            int filmRank = 0;
            int ticketRank = 0; // piority: 2 | 
            int qualityRank = 0;
            int hotRank = 0;
            int ticketSold = 0;

            int showTimeDuration = DateTime.Today.DayOfYear - aFilm.DateRelease.DayOfYear + 1;
            if (aFilm.TicketSold != null && aFilm.TicketSold > 0 && showTimeDuration!=0)
            {
                ticketSold = (int)aFilm.TicketSold;
                ticketRank = RankingConstant.getTickRank(ticketSold / showTimeDuration) * RankingConstant.TickSoldPiority;
            }
            
            if (aFilm.Imdb != null && aFilm.Imdb!=0)
                qualityRank = RankingConstant.getQualityRank((double)aFilm.Imdb) * RankingConstant.QualityPiority;

            hotRank = RankingConstant.getHotRank(showTimeDuration) * RankingConstant.HotPiority;

            filmRank = ticketRank + qualityRank + hotRank;
            return filmRank;
        }

        public Dictionary<Film, int> getFilmFrequency(List<Film> films, int totalShowTime)
        {
            Dictionary<Film, int> filmAndShowTimeMap = new Dictionary<Film, int>();
            int maxItem = films.Count;
            int i = 0;
            while (i < totalShowTime)
            {
                i++;
                int numRank = i / maxItem;
                int compareRank = numRank % Constant.RankingConstant.maxRank;
                int index = i % maxItem;
                Film film = films[index];
                int filmRank = getFilmRank(film);
                if (filmRank == compareRank)
                {
                    KeyValuePair<Film, int> item = filmAndShowTimeMap.FirstOrDefault(t => t.Key == film);
                    if (item.Equals(new KeyValuePair<Film, int>()))
                        filmAndShowTimeMap[film] = item.Value + 1;
                    else
                        filmAndShowTimeMap.Add(film, 0);
                }
            }
            return filmAndShowTimeMap;
        } 
    }
}
