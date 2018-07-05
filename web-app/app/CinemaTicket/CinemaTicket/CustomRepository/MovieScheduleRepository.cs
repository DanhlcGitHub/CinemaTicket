using CinemaTicket.BaseRepository;
using CinemaTicket.Utility;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CinemaTicket.CustomRepository
{
    interface IMovieScheduleRepository
    {
        List<Object> getMovieScheduleOfCinema(int cinemaID, DateTime currentDate);
        List<Object> GetMovieScheduleForDetailFilm(int cinemaId, DateTime currentDate, int digTypeId, int filmId);
        List<MovieSchedule> FindMovieSchedule(int filmId, int timeId, int cinemaId, DateTime scheduleDate);
    }
    class MovieScheduleRepository : BaseRepository<MovieSchedule>, IMovieScheduleRepository
    {

        public List<Object> getMovieScheduleOfCinema(int cinemaId, DateTime currentDate)
        {
            string endDateStr = currentDate.Year + "-" + currentDate.Month + "-" + currentDate.Day + " " + "23:59:59";
            DateTime endDate = DateTime.Parse(endDateStr);
            List<Object> list = new List<Object>();
            using (SqlConnection con = DBUtility.GetConnection1())
            {
                SqlCommand cmd = new SqlCommand("spGetMovieScheduleOfCinema", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@cinemaId", cinemaId);
                cmd.Parameters.AddWithValue("@currentDate", currentDate);
                cmd.Parameters.AddWithValue("@endOfDay", endDate);
                con.Open();
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Object c = new
                    {
                        filmId = Convert.ToInt32(rdr["filmId"].ToString()),
                        timeId = Convert.ToInt32(rdr["timeId"].ToString()),
                    };
                    list.Add(c);
                }
            }
            return list;
        }

        public List<Object> GetMovieScheduleForDetailFilm(int cinemaId, DateTime currentDate, int digTypeId, int filmId)
        {
            string endDateStr = currentDate.Year + "-" + currentDate.Month + "-" + currentDate.Day + " " + "23:59:59";
            DateTime endDate = DateTime.Parse(endDateStr);
            List<Object> list = new List<Object>();
            using (SqlConnection con = DBUtility.GetConnection1())
            {
                SqlCommand cmd = new SqlCommand("spGetMovieScheduleForDetailFilm", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@cinemaId", cinemaId);
                cmd.Parameters.AddWithValue("@filmId", filmId);
                cmd.Parameters.AddWithValue("@digTypeId", digTypeId);
                cmd.Parameters.AddWithValue("@currentDate", currentDate);
                cmd.Parameters.AddWithValue("@endOfDay", endDate);
                con.Open();
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Object c = new
                    {
                        timeId = Convert.ToInt32(rdr["timeId"].ToString()),
                    };
                    list.Add(c);
                }
            }
            return list;
        }


        public List<MovieSchedule> FindMovieSchedule(int filmId, int timeId, int cinemaId,DateTime scheduleDate)
        {
            using (var db = new CinemaBookingDBEntities())
            {
                List<MovieSchedule> list = db.MovieSchedules.Include("Room")
                                    .Where(s => s.filmId == filmId
                                            && s.timeId == timeId
                                            && s.scheduleDate == scheduleDate
                                            && s.Room.cinemaId == cinemaId).ToList();
                return list;
            }
        }
    }
}