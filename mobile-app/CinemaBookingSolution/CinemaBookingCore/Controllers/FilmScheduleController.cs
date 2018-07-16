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
        public IActionResult Get(int filmId)
        {
            DateTime nowDate = DateTime.Now;

            List<FilmScheduleModel> schedule = new List<FilmScheduleModel>();

            for (int j = 0; j < NUMBER_DATE_RETURN; j++)
            {
                var tmpDate = new DateTime();
                var tmpNextDate = new DateTime();

                tmpDate = nowDate.AddDays(j);
                if (j != 0)
                {
                    tmpDate = nowDate.Date.AddDays(j);
                }
                tmpNextDate = tmpDate.AddDays(j + 1).Date;

                List<Cinema> cinemas = context.Cinema.Include(c => c.GroupCinema).ToList();

                List<DateScheduleModel> dateScheduleModels = new List<DateScheduleModel>();

                for (int i = 0; i < cinemas.Count(); i++)
                {
                    int index = i + 1;
                    var cinemaSchedules = context.MovieSchedule
                                               .Where(s => s.FilmId == filmId
                                                            && s.ScheduleDate >= tmpDate
                                                            && s.ScheduleDate < tmpNextDate
                                                            && s.Room.CinemaId == index)
                                               .Include(s => s.ShowTime)
                                               .ToList();

                    if (cinemaSchedules.Count() != 0)
                    {
                        ShowTimeListModel listChild = new ShowTimeListModel();
                        listChild.CinemaName = cinemas[i].CinemaName;
                        listChild.CinemaGroupName = cinemas[i].GroupCinema.Name;
                        listChild.ShowTimeChildModels = new List<ShowTimeChildModel>();
                        listChild.GroupCinemaLogo = cinemas[i].GroupCinema.LogoImg;

                        foreach (var item in cinemaSchedules)
                        {
                            Room room = context.Room.Where(r => r.RoomId == item.RoomId)
                                .Include(r => r.DigitalType)
                                .Include(r => r.Cinema).ThenInclude(r => r.GroupCinema)
                                .FirstOrDefault();

                            TypeOfSeat typeOfSeat = context.TypeOfSeat
                                .Where(t => t.GroupId == room.Cinema.GroupCinema.GroupId)
                                .FirstOrDefault();

                            ShowTimeChildModel child = new ShowTimeChildModel
                            {
                                TimeId = item.ShowTime.TimeId,
                                ScheduleId = item.ScheduleId,
                                Type = room.DigitalType.Name,
                                Price = typeOfSeat.Price.ToString(),
                                TimeStart = item.ShowTime.StartTime,
                                TimeEnd = item.ShowTime.EndTime,
                                FilmId = item.FilmId,
                                RoomId = item.RoomId,
                                GroupId = typeOfSeat.GroupId,
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

            return Ok(schedule);
        }

        [HttpGet("GetSchdeduleByCinemaId")]
        public IActionResult GetSchdeduleByCinemaId(int cinemaId)
        {
            DateTime nowDate = DateTime.Now;

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
                                                    .ToList());
                }

                List<DateScheduleModel> dateScheduleModels = new List<DateScheduleModel>();

                List<int> listFilmId = new List<int>();

                foreach (var scheduleItem in movieSchedules)
                {
                    int tmpFilmId = scheduleItem.FilmId;

                    if(listFilmId.Count() == 0)
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
    }

}
