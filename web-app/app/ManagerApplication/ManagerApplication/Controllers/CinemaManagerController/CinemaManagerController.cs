using ManagerApplication.Constant;
using ManagerApplication.Service;
using Newtonsoft.Json.Linq;
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

        public JsonResult GetScheduleByDateFilm(string cinemaIdStr, string selectedDateStr)
        {
            int cinemaId = Convert.ToInt32(cinemaIdStr);
            DateTime selectedDate = DateTime.Parse(selectedDateStr);
            List<Film> filmList = new FilmService().FindBy(f => f.filmStatus != (int)FilmStatus.notAvailable);// 
            string compareDateStr = selectedDate.Year + "-" + selectedDate.Month + "-" + selectedDate.Day + " ";//21:30
            DateTime beginOfDate = DateTime.Parse(compareDateStr + "00:00");
            DateTime endOfDate = DateTime.Parse(compareDateStr + "23:59");

            List<object> returnList = new List<object>();
            List<Room> roomList = new RoomService().FindBy(r => r.cinemaId == cinemaId);

            for (int i = 0; i < roomList.Count; i++)
            {
                List<ShowTime> allShowTime = new ShowTimeService().GetAll();
                Room aRoom = roomList[i];
                List<MovieSchedule> addedShowTime = new MovieScheduleService().FindBy(s => s.scheduleDate > beginOfDate
                                    && s.scheduleDate < endOfDate && s.roomId == aRoom.roomId);
                // remove dupplicate
                for (int j = 0; j < addedShowTime.Count; j++)
                {
                    ShowTime addedTime = new ShowTimeService().FindByID(addedShowTime[j].timeId);
                    for (int k = 0; k < allShowTime.Count; k++)
                    {
                        ShowTime aTime = allShowTime[k];
                        int startTime = Convert.ToInt32(aTime.startTime.Split(':')[0]) * 60 +
                                                     Convert.ToInt32(aTime.startTime.Split(':')[1]);
                        int endTime = Convert.ToInt32(aTime.endTime.Split(':')[0]) * 60 +
                                            Convert.ToInt32(aTime.endTime.Split(':')[1]);
                        int addedStartTime = Convert.ToInt32(addedTime.startTime.Split(':')[0]) * 60 +
                                                Convert.ToInt32(addedTime.startTime.Split(':')[1]);
                        int addedEndTime = Convert.ToInt32(addedTime.endTime.Split(':')[0]) * 60 +
                                                       Convert.ToInt32(addedTime.endTime.Split(':')[1]);

                        if (startTime < addedStartTime && addedStartTime < endTime)
                        {
                            allShowTime.Remove(aTime);
                        }
                        else if (startTime < addedEndTime && endTime > addedEndTime)
                        {
                            allShowTime.Remove(aTime);
                        }
                    }
                }

                //self remove
                for (int j = 0; j < allShowTime.Count; j++)
                {
                    ShowTime aTime = allShowTime[j];
                    for (int k = 0; k < addedShowTime.Count; k++)
                    {
                        if (aTime.timeId == addedShowTime[k].timeId)
                        {
                            allShowTime.Remove(aTime);
                        }
                    }
                }

                //"allShowTime" list now is contain all available showtime
                List<object> currentShowTime = new List<object>();
                foreach (var time in allShowTime)
                {
                    var aTime = new
                    {
                        timeId = time.timeId,
                        startTimeNum = Convert.ToInt32(time.startTime.Split(':')[0]),
                        endTimeNum = Convert.ToInt32(time.endTime.Split(':')[0]),
                        startTime = time.startTime,
                        endTime = time.endTime,
                        status = "available"
                    };
                    currentShowTime.Add(aTime);
                }
                // current Film added ShowTime
                foreach (var item in addedShowTime)
                {
                    ShowTime time = new ShowTimeService().FindByID(item.timeId);
                    int endTimeNum = Convert.ToInt32(time.endTime.Split(':')[0]);
                    int endTimeMinute = Convert.ToInt32(time.endTime.Split(':')[1]);
                    if (endTimeNum == 23 && endTimeMinute == 59) endTimeNum = 24;
                    Film aFilm = filmList.Find(f => f.filmId == item.filmId);
                    var aTime = new
                    {
                        timeId = time.timeId,
                        startTimeNum = Convert.ToInt32(time.startTime.Split(':')[0]),
                        endTimeNum = endTimeNum,
                        startTime = time.startTime,
                        endTime = time.endTime,
                        status = "added",
                        filmId = aFilm.filmId,
                        filmName = aFilm.name,
                    };
                    currentShowTime.Add(aTime);
                }

                var obj = new
                {
                    roomId = aRoom.roomId,
                    roomName = aRoom.name,
                    currentShowTime = currentShowTime
                };

                returnList.Add(obj);
            }
            var returnObj = from s in returnList
                            select s;
            return Json(returnObj);
        }

        public ShowTime findShowTimeById(int id,List<ShowTime> showTimeList)
        {
            return showTimeList.Find(s => s.timeId == id);
        } 

        public JsonResult SaveCustomSchedule()
        {
            string scheduleInfor = Request.Params["scheduleInfor"];
            JObject dataArrayObj = JObject.Parse(scheduleInfor);
            JArray dataArray = (JArray)dataArrayObj.GetValue("dataArray");
            foreach (JObject data in dataArray)
            {
                int roomId = (int)data.GetValue("roomId");
                int filmId = (int)data.GetValue("filmId");
                string selectedDateStr = (string)data.GetValue("selectedDate");
                DateTime inputDate = DateTime.Parse(selectedDateStr);

                JArray timeList = (JArray)data.GetValue("timeList");
                foreach (JObject item in timeList)
                {
                    int timeId = (int)item.GetValue("timeId");
                    ShowTime aTime = new ShowTimeService().FindByID(timeId);
                    MovieSchedule ms = new MovieSchedule();
                    ms.timeId = timeId;
                    ms.roomId = roomId;
                    ms.filmId = filmId;
                    string scheduleDateStr = inputDate.Year + "-" + inputDate.Month + "-" + inputDate.Day + " " + aTime.startTime;
                    DateTime scheduleDate = DateTime.Parse(scheduleDateStr);
                    ms.scheduleDate = scheduleDate;
                    new MovieScheduleService().Create(ms);
                }
            }
            var obj = new
            {
                isSucess = "true",
            };
            return Json(obj);
        }

        [HttpPost]
        public JsonResult LoadAvailableFilm()
        {
            FilmService filmService = new FilmService();
            List<Film> filmList = filmService.FindBy(f => f.filmStatus != (int)FilmStatus.notAvailable);// 
            var obj = filmList
                .Select(item => new
                {
                    id = item.filmId,
                    name = item.name,
                    filmStatus = item.filmStatus,
                    trailerUrl = item.trailerLink,
                    imdb = item.imdb,
                    dateRelease = item.dateRelease,
                    restricted = item.restricted,
                    img = item.additionPicture.Split(';')[0],
                    length = item.filmLength,
                    star = new string[(int)Math.Ceiling((double)item.imdb / 2)]
                });
            return Json(obj);
        }

        [HttpPost]
        public JsonResult GetCurrentDate()
        {
            DateTime today = DateTime.Today;
            var obj = new
            {
                today = today.Year + "-" + today.Month + "-" + today.Day
            };
            return Json(obj);
        }

    }
}
