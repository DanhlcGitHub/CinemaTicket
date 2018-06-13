using CinemaTicket.BaseRepository;
using CinemaTicket.Utility;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CinemaTicket.CustomRepository
{
    interface ICinemaRepository
    {
        List<Cinema> getCinemaHasScheduleInCurrentDate(DateTime currentDate, int filmId);
    }
    class CinemaRepository : BaseRepository<Cinema>, ICinemaRepository
    {

        public List<Cinema> getCinemaHasScheduleInCurrentDate(DateTime currentDate,int filmId)
        {
            List<Cinema> list = new List<Cinema>();
            using (SqlConnection con = DBUtility.GetConnection1())
            {
                SqlCommand cmd = new SqlCommand("spGetCinemaHasScheduleInCurrentDate", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@currentDate", currentDate);
                cmd.Parameters.AddWithValue("@filmId", filmId);
                con.Open();
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Cinema c = new Cinema();
                    c.cinemaId = Convert.ToInt32(rdr["cinemaId"].ToString());
                    c.profilePicture = rdr["profilePicture"].ToString();
                    c.cinemaName = rdr["cinemaName"].ToString();
                    c.cinemaAddress = rdr["cinemaAddress"].ToString();
                    c.groupId = Convert.ToInt32(rdr["groupId"].ToString());
                    list.Add(c);
                }
            }
            return list;
        }
    }
}