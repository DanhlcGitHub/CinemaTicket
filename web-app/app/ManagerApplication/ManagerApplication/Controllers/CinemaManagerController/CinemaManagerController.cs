using ManagerApplication.Constant;
using ManagerApplication.Service;
using ManagerApplication.Utility;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
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
            string logoImg = new GroupCinemaServcie().FindByID(c.groupId).logoImg;
            if (!logoImg.Contains("http")) logoImg = serverPath + logoImg;
            var jsonObj = new
            {
                logoImg = logoImg,
                profileImg = c.profilePicture.Contains("http") ? c.profilePicture : (serverPath + c.profilePicture),
                name = c.cinemaName,
                address = c.cinemaAddress,
                email = c.email,
                phone = c.phone
            };
            return Json(jsonObj);
        }

        public JsonResult GetScheduleByDate(string cinemaIdStr, string selectedDateStr)
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

        public JsonResult IsEmptyScheduleNextDay(string cinemaIdStr, string selectedDateStr)
        {
            object obj = new
            {
                isEmpty = "true"
            };
            int cinemaId = Convert.ToInt32(cinemaIdStr);
            DateTime selectedDate = DateTime.Parse(selectedDateStr).AddDays(1);
            int totalSchedule = countScheduleInDay(cinemaId, selectedDate);
            if (totalSchedule != 0)
            {
                obj = new {
                    isEmpty = "false"
                };
            }
            return Json(obj);
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

        private int countScheduleInDay(int cinemaId, DateTime selectedDate)
        {
            int totalSchedule = 0;
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

                if(addedShowTime!=null)
                    totalSchedule += addedShowTime.Count;
            }
            return totalSchedule;
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

        public JsonResult SaveCustomScheduleNextDay()
        {
            string scheduleInfor = Request.Params["scheduleInfor"];
            JObject dataArrayObj = JObject.Parse(scheduleInfor);
            JArray dataArray = (JArray)dataArrayObj.GetValue("dataArray");
            foreach (JObject data in dataArray)
            {
                int roomId = (int)data.GetValue("roomId");
                //int filmId = (int)data.GetValue("filmId");
                string selectedDateStr = (string)data.GetValue("selectedDate");
                DateTime inputDate = DateTime.Parse(selectedDateStr).AddDays(1);

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

        public JsonResult basicAddSchedule(string filmIdStr, string timeIdStr, string roomIdStr, string scheduleDateStr)
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

            List<MovieSchedule> list = new MovieScheduleService().FindBy(ms => ms.roomId == roomId
                                                             && ms.scheduleDate == scheduleDate);
            if (list == null || list.Count == 0)
            {
                //lay tat ca suat chieu da add trong ngay
                DateTime beginOfDate = DateTime.Parse(inputedDate.Year + "-" + inputedDate.Month + "-" + inputedDate.Day + " 00:00");
                DateTime endOfDate = DateTime.Parse(inputedDate.Year + "-" + inputedDate.Month + "-" + inputedDate.Day + " 23:59");
                List<MovieSchedule> addedShowTime = new MovieScheduleService().FindBy(s => s.scheduleDate > beginOfDate
                                    && s.scheduleDate < endOfDate && s.roomId == roomId);
                if (checkAddableSchedule(timeId, addedShowTime, allShowTime))
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
        public JsonResult GetDashboardCommonData(string cinemaIdStr)
        {
            int cinemaId = Convert.ToInt32(cinemaIdStr);
            List<Film> filmList = new FilmService().FindBy(f => f.filmStatus != (int)FilmStatus.notAvailable);
            List<Room> roomList = new RoomService().FindBy(r => r.cinemaId == cinemaId);
            DateTime today = DateTime.Today;
            DateTime beginOfDate = DateTime.Parse(today.Year + "-" + today.Month + "-" + today.Day + " 00:00");
            DateTime endOfDate = DateTime.Parse(today.Year + "-" + today.Month + "-" + today.Day + " 23:59");
            int totalSchedule = 0;
            int totalTicket = 0;
            foreach (var aRoom in roomList)
            {
                List<MovieSchedule> addedSchedules = new MovieScheduleService().FindBy(s => s.scheduleDate > beginOfDate
                                    && s.scheduleDate < endOfDate && s.roomId == aRoom.roomId);
                totalSchedule += addedSchedules.Count;

                foreach (var schedule in addedSchedules)
                {
                    List<Ticket> tickets = new TicketService().FindBy(t => t.scheduleId == schedule.scheduleId && t.ticketStatus == TicketStatus.buyed);
                    if (tickets != null)
                        totalTicket += tickets.Count;
                }
            }

            var obj = new
            {
                showingMovie = filmList.Count,
                totalRoom = roomList.Count,
                totalSchedule = totalSchedule,
                totalTicket = totalTicket,
            };
            return Json(obj);
        }

        [HttpPost]
        public JsonResult GetWeeklyTicketData(string cinemaIdStr)
        {
            List<object> returnList = new List<object>();
            int cinemaId = Convert.ToInt32(cinemaIdStr);
            int groupId = (int)new CinemaService().FindByID(cinemaId).groupId;
            float price = 0;
            try
            {
                price = (float)new TypeOfSeatService().FindBy(t => t.groupId == groupId).FirstOrDefault().price;
            }
            catch (Exception)
            {

            }

            int todaySold = 0;
            int totalSold = 0;
            DateTime startOfWeek = DateTime.Today.AddDays(
              (int)CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek -
              (int)DateTime.Today.DayOfWeek);
            List<int> weeklyTicketList = new List<int>();
            List<DateTime> dates = new DateUtility().getSevenDateFromNow(startOfWeek);
            List<Room> roomList = new RoomService().FindBy(r => r.cinemaId == cinemaId);
            foreach (var date in dates)
            {
                int ticketInDay = getTicketSoldInDay(roomList, date);
                weeklyTicketList.Add(ticketInDay);
                totalSold += ticketInDay;
                if (date == DateTime.Today)
                    todaySold = ticketInDay;
            }
            var obj = new
            {
                infor = String.Format("{0:yyyy/MM/dd}", dates[0]) + " - " + String.Format("{0:yyyy/MM/dd}", dates[6]),
                price = price,
                todaySold = todaySold,
                totalSold = totalSold,
                weeklyTicketList = weeklyTicketList,
            };
            return Json(obj);
        }

        public JsonResult GetTop4Film(string cinemaIdStr)
        {
            int cinemaId = Convert.ToInt32(cinemaIdStr);
            DateTime startOfWeek = DateTime.Today.AddDays(
              (int)CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek -
              (int)DateTime.Today.DayOfWeek);
            List<Room> roomList = new RoomService().FindBy(r => r.cinemaId == cinemaId);
            FilmService filmService = new FilmService();
            List<Film> hotFilm = new List<Film>();
            List<Film> filmList = filmService.FindBy(f => f.filmStatus == (int)FilmStatus.showingMovie);//
            var dictionaryRank = new Dictionary<Film, int>();
            foreach (var item in filmList)
            {
                int ticketSold = getTicketSoldInWeekByFilm(roomList, startOfWeek, item);
                dictionaryRank.Add(item, ticketSold);
            }
            var items = from pair in dictionaryRank
                        orderby pair.Value descending
                        select pair;

            List<object> hightlightFilm = new List<object>();
            foreach (KeyValuePair<Film, int> pair in items.Take(4))
            {
                var obj = new
                {
                    filmId = pair.Key.filmId,
                    filmName = pair.Key.name,
                    tickSold = pair.Value,
                    dateRelease = String.Format("{0:dd/MM/yyyy}", pair.Key.dateRelease),
                };
                hightlightFilm.Add(obj);
            }
            return Json(hightlightFilm);
        }

        private int getTicketSoldInDay(List<Room> roomList, DateTime date)
        {
            int ticketInDay = 0;
            DateTime beginOfDate = DateTime.Parse(date.Year + "-" + date.Month + "-" + date.Day + " 00:00");
            DateTime endOfDate = DateTime.Parse(date.Year + "-" + date.Month + "-" + date.Day + " 23:59");
            foreach (var aRoom in roomList)
            {
                List<MovieSchedule> addedSchedules = new MovieScheduleService().FindBy(s => s.scheduleDate > beginOfDate
                                    && s.scheduleDate < endOfDate && s.roomId == aRoom.roomId);

                foreach (var schedule in addedSchedules)
                {
                    List<Ticket> tickets = new TicketService().FindBy(t => t.scheduleId == schedule.scheduleId && t.ticketStatus == TicketStatus.buyed);
                    if (tickets != null)
                        ticketInDay += tickets.Count;
                }
            }
            return ticketInDay;
        }

        private int getTicketSoldInWeekByFilm(List<Room> roomList, DateTime dateStart, Film aFilm)
        {
            int ticketInDay = 0;
            DateTime dateEnd = dateStart.AddDays(6);
            DateTime beginOfDate = DateTime.Parse(dateStart.Year + "-" + dateStart.Month + "-" + dateStart.Day + " 00:00");
            DateTime endOfDate = DateTime.Parse(dateEnd.Year + "-" + dateEnd.Month + "-" + dateEnd.Day + " 23:59");
            foreach (var aRoom in roomList)
            {
                List<MovieSchedule> addedSchedules = new MovieScheduleService().FindBy(s => s.scheduleDate > beginOfDate
                                    && s.scheduleDate < endOfDate && s.roomId == aRoom.roomId && s.filmId == aFilm.filmId);

                foreach (var schedule in addedSchedules)
                {
                    List<Ticket> tickets = new TicketService().FindBy(t => t.scheduleId == schedule.scheduleId && t.ticketStatus == TicketStatus.buyed);
                    if (tickets != null)
                        ticketInDay += tickets.Count;
                }
            }
            return ticketInDay;
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
