using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CinemaTicket.Controllers
{
    public class TicketController : Controller
    {
        //
        // GET: /Ticket/

        public JsonResult CheckSeatAvailable(string choosedList, string scheduleId)
        {
            JArray seatList = JArray.Parse(choosedList);
            foreach (JObject item in seatList)
            {
                string seatId = (string)item.GetValue("seatId");
            }
            return null;
        }

    }
}
