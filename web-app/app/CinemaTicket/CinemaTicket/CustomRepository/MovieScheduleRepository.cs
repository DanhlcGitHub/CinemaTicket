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
    }
    class MovieScheduleRepository : BaseRepository<MovieSchedule>, IMovieScheduleRepository
    {

        public List<Object> getMovieScheduleOfCinema(int cinemaId, DateTime currentDate)
        {
            List<Object> list = new List<Object>();
            using (SqlConnection con = DBUtility.GetConnection1())
            {
                SqlCommand cmd = new SqlCommand("spGetMovieScheduleOfCinema", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@cinemaId", cinemaId);
                cmd.Parameters.AddWithValue("@currentDate", currentDate);
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
    }
}