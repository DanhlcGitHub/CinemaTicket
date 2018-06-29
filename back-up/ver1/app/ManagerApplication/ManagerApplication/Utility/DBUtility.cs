using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ManagerApplication.Utility
{
    interface IDBUtility
    {

    }
    class DBUtility : IDBUtility
    {
        public static SqlConnection GetConnection()
        {
            string cs = ConfigurationManager.ConnectionStrings["CinemaBookingDBEntitiesCustom"].ConnectionString;
            SqlConnection con = new SqlConnection(cs);
            return con;
        }
        
        public static SqlConnection GetConnection1()
        {
            using (var db = new CinemaBookingDBEntities())
            {
                var ec = db.Database.Connection;
                var adoConnStr = ec.ConnectionString;
                return new SqlConnection(adoConnStr);
            }
        }
    }
}