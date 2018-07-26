using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CinemaBookingCore.Data.Entities;
using CinemaBookingCore.Data;
using CinemaBookingCore.Data.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;
using System.IO;
using System.Data.SqlClient;

namespace CinemaBookingCore.Controllers
{
    [Route("[controller]")]
    public class AccountController : Controller
    {

        private readonly CinemaBookingDBContext context;

        public AccountController(CinemaBookingDBContext context)
        {
            this.context = context;
        }

        [HttpGet("login")]
        public IActionResult Login(String username, String password)
        {
            UserAccount user = context.UserAccount.Where(u => u.UserId == username && u.UserPassword == password).FirstOrDefault();
            if (user == null)
            {
                user = new UserAccount();
            }

            return Ok(user);
        }


        public String getAndroidMessage(String title, String body, String image, String message, String accountId, String to)
        {
            MessageModel model = new MessageModel
            {
                to = to,
                notification = new NotificationModel
                {
                    title = title,
                    body = body
                },
                data = new Dictionary<String, String>
                {
                    { "image", image },
                    { "message", message },
                    { "accountId",  accountId}
                },
            };

            return JsonConvert.SerializeObject(model);

        }

        public String Send(String notification)
        {
            var fcmKey = "AIzaSyARHB_AnvZp-FkZOdU4R5wcFs9VMGwG0rY";
            var http = new HttpClient();
            http.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "key=" + fcmKey);

            http.DefaultRequestHeaders.TryAddWithoutValidation("content-length", notification.Length.ToString());
            var content = new StringContent(notification, System.Text.Encoding.UTF8, "application/json");

            var response = http.PostAsync("https://fcm.googleapis.com/fcm/send", content);

            return notification;
        }

