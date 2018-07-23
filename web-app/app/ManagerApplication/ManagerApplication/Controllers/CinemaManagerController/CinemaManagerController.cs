using ManagerApplication.Constant;
using ManagerApplication.Service;
using ManagerApplication.Utility;
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
            List<object> returnList = new List<object>();
            if (selectedDate < DateTime.Today)
                returnList = getViewOnlyList(cinemaId, selectedDate);
            else
                returnList = getSuggestList(cinemaId, selectedDate);
            
            var returnObj = from s in returnList
                            select s;
            return Json(returnObj);
        }

        public List<object> getSuggestList(int cinemaId, DateTime selectedDate)
        {
            List<object> returnList = new List<object>();
            Random ran = new Random();
            string compareDateStr = selectedDate.Year + "-" + selectedDate.Month + "-" + selectedDate.Day + " ";//21:30
            DateTime beginOfDate = DateTime.Parse(compareDateStr + "00:00");
            DateTime endOfDate = DateTime.Parse(compareDateStr + "23:59");
            List<Film> baseFilmList = new FilmService().FindBy(f => f.filmStatus != (int)FilmStatus.notAvailable);
            List<Film> filmList = new FilmService().FindBy(f => f.filmStatus == (int)FilmStatus.showingMovie);// 
            List<Film> hotFilmList = new List<Film>();
            List<Film> normalFilmList = new List<Film>();
            List<Room> roomList = new RoomService().FindBy(r => r.cinemaId == cinemaId);
            foreach (var item in filmList)
            {
                int rank = RankingUtility.getFilmRank(item);
                if (RankingConstant.isHotFilm(rank))
                {
                    hotFilmList.Add(item);
                    normalFilmList.Add(item);
                }

                normalFilmList.Add(item);
            }

            for (int i = 0; i < roomList.Count; i++)
            {
                List<ShowTime> baseShowTime = new ShowTimeService().GetAll();
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

                for (int j = 0; j < addedShowTime.Count; j++)
                {
                    int addedTimeId = addedShowTime[j].timeId;
                    ShowTime removeTime = allShowTime.Find(t => t.timeId == addedTimeId);
                    if (removeTime != null)
                    {
                        allShowTime.Remove(removeTime);
                    }
                }


                //get addable and not dupplicate showtimelist
                List<ShowTime> suggestList = new List<ShowTime>();
                for (int j = 0; j < allShowTime.Count; j++)
                {
                    //select one
                    ShowTime aTime = allShowTime[j];
                    //check dupplicate ; if dupplicate ->remove
                    // checkAddableSchedule khong trung vs added list
                    // isDupplicate() khong dupplicate vs list hien tai
                    if (checkAddableSchedule(aTime.timeId, addedShowTime, baseShowTime) && !isDupplicate(aTime.timeId, suggestList, baseShowTime))
                    {
                        suggestList.Add(aTime);
                    }
                }

                //get dictionary
                Dictionary<ShowTime, Film> suggestDictionary = new Dictionary<ShowTime, Film>();
                foreach (var time in suggestList)
                {

                    if (RankingConstant.isHotTime(time))
                    {
                        if (hotFilmList.Count != 0)
                        {
                            int ranNum = ran.Next(hotFilmList.Count);
                            suggestDictionary.Add(time, hotFilmList[ranNum]);
                        }
                    }
                    else
                    {
                        if (normalFilmList.Count != 0)
                        {
                            int ranNum = ran.Next(normalFilmList.Count);
                            suggestDictionary.Add(time, normalFilmList[ranNum]);
                        }

                    }
                }

                //"allShowTime" list now is contain all available showtime
                List<object> currentShowTime = new List<object>();

                foreach (var time in allShowTime)//11 13 14 15 19
                {
                    string status = "available";
                    int filmId = -1;
                    string name = "";
                    Film aFilm = findFilmInDictionary(time, suggestDictionary);
                    if (aFilm != null)
                    {
                        status = "suggested";
                        filmId = aFilm.filmId;
                        name = aFilm.name;
                    }

                    var aTime = new
                    {
                        timeId = time.timeId,
                        startTimeNum = Convert.ToInt32(time.startTime.Split(':')[0]),
                        endTimeNum = Convert.ToInt32(time.endTime.Split(':')[0]),
                        startTime = time.startTime,
                        endTime = time.endTime,
                        status = status,
                        filmId = filmId,
                        filmName = name,
                    };
                    currentShowTime.Add(aTime);
                }
                // add suggest show time

                // current Film added ShowTime
                foreach (var item in addedShowTime)//19 
                {
                    ShowTime time = baseShowTime.Find(t => t.timeId == item.timeId);
                    int endTimeNum = Convert.ToInt32(time.endTime.Split(':')[0]);
                    int endTimeMinute = Convert.ToInt32(time.endTime.Split(':')[1]);
                    if (endTimeNum == 23 && endTimeMinute == 59) endTimeNum = 24;
                    Film aFilm = baseFilmList.Find(f => f.filmId == item.filmId);
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
            return returnList;
        }

        public List<object> getViewOnlyList(int cinemaId, DateTime selectedDate)
        {
            List<object> returnList = new List<object>();
            string compareDateStr = selectedDate.Year + "-" + selectedDate.Month + "-" + selectedDate.Day + " ";//21:30
            DateTime beginOfDate = DateTime.Parse(compareDateStr + "00:00");
            DateTime endOfDate = DateTime.Parse(compareDateStr + "23:59");
            List<Film> baseFilmList = new FilmService().FindBy(f => f.filmStatus != (int)FilmStatus.notAvailable);
            List<Room> roomList = new RoomService().FindBy(r => r.cinemaId == cinemaId);


            for (int i = 0; i < roomList.Count; i++)
            {
                List<ShowTime> allShowTime = new ShowTimeService().GetAll();
                Room aRoom = roomList[i];
                List<MovieSchedule> addedShowTime = new MovieScheduleService().FindBy(s => s.scheduleDate > beginOfDate
                                    && s.scheduleDate < endOfDate && s.roomId == aRoom.roomId);

                // add suggest show time
                List<object> currentShowTime = new List<object>();
                // current Film added ShowTime
                foreach (var item in addedShowTime)
                {
                    ShowTime time = allShowTime.Find(t => t.timeId == item.timeId);
                    int endTimeNum = Convert.ToInt32(time.endTime.Split(':')[0]);
                    int endTimeMinute = Convert.ToInt32(time.endTime.Split(':')[1]);
                    if (endTimeNum == 23 && endTimeMinute == 59) endTimeNum = 24;
                    Film aFilm = baseFilmList.Find(f => f.filmId == item.filmId);
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
            return returnList;
        }

        private Film findFilmInDictionary(ShowTime aTime, Dictionary<ShowTime, Film> myDictionary)
        {
            foreach (KeyValuePair<ShowTime, Film> entry in myDictionary)
            {
                ShowTime time = entry.Key;
                if (time.timeId == aTime.timeId)
                {
                    return entry.Value;
                }
            }
            return null;
        }

        private bool isDupplicate(int timeId, List<ShowTime> addedShowTime, List<ShowTime> allShowTime)
        {

            ShowTime aTime = allShowTime.Find(t => t.timeId == timeId);
            foreach (ShowTime schedule in addedShowTime)
            {
                ShowTime addedTime = allShowTime.Find(t => t.timeId == schedule.timeId);
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
                    return true;
                }
                else if (startTime < addedEndTime && endTime > addedEndTime)
                {
                    return true;
                }
            }
            return false;
        }

        public ShowTime findShowTimeById(int id, List<ShowTime> showTimeList)
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
                //int filmId = (int)data.GetValue("filmId");
                string selectedDateStr = (string)data.GetValue("selectedDate");
                DateTime inputDate = DateTime.Parse(selectedDateStr);

                JArray timeList = (JArray)data.GetValue("timeList");
                foreach (JObject item in timeList)
                {
                    int timeId = (int)item.GetValue("timeId");
                    int filmId = (int)item.GetValue("filmId");
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
            List<Film> filmList = filmService.FindBy(f => f.filmStatus == (int)FilmStatus.showingMovie);// 
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


        public JsonResult LoadAllRoomByCinemaId(string cinemaIdStr)
        {
            int cinemaId = Convert.ToInt32(cinemaIdStr);
            List<Room> roomList = new RoomService().FindBy(r => r.cinemaId == cinemaId);// 
            var obj = roomList
                .Select(item => new
                {
                    roomId = item.roomId,
                    roomName = item.name,
                });
            return Json(obj);
        }


        public JsonResult LoadAllShowTime()
        {
            List<ShowTime> timeList = new ShowTimeService().GetAll();
            var obj = timeList
                .Select(item => new
                {
                    timeId = item.timeId,
                    startTime = item.startTime,
                    endTime = item.endTime
                });
            return Json(obj);
        }

        public JsonResult basicAddSchedule(string filmIdStr,string timeIdStr,string roomIdStr, string scheduleDateStr)
        {
            string message = "Add success!";
            List<ShowTime> allShowTime = new ShowTimeService().GetAll();
            int filmId = Convert.ToInt32(filmIdStr);
            int timeId = Convert.ToInt32(timeIdStr);
            ShowTime aTime = allShowTime.Find(t => t.timeId == timeId);
            int roomId = Convert.ToInt32(roomIdStr);
            DateTime inputedDate = DateTime.Parse(scheduleDateStr);
            string formatDateStr = inputedDate.Year + "-" + inputedDate.Month + "-" + inputedDate.Day + " " + aTime.startTime;
            DateTime scheduleDate = DateTime.Parse(formatDateStr);

            List<MovieSchedule> list = new MovieScheduleService().FindBy(ms => ms.filmId == filmId && ms.roomId == roomId
                                                             && ms.scheduleDate == scheduleDate);
            if (list == null || list.Count == 0)
            {
                //lay tat ca suat chieu da add trong ngay
                DateTime beginOfDate = DateTime.Parse(inputedDate.Year + "-" + inputedDate.Month + "-" + inputedDate.Day  +" 00:00");
                DateTime endOfDate = DateTime.Parse(inputedDate.Year + "-" + inputedDate.Month + "-" + inputedDate.Day  +" 23:59");
                List<MovieSchedule> addedShowTime = new MovieScheduleService().FindBy(s => s.scheduleDate > beginOfDate
                                    && s.scheduleDate < endOfDate && s.roomId == roomId);
                if (checkAddableSchedule(timeId, addedShowTime,allShowTime))
                {
                    MovieSchedule ms = new MovieSchedule();
                    ms.filmId = filmId;
                    ms.timeId = timeId;
                    ms.roomId = roomId;
                    ms.scheduleDate = scheduleDate;

                    new MovieScheduleService().Create(ms);

                    message = "Add Schedule Success!";
                }
                else
                {
                    message = "Schedule already exist!";
                }
            }
            else
            {
                message = "Schedule already exist!";
            }

            var obj = new
            {
                message = message
            };
            return Json(obj);
        }

        public bool checkAddableSchedule(int timeId, List<MovieSchedule> addedShowTime, List<ShowTime> allShowTime)
        {
            
            ShowTime aTime = allShowTime.Find(t => t.timeId == timeId);
            foreach (MovieSchedule schedule in addedShowTime)
            {
                ShowTime addedTime = allShowTime.Find(t => t.timeId == schedule.timeId);
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
                    return false;
                }
                else if (startTime < addedEndTime && endTime > addedEndTime)
                {
                    return false;
                }
            }
            return true;
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
