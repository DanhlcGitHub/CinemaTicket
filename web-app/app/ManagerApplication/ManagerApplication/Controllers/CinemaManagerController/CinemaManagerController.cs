using ManagerApplication.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManagerApplication.Controllers.CinemaManagerController
{
    public class CinemaManagerController : Controller
    {
        //
        // GET: /CinemaManager/

        public ActionResult Home()
        {
            return View();
        }

        public JsonResult GetCinemaInfor(string cinemaIdStr)
        {
            int cinemaId = Convert.ToInt32(cinemaIdStr);
            string serverPath = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            Cinema c = new CinemaService().FindByID(cinemaId);
            var jsonObj = new
            {
                logoImg = serverPath + new GroupCinemaServcie().FindByID(c.groupId).logoImg,
                profileImg = serverPath + c.profilePicture,
                name = c.cinemaName,
                address = c.cinemaAddress,
                email = c.email,
                phone = c.phone
            };
            return Json(jsonObj);
        }

        public JsonResult GetScheduleByDateFilm(string cinemaIdStr,string selectedDateStr, string filmIdStr)
        {
            int cinemaId = Convert.ToInt32(cinemaIdStr);
            int filmId = Convert.ToInt32(filmIdStr);
            DateTime selectedDate = DateTime.Parse(selectedDateStr);

            string compareDateStr = selectedDate.Year + "-" + selectedDate.Month + "-" + selectedDate.Day + " ";//21:30
            DateTime beginOfDate = DateTime.Parse(compareDateStr+ "00:00");
            DateTime endOfDate = DateTime.Parse(compareDateStr+ "23:59");

            List<object> returnList = new List<object>();
            List<Room> roomList = new RoomService().FindBy(r => r.cinemaId == cinemaId);
            
            for(int i = 0 ; i< roomList.Count ; i++){
                List<ShowTime> allShowTime = new ShowTimeService().GetAll();
                Room aRoom = roomList[i];
                List<MovieSchedule> scheduleAlreadyHas = new MovieScheduleService().FindBy(s => s.scheduleDate > beginOfDate
                                    && s.scheduleDate < endOfDate && s.roomId == aRoom.roomId && s.filmId == filmId);
                // remove dupplicate
                for (int j = 0; j < scheduleAlreadyHas.Count; j++)
                {
                    ShowTime addedTime = new ShowTimeService().FindByID(scheduleAlreadyHas[j].timeId);
                    for (int k = 0; k < allShowTime.Count; k++)
                    {
                        ShowTime aTime = allShowTime[k];
                        int startTime = Convert.ToInt32(aTime.startTime.Split(':')[0]);
                        int endTime = Convert.ToInt32(aTime.endTime.Split(':')[0]);
                        int addedStartTime = Convert.ToInt32(addedTime.startTime.Split(':')[0]);
                        int addedEndTime = Convert.ToInt32(addedTime.endTime.Split(':')[0]);

                        if (startTime < addedStartTime && addedStartTime < endTime)
                        {
                            allShowTime.Remove(aTime);
                        }else if(startTime < addedEndTime && endTime > addedEndTime){
                            allShowTime.Remove(aTime);
                        }
                    }
                }

                //self remove
                for (int j= 0; j < allShowTime.Count; j++)
                {
                    ShowTime aTime = allShowTime[j];
                    for (int k = 0; k < scheduleAlreadyHas.Count; k++)
                    {
                        if (aTime.timeId == scheduleAlreadyHas[k].timeId)
                        {
                            allShowTime.Remove(aTime);
                        }
                    }
                }

                var obj= new {
                    roomId = aRoom.roomId,
                    roomName = aRoom.name,
                    availableSchedule = allShowTime.Select(item => new
                    {
                        timeId = item.timeId,
                        startTime = item.startTime,
                        endTime = item.endTime,
                    })
                };
                
                returnList.Add(obj);
            }
            var returnObj = from s in returnList
                           select s;
            return Json(returnObj);
        }

    }
}
