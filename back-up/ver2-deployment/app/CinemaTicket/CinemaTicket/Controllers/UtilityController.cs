using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CinemaTicket.Controllers
{
    public class UtilityController : Controller
    {
        //
        // GET: /Utility/

        [HttpPost]
        public JsonResult CompareScheduleTimeForToday(string startTime)
        {
            DateTime today = DateTime.Now;
            string dateInput = today.Year + "-" + today.Month + "-" + today.Day + " " + startTime;//21:30
            DateTime currentDate = DateTime.Parse(dateInput);
            var obj = new
            {
                valid = "true"
            };
            if (currentDate < today)
            {
                obj = new
                {
                    valid = "false"
                };
            }
            return Json(obj);
        }
        
            [HttpPost]
        public JsonResult CompareScheduleTimeForSelectedDate(string startTime,string selectDate)
        {
            //DateTime today = DateTime.Now;
            DateTime today = DateTime.Parse("2017-06-23");
            string dateInput = selectDate + " " + startTime;
            DateTime selectedDate = DateTime.Parse(dateInput);
            var obj = new
            {
                valid = "true"
            };
            if (selectedDate < today)
            {
                obj = new
                {
                    valid = "false"
                };
            }
            return Json(obj);
        }
    }
}