        [HttpGet("notificationScheduleAuto")]
        public IActionResult NotificationScheduleAuto()
        {
            
            try
            {
                while (true)
                {
                    DateTime nowDate = DateTime.UtcNow;
                    //GMT +7
                    nowDate = nowDate.AddHours(7);

                    //add one hour
                    nowDate = nowDate.AddHours(1);

                    List<Ticket> tickets = context.Ticket.Where(t => t.MovieSchedule.ShowTime.StartTimeDouble == nowDate.Hour
                                                                    && t.MovieSchedule.ScheduleDate.Date == nowDate.Date
                                                                    && t.TicketStatus == "buyed")
                                                                    .Include(t => t.MovieSchedule).ThenInclude(ms => ms.ShowTime)
                                                                    .Include(t => t.BookingTicket).ThenInclude(bt => bt.Customer)
                                                                    .ToList();

                    List<int> listBookingTicketId = new List<int>();
                    foreach (var ticket in tickets)
                    {
                        int tmpId = (int)ticket.BookingId;
                        if (listBookingTicketId.IndexOf(tmpId) == -1)
                        {
                            listBookingTicketId.Add(tmpId);
                        }
                    }

                    List<int> listCustomerId = new List<int>();

                    foreach (var bookingId in listBookingTicketId)
                    {
                        int tmpId = tickets.Where(t => t.BookingId == bookingId).FirstOrDefault().BookingTicket.CustomerId;
                        if (listCustomerId.IndexOf(tmpId) == -1)
                        {
                            listCustomerId.Add(tmpId);
                        }
                    }

                    List<String> listAccountId = new List<String>();

                    foreach (var customerId in listCustomerId)
                    {
                        String tmpId = tickets.Where(t => t.BookingTicket.CustomerId == customerId).FirstOrDefault().BookingTicket.Customer.UserId;
                        if (listAccountId.IndexOf(tmpId) == -1)
                        {
                            listAccountId.Add(tmpId);
                        }
                    }

                    String title = "Đến giờ chiếu phim rồi nè.";
                    String body = "Mau đến rạp nào.";
                    String image = "https://s3.amazonaws.com/user-media.venngage.com/886214-19c392a6acb8bfa85f72fd6051c0284d.png";
                    String message = "nghich message";
                    String to = "/topics/schedule-coming-show-soon";

                    foreach (var accountId in listAccountId)
                    {
                        String notification = getAndroidMessage(title, body, image, message, accountId, to);

                        Send(notification);
                    }

                    System.Threading.Thread.Sleep(TimeSpan.FromMinutes(15));
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("register")]
        public IActionResult Register(String userId, String password, String email, String phone)
        {
            UserAccount user = context.UserAccount.Where(u => u.UserId == userId).FirstOrDefault();

            AccountModel accountModel = new AccountModel();
            if (user == null)
            {
                user = new UserAccount
                {
                    UserId = userId,
                    UserPassword = password,
                    Email = email,
                    Phone = phone
                };
                context.Add(user);
                context.SaveChanges();
                accountModel.IsExited = false;
            }
            else
            {
                accountModel.IsExited = true;
            }

            return Ok(accountModel);
        }

        [HttpGet("getAllOrderByAccountId")]
        public IActionResult GetAllOrderByAccountId(String accountId)
        {
            try
            {
                //List<Ticket> listTicket = context.Ticket.Where(t => t.BookingTicket.Customer.UserId == accountId)
                //                                         .Include(t => t.BookingTicket).ThenInclude(bt => bt.Customer)
                //                                         .Include(t => t.Seat)
                //                                         .Include(t => t.MovieSchedule).ThenInclude(ms => ms.Room).ThenInclude(r => r.DigitalType)
                //                                         .Include(t => t.MovieSchedule).ThenInclude(ms => ms.Room).ThenInclude(r => r.Cinema).ThenInclude(c => c.GroupCinema)
                //                                         .Include(t => t.MovieSchedule).ThenInclude(ms => ms.ShowTime)
                //                                         .Include(t => t.MovieSchedule).ThenInclude(ms => ms.Film)
                //                                         .ToList();

                List<BookingTicket> bookingTickets = context.BookingTicket.Where(b => b.Customer.UserId == accountId)
                    .Include(b => b.Customer)
                    .Include(b => b.Tickets)
                    .ToList();

                List<AccountPurchasedModel> accountPurchasedModels = new List<AccountPurchasedModel>();
                Ticket ticket = null;
                MovieSchedule movieSchedule = null;
                Film film = null;
                Room room = null;
                ShowTime showTime = null;
                Cinema cinema = null;
                GroupCinema groupCinema = null;


                foreach (var bookingTicket in bookingTickets)
                {
                    List<Ticket> tickets = context.Ticket.Where(t => t.BookingId == bookingTicket.BookingId)
                        .Include(t => t.Seat)
                        .Include(t => t.MovieSchedule).ThenInclude(ms => ms.Room).ThenInclude(r => r.DigitalType)
                        .Include(t => t.MovieSchedule).ThenInclude(ms => ms.Room).ThenInclude(r => r.Cinema).ThenInclude(c => c.GroupCinema)
                        .Include(t => t.MovieSchedule).ThenInclude(ms => ms.ShowTime)
                        .Include(t => t.MovieSchedule).ThenInclude(ms => ms.Film)
                        .ToList();
                    Double totalPrice = 0;

                    String stringSeats = "";
                    foreach (var item in tickets)
                    {
                        Seat seat = item.Seat;
                        Room roomForSeat = item.MovieSchedule.Room;

                        Char character = 'A';
                        List<Char> resultAbc = new List<Char>();
                        int ascii = (int)character;

                        for (int i = 0; i < roomForSeat.MatrixSizeX; i++)
                        {
                            Char tmp = (char)(ascii + i);
                            resultAbc.Add(tmp);
                        }

                        String position = resultAbc[seat.Px].ToString() + (seat.Py + 1);

                        stringSeats += position + " ";
                        totalPrice += item.Price;
                    }

                    ticket = tickets.FirstOrDefault();
                    if (ticket != null)
                    {
                        movieSchedule = ticket.MovieSchedule;
                        if (movieSchedule != null)
                        {
                            film = ticket.MovieSchedule.Film;
                            showTime = ticket.MovieSchedule.ShowTime;

                            room = ticket.MovieSchedule.Room;
                            DigitalType digitalType = room.DigitalType;

                            if (room != null)
                            {
                                cinema = room.Cinema;
                                if (cinema != null)
                                {
                                    groupCinema = cinema.GroupCinema;
                                }
                                AccountPurchasedModel accountPurchasedModel = new AccountPurchasedModel
                                {
                                    BookingTicketId = bookingTicket.BookingId,
                                    CinemaName = cinema.CinemaName,
                                    GroupCinemaName = groupCinema.Name,
                                    Date = movieSchedule.ScheduleDate.ToString("dd/MM/yyyy"),
                                    FilmImage = film.AdditionPicture,
                                    FilmName = film.Name,
                                    RoomName = room.Name,
                                    ShowTime = showTime.StartTime,
                                    ScheduleId = movieSchedule.ScheduleId,
                                    RoomId = room.RoomId,
                                    DigType = digitalType.Name,
                                    Restricted = film.Restricted.ToString(),
                                    TotalPrice = totalPrice,
                                    StringSeats = stringSeats,
                                    Email = bookingTicket.Customer.Email,
                                    Phone = bookingTicket.Customer.Phone
                                };
                                accountPurchasedModels.Add(accountPurchasedModel);
                            }
                        }
                    }
                }
                return Ok(accountPurchasedModels);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
