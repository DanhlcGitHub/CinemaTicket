using CrawlCinemaFilm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CrawlController.Constant
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
        public static String resell { get { return "resell"; } }
        public static String reselled { get { return "reselled"; } }

        public static Dictionary<String, String> ViStatus =
        new Dictionary<String, String>
        {
            { "available", "chưa mua" },
            { "buying", "đang mua" },
            { "buyed", "đã mua" },
            { "resell", "bán lại" },
            { "reselled", "đã bán lại" },
        };
    }

    public static class ConstantArray
    {
        public static string[] Alphabet { get { return new string[] { "A", "B", "C", "D", "E", "F", "J", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U" }; } }
    }

    public static class Role
    {
        public static String Partner { get { return "partner"; } }
        public static String CinemaManager { get { return "cinemaManager"; } }
    }

    public static class AppSession
    {
        public static String User { get { return "User"; } }
        public static String UserRole { get { return "UserRole"; } }
    }

    public static class RankingConstant
    {
        public static int maxRank { get { return 4; } }
        public static int excellent { get { return 3; } }
        public static int good { get { return 2; } }
        public static int prettyGood { get { return 1; } }
        public static int normal { get { return 0; } }

        public static int TickSoldPiority { get { return 2; } }
         public static int QualityPiority { get { return 1; } }
         public static int HotPiority { get { return 1; } }

        public static int getTickRank(float tickSold){
            int rank = 0;
            if(tickSold>=0 && tickSold <=100){
                rank = 0;
            }else if(tickSold>=101 && tickSold<=200){
                rank = 1;
            }else if(tickSold>=200 && tickSold<=300){
                rank = 2;
            }else{
                rank = 3;
            }
            return rank;
        }

        public static int getQualityRank(double imdb){
            if(imdb>7) return 2;
            else return 0;
        }

        public static int getHotRank(int duration){
            int rank = 0;
            if(duration<3){
                rank = 3;
            }else if(duration >=3 && duration <6){
                rank = 2;
            }else if(duration >=6 && duration <10){
                rank = 1;
            }
            return rank;
        }

        public static bool isHotFilm(int rank){
            if(rank >= 1)return true;//>=4
            else return false;
        }

        public static bool isHotTime(ShowTime aTime){
            int startTimeNum = Convert.ToInt32(aTime.startTime.Split(':')[0]);
            if(startTimeNum >= 17 && startTimeNum <= 21){
                return true;
            }
            return false;
        }
    }

    
}