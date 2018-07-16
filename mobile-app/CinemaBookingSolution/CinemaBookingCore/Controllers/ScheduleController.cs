using CinemaBookingCore.Data;
using CinemaBookingCore.Data.Entities;
using CinemaBookingCore.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaBookingCore.Controllers
{
    //Schedule/setScheduleForGroupCinemas
    [Route("[controller]")]
    public class ScheduleController : Controller
    {

        public static int STATUS_FILM_NOW_SHOWING = 1;
        public static int NUMBER_BEGIN_RANDOM = 0;
        public static int NUMBER_DIVIDE_ROOMS = 2;

        private List<RoomModel> listRoomModel;

        private readonly CinemaBookingDBContext context;

        public ScheduleController(CinemaBookingDBContext context)
        {
            this.context = context;
        }

        [HttpGet("setScheduleForGroupCinemas")]
        public IActionResult SetScheduleForGroupCinemas()
        {
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                DateTime date = DateTime.Now.Date;
                List<Film> films = context.Film.Where(f => f.FilmStatus == STATUS_FILM_NOW_SHOWING).ToList();

                List<FilmModel> listFilmModel = new List<FilmModel>();

                List<ShowTime> showTimes = context.ShowTime.ToList();

                foreach (var film in films)
                {
                    FilmModel filmModel = new FilmModel
                    {
                        FilmId = film.FilmId,
                        IsSelect = false
                    };
                    listFilmModel.Add(filmModel);
                }

                for (int j = 0; j < 7; j++)
                {
                    DateTime tpmDate = date.AddDays(j);

                    List<Cinema> cinemas = context.Cinema.ToList();
                    foreach (var cinema in cinemas)
                    {
                        List<Room> rooms = context.Room.Where(r => r.CinemaId == cinema.CinemaId).ToList();

                        List<MovieSchedule> schedules = new List<MovieSchedule>();

                        Random random = new Random();

                        foreach (var room in rooms)
                        {
                            int indexOfFilm;
                            int countFilm = 0;
                            do
                            {
                                indexOfFilm = random.Next(0, films.Count());
                                if (countFilm == films.Count())
                                {
                                    ResetListFilm(listFilmModel);
                                }
                                countFilm++;
                            } while (listFilmModel[indexOfFilm].IsSelect != false);

                            int indexShowTime = random.Next(0, showTimes.Count());
                            ShowTime showTime = showTimes[indexShowTime];
                            DateTime scheduleDateTime = tpmDate.AddHours(showTime.StartTimeDouble);

                            MovieSchedule schedule = new MovieSchedule
                            {
                                RoomId = room.RoomId,
                                FilmId = films[indexOfFilm].FilmId,
                                TimeId = showTime.TimeId,
                                ScheduleDate = scheduleDateTime
                            };

                            String insertSchedule = "INSERT INTO MovieSchedule(filmId, timeId, roomId, scheduleDate)" +
                                "VALUES(" + schedule.FilmId + ", " + schedule.TimeId + ", " + schedule.RoomId + ", N'" + schedule.ScheduleDate + "');";

                            stringBuilder.Append(insertSchedule);
                            stringBuilder.Append(System.Environment.NewLine);
                            listFilmModel[indexOfFilm].IsSelect = true;
                        }
                    }
                }

                String filepath = "E:\\auto-schedule.txt";
                FileStream fs = new FileStream(filepath, FileMode.Create);
                StreamWriter sWriter = new StreamWriter(fs, Encoding.UTF8);
                sWriter.WriteLine("use CinemaBookingDB;");
                sWriter.WriteLine(stringBuilder.ToString());

                sWriter.Flush();
                fs.Close();

                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        public void ResetListFilm(List<FilmModel> films)
        {
            foreach (var film in films)
            {
                film.IsSelect = false;
            }
        }

        public void ResetStatusListRooms(List<Room> rooms)
        {
            listRoomModel = new List<RoomModel>();
            foreach (var room in rooms)
            {
                RoomModel roomModel = new RoomModel
                {
                    RoomId = room.RoomId,
                    IsSelected = false
                };

                listRoomModel.Add(roomModel);
            }
        }
    }
}
