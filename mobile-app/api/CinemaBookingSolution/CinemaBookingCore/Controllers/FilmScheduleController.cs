using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CinemaBookingCore.Data;
using CinemaBookingCore.Data.Models;
using CinemaBookingCore.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CinemaBookingCore.Controllers
{
    [Route("[controller]")]
    public class FilmScheduleController : Controller
    {
        private static int NUMBER_DATE_RETURN = 6;
        private readonly CinemaBookingDBContext context;

        public FilmScheduleController(CinemaBookingDBContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public List<FilmScheduleModel> Get(int filmId, int indexDate)
        {
            DateTime nowDate = DateTime.UtcNow;
            nowDate = nowDate.AddHours(7);

            List<FilmScheduleModel> schedule = new List<FilmScheduleModel>();

            for (int j = 0; j < NUMBER_DATE_RETURN; j++)
            {
                var tmpDate = new DateTime();
                var tmpNextDate = new DateTime();
                if (indexDate != 0)
                {
                    tmpDate = nowDate.Date.AddDays(j);
                }
                else
                {
                    tmpDate = nowDate.AddDays(j);
                }

                tmpNextDate = nowDate.AddDays(j +1).Date;

                List<DateScheduleModel> dateScheduleModels = new List<DateScheduleModel>();

                if (j == indexDate)
                {
                    var cinemaSchedules = context.MovieSchedule
                                              .Where(s => s.FilmId == filmId
                                                     && s.ScheduleDate >= tmpDate
                                                     && s.ScheduleDate < tmpNextDate
                                                     )
                                                     .Include(s => s.Film)
                                                     .Include(s => s.Room).ThenInclude(r => r.Cinema).ThenInclude(c => c.GroupCinema).ThenInclude(g => g.TypeOfSeats)
                                                     .Include(s => s.Room).ThenInclude(r => r.DigitalType)
                                              .Include(s => s.ShowTime)
                                              .ToList();

                    if (cinemaSchedules.Count() != 0)
                    {
                        List<int> listCinemaId = new List<int>();

                        foreach (var cinemaSchedule in cinemaSchedules)
                        {
                            var cinema = cinemaSchedule.Room.Cinema;
                            int index = listCinemaId.IndexOf(cinema.CinemaId);

                            if (index == -1)
                            {
                                listCinemaId.Add(cinema.CinemaId);
                            }

                        }

                        foreach (var id in listCinemaId)
                        {

                            var cinema = context.Cinema.Where(c => c.CinemaId == id).FirstOrDefault();

                            ShowTimeListModel listChild = new ShowTimeListModel
                            {
                                CinemaName = cinema.CinemaName,
                                CinemaGroupName = cinema.GroupCinema.Name,
                                ShowTimeChildModels = new List<ShowTimeChildModel>(),
                                GroupCinemaLogo = cinema.GroupCinema.LogoImg
                            };

                            var schedulesByCinemaId = cinemaSchedules.Where(s => s.Room.CinemaId == cinema.CinemaId).OrderBy(s => s.ShowTime.StartTimeDouble).ToList();

                            foreach (var item in schedulesByCinemaId)
                            {
                                Room room = item.Room;

                                ShowTimeChildModel child = new ShowTimeChildModel
                                {
                                    TimeId = item.ShowTime.TimeId,
                                    ScheduleId = item.ScheduleId,
                                    Type = room.DigitalType.Name,
                                    Price = cinema.GroupCinema.TypeOfSeats.FirstOrDefault().Price.ToString(),
                                    TimeStart = item.ShowTime.StartTime,
                                    TimeEnd = item.ShowTime.EndTime,
                                    FilmId = item.FilmId,
                                    RoomId = item.RoomId,
                                    GroupId = cinema.GroupId,
                                    Col = room.MatrixSizeY,
                                    Row = room.MatrixSizeX
                                };

                                listChild.ShowTimeChildModels.Add(child);
                            }

                            DateScheduleModel dateScheduleModel = new DateScheduleModel
                            {
                                ShowTimeListModel = listChild
                            };
                            dateScheduleModels.Add(dateScheduleModel);
                        }
                    }
                }

                String dayofWeek = "";
                switch (tmpDate.DayOfWeek)
                {
                    case DayOfWeek.Sunday:
                        dayofWeek = "CN";
                        break;
                    case DayOfWeek.Monday:
                        dayofWeek = "T2";
                        break;
                    case DayOfWeek.Tuesday:
                        dayofWeek = "T3";
                        break;
                    case DayOfWeek.Wednesday:
                        dayofWeek = "T4";
                        break;
                    case DayOfWeek.Thursday:
                        dayofWeek = "T5";
                        break;
                    case DayOfWeek.Friday:
                        dayofWeek = "T6";
                        break;
                    case DayOfWeek.Saturday:
                        dayofWeek = "T7";
                        break;
                }

                FilmScheduleModel filmInDay = new FilmScheduleModel
                {
                    Date = tmpDate.Date.ToString(),
                    Day = tmpDate.Day.ToString(),
                    DateOfWeek = dayofWeek,
                    DateScheduleModels = dateScheduleModels
                };
                schedule.Add(filmInDay);
            }

            return schedule;
        }

        [HttpGet("GetSchdeduleByCinemaId")]
        public IActionResult GetSchdeduleByCinemaId(int cinemaId, int indexDate)
        {
            DateTime nowDate = DateTime.UtcNow;
            nowDate = nowDate.AddHours(7);

            List<FilmScheduleModel> schedule = new List<FilmScheduleModel>();

            Cinema cinema = context.Cinema.Where(c => c.CinemaId == cinemaId)
                                                .Include(c => c.GroupCinema)
                                                .ThenInclude(g => g.TypeOfSeats)
                                                .FirstOrDefault();

            int groupId = cinema.GroupCinema.GroupId;

            String basePrice = cinema.GroupCinema.TypeOfSeats.FirstOrDefault().Price.ToString();

            List<Room> rooms = context.Room.Where(r => r.CinemaId == cinema.CinemaId)
                                            .Include(r => r.DigitalType)
                                            .ToList();

            for (int j = 0; j < NUMBER_DATE_RETURN; j++)
            {
                var tmpDate = new DateTime();
                var tmpNextDate = new DateTime();

                tmpDate = nowDate.AddDays(j);

                if (j != 0)
                {
                    tmpDate = nowDate.Date.AddDays(j);
                }
                tmpNextDate = nowDate.AddDays(j + 1).Date;
                List<DateScheduleModel> dateScheduleModels = new List<DateScheduleModel>();

                if (j == indexDate)
                {
                    List<MovieSchedule> movieSchedules = new List<MovieSchedule>();

                    foreach (var room in rooms)
                    {
                        int roomId = room.RoomId;
                        movieSchedules.AddRange(context.MovieSchedule
                                                        .Where(
                                                                ms => ms.RoomId == roomId
                                                                && ms.ScheduleDate >= tmpDate
                                                                && ms.ScheduleDate < tmpNextDate
                                                              ).Include(s => s.ShowTime)
                                                              .Include(ms => ms.Film)
                                                              .OrderBy(ms => ms.ShowTime.StartTimeDouble)
                                                        .ToList());
                    }

                    List<int> listFilmId = new List<int>();

                    foreach (var scheduleItem in movieSchedules)
                    {
                        int tmpFilmId = scheduleItem.FilmId;

                        if (listFilmId.Count() == 0)
                        {
                            listFilmId.Add(tmpFilmId);
                        }
                        else
                        {
                            bool alreadyExist = listFilmId.Contains(tmpFilmId);
                            if (!alreadyExist)
                            {
                                listFilmId.Add(tmpFilmId);
                            }
                        }
                    }

                    foreach (var filmId in listFilmId)
                    {
                        ShowTimeListModel listChild = new ShowTimeListModel();

                        List<MovieSchedule> tmpSchedule = movieSchedules.Where(ms => ms.FilmId == filmId).ToList();
                        listChild.CinemaGroupName = cinema.GroupCinema.Name;
                        listChild.CinemaName = cinema.CinemaName;
                        listChild.FilmImg = tmpSchedule.FirstOrDefault().Film.AdditionPicture;
                        listChild.FilmName = tmpSchedule.FirstOrDefault().Film.Name;

                        List<ShowTimeChildModel> showTimeChildModels = new List<ShowTimeChildModel>();
                        foreach (var item in tmpSchedule)
                        {
                            ShowTimeChildModel showTimeChildModel = new ShowTimeChildModel
                            {
                                TimeId = item.ShowTime.TimeId,
                                ScheduleId = item.ScheduleId,
                                Type = item.Room.DigitalType.Name,
                                Price = basePrice,
                                TimeStart = item.ShowTime.StartTime,
                                TimeEnd = item.ShowTime.EndTime,
                                FilmId = item.FilmId,
                                RoomId = item.RoomId,
                                GroupId = groupId,
                                Col = item.Room.MatrixSizeY,
                                Row = item.Room.MatrixSizeX
                            };
                            showTimeChildModels.Add(showTimeChildModel);
                        }

                        listChild.ShowTimeChildModels = showTimeChildModels;

                        DateScheduleModel dateScheduleModel = new DateScheduleModel
                        {

                            ShowTimeListModel = listChild
                        };
                        dateScheduleModels.Add(dateScheduleModel);
                    }
                }

                String dayofWeek = "";
                switch (tmpDate.DayOfWeek)
                {
                    case DayOfWeek.Sunday:
                        dayofWeek = "CN";
                        break;
                    case DayOfWeek.Monday:
                        dayofWeek = "T2";
                        break;
                    case DayOfWeek.Tuesday:
                        dayofWeek = "T3";
                        break;
                    case DayOfWeek.Wednesday:
                        dayofWeek = "T4";
                        break;
                    case DayOfWeek.Thursday:
                        dayofWeek = "T5";
                        break;
                    case DayOfWeek.Friday:
                        dayofWeek = "T6";
                        break;
                    case DayOfWeek.Saturday:
                        dayofWeek = "T7";
                        break;
                }

                FilmScheduleModel schdeduleInDay = new FilmScheduleModel
                {
                    Date = tmpDate.Date.ToString(),
                    Day = tmpDate.Day.ToString(),
                    DateOfWeek = dayofWeek,
                    DateScheduleModels = dateScheduleModels
                };
                schedule.Add(schdeduleInDay);
            }
            return Ok(schedule);
        }

        [HttpGet("GetSchdeduleForChangeTicket")]
        public IActionResult GetSchdeduleForChangeTicket(int cinemaId, int indexDate, int filmId, int scheduleId)
        {
            DateTime nowDate = DateTime.UtcNow;
            nowDate = nowDate.AddHours(7);

            List<FilmScheduleModel> schedule = new List<FilmScheduleModel>();

            Cinema cinema = context.Cinema.Where(c => c.CinemaId == cinemaId)
                                                .Include(c => c.GroupCinema).ThenInclude(g => g.TypeOfSeats)
                                                .Include(c => c.Rooms).ThenInclude(r => r.DigitalType)
                                                .Include(c => c.Rooms).ThenInclude(r => r.MovieSchedules).ThenInclude(s => s.ShowTime)
                                                .Include(c => c.Rooms).ThenInclude(r => r.MovieSchedules).ThenInclude(s => s.Film)
                                                .FirstOrDefault();

            int groupId = cinema.GroupCinema.GroupId;

            String basePrice = cinema.GroupCinema.TypeOfSeats.FirstOrDefault().Price.ToString();

            var rooms = cinema.Rooms;

            var tmpDate = new DateTime();
            var tmpNextDate = new DateTime();

            if (indexDate != 0)
            {
                tmpDate = nowDate.Date.AddDays(indexDate);
            }
            else
            {
                tmpDate = nowDate.AddDays(indexDate);
            }

            tmpNextDate = nowDate.AddDays(indexDate + 1).Date;

            List<DateScheduleModel> dateScheduleModels = new List<DateScheduleModel>();


            List<MovieSchedule> movieSchedules = new List<MovieSchedule>();

            foreach (var room in rooms)
            {
                movieSchedules.AddRange(room.MovieSchedules.Where(ms => ms.FilmId == filmId
                                                                  && ms.ScheduleId != scheduleId 
                                                                  && ms.ScheduleDate >= tmpDate.AddHours(4)
                                                                  && ms.ScheduleDate < tmpNextDate
                                                                 ).OrderBy(ms => ms.ShowTime.StartTimeDouble));
            }

            ShowTimeListModel listChild = new ShowTimeListModel();

            listChild.CinemaGroupName = cinema.GroupCinema.Name;
            listChild.CinemaName = cinema.CinemaName;

            List<ShowTimeChildModel> showTimeChildModels = new List<ShowTimeChildModel>();
            foreach (var item in movieSchedules)
            {
                ShowTimeChildModel showTimeChildModel = new ShowTimeChildModel
                {
                    TimeId = item.ShowTime.TimeId,
                    ScheduleId = item.ScheduleId,
                    Type = item.Room.DigitalType.Name,
                    Price = basePrice,
                    TimeStart = item.ShowTime.StartTime,
                    TimeEnd = item.ShowTime.EndTime,
                    FilmId = item.FilmId,
                    RoomId = item.RoomId,
                    GroupId = groupId,
                    Col = item.Room.MatrixSizeY,
                    Row = item.Room.MatrixSizeX,
                    Datetime = item.ScheduleDate.ToString()
                };
                showTimeChildModels.Add(showTimeChildModel);
            }

            listChild.ShowTimeChildModels = showTimeChildModels;

            return Ok(listChild);
        }
    }

}
