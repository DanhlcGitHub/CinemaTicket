using CinemaTicket.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CinemaTicket.Controllers
{
    public class ChooseTicketController : Controller
    {
        //
        // GET: /ChooseTicket/

        [HttpPost]
        public JsonResult GetChooseTicketData(string scheduleId)
        {
            MovieSchedule schedule = new MovieScheduleService().FindByID(Convert.ToInt32(scheduleId));
            Film aFilm = new FilmService().FindByID(schedule.filmId);
            Room room = new RoomService().FindByID(schedule.roomId);
            ShowTime time = new ShowTimeService().FindByID(schedule.timeId);
            Cinema cinema = new CinemaService().FindByID(room.cinemaId);
            GroupCinema groupCinema = new GroupCinemaServcie().FindByID(cinema.groupId);

            string serverPath = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            string groupCinemaImg = serverPath + groupCinema.logoImg;
            if (groupCinema.logoImg.Contains("http"))
            {
                groupCinemaImg = groupCinema.logoImg;
            }
            string filmAdditionPicture = "";
            if (aFilm.additionPicture != null)
            {
                filmAdditionPicture = aFilm.additionPicture.Split(';')[0];
                if (!filmAdditionPicture.Contains("http"))
                {
                    filmAdditionPicture = serverPath + filmAdditionPicture;
                }
            }
            else
            {
                filmAdditionPicture = "https://www.valmorgan.com.au/wp-content/uploads/2016/06/default-movie-1-3.jpg";
            }

            var obj = new
            {
                filmId = aFilm.filmId,
                filmName = aFilm.name,
                releaseDate = String.Format("{0:dd/MM/yyyy}", aFilm.dateRelease),
                img = filmAdditionPicture,
                length = aFilm.filmLength,
                restricted = aFilm.restricted,
                imdb = aFilm.imdb,
                digitalType = "2d",
                groupCinemaImg = groupCinemaImg,
                cinemaName = cinema.cinemaName,
                cinemaAddress = cinema.cinemaAddress,
                roomId = room.roomId,
                roomName = room.name,
                startTime = time.startTime,
                date = schedule.scheduleDate == DateTime.Today ? "Hôm nay" : String.Format("{0:dd/MM/yyyy}", schedule.scheduleDate),
                typeOfSeats = new TypeOfSeatService().FindBy(t => t.groupId == groupCinema.GroupId).
                                                      Select(s =>new {
                                                             typeId = s.typeSeatId,
                                                             typeName = s.typeName,
                                                             price = s.price,
                                                             available = 10, // truy van boo
                                                             userChoose = 1,
                                                      }),                                                                           
                seatAvailable = 100,

            };
            return Json(obj);
        }

    }
}
