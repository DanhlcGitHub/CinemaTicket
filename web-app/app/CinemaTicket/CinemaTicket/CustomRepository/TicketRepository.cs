using CinemaTicket.BaseRepository;
using CinemaTicket.Utility;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CinemaTicket.CustomRepository
{
    interface ITicketRepository
    {
        List<Ticket> getTicketByEmail(string email);
    }
    class TicketRepository : BaseRepository<Ticket>, ITicketRepository
    {

        public List<Ticket> getTicketByEmail(string email)
        {
            List<Ticket> list = new List<Ticket>();
            using (SqlConnection con = DBUtility.GetConnection1())
            {
                SqlCommand cmd = new SqlCommand("spGetTicketByEmail", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@email", email);
                con.Open();
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Ticket c = new Ticket();
                    c.ticketId = Convert.ToInt32(rdr["ticketId"].ToString());
                    c.bookingId = Convert.ToInt32(rdr["bookingId"].ToString());
                    c.scheduleId = Convert.ToInt32(rdr["scheduleId"].ToString());
                    c.seatId = Convert.ToInt32(rdr["seatId"].ToString());
                    c.ticketStatus = rdr["ticketStatus"].ToString();
                    list.Add(c);
                }
            }
            return list;
        }
    }
}