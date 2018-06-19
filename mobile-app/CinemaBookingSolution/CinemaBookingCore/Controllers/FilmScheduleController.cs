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
        private readonly CinemaBookingDBContext context;

        public FilmScheduleController(CinemaBookingDBContext context)
        {
            this.context = context;
        }


        [HttpGet]
        public IActionResult Get(int filmId)
        {
            DateTime dateInput = DateTime.Parse("2018-06-08T00:00:00");

            List<FilmScheduleModel> schedule = new List<FilmScheduleModel>();

            for (int j = 0; j < 6; j++)
            {
                var tmpDate = new DateTime();
                switch (j)
                {
                    case 0:
                        tmpDate = dateInput;
                        break;
                    case 1:
                        tmpDate = dateInput.AddDays(1);
                        break;
                    case 2:
                        tmpDate = dateInput.AddDays(2);
                        break;
                    case 3:
                        tmpDate = dateInput.AddDays(3);
                        break;
                    case 4:
                        tmpDate = dateInput.AddDays(4);
                        break;
                    case 5:
                        tmpDate = dateInput.AddDays(5);
                        break;
                }
                List<Cinema> cinemas = context.Cinema.Include(c => c.GroupCinema).ToList();

                List<DateScheduleModel> result = new List<DateScheduleModel>();

                for (int i = 0; i < cinemas.Count(); i++)
                {
                    int index = i + 1;
                    var cinemaSchedules = context.MovieSchedule
                                               .Where(s => s.FilmId == filmId
                                                            && s.ScheduleDate == tmpDate
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
                        result.Add(dateScheduleModel);
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
                    DateScheduleModels = result

                };
                schedule.Add(filmInDay);
            }

            return Ok(schedule);
        }
    }

}
