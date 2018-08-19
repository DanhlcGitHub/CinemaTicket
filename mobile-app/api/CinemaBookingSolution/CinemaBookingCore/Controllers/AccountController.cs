using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using CinemaBookingCore.Data.Entities;
using CinemaBookingCore.Data;
using CinemaBookingCore.Data.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http;
using CinemaBookingCore.Utility;
using System.Threading.Tasks;
using CinemaTicket.Utility;
using CinemaBookingCore.Constant;

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
            String passwordEncript = EncryptUtility.EncryptString(password);
            UserAccount user = context.UserAccount.Where(u => u.UserId == username && u.UserPassword == passwordEncript).FirstOrDefault();
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

        //Account/notificationScheduleAuto
        [HttpGet("notificationScheduleAuto")]
        public IActionResult NotificationScheduleAuto()
        {
            try
            {
                while (true)
                {
                    DateTime nowDate = DateTime.UtcNow;

                    //GMT +7 and + them 1h de tim kiem lich chieu sau 1 gio
                    nowDate = nowDate.AddHours(8);

                    //5 phut sau
                    DateTime nextFiveMinuteDateTime = nowDate.AddMinutes(5);

                    List<Ticket> tickets = context.Ticket.Where(t => t.MovieSchedule.ShowTime.StartTimeDouble == nowDate.Hour
                                                                    && t.MovieSchedule.ScheduleDate >= nowDate
                                                                    && t.MovieSchedule.ScheduleDate < nextFiveMinuteDateTime
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

                    System.Threading.Thread.Sleep(TimeSpan.FromMinutes(5));
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

            String passwordEncript = EncryptUtility.EncryptString(password);

            if (user == null)
            {
                user = new UserAccount
                {
                    UserId = userId,
                    UserPassword = passwordEncript,
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

                                //GMT +7
                                DateTime nowDate = DateTime.UtcNow.AddHours(7);

                                bool isCanChange = false;

                                //if now + 4h then customer can change ticket and resell ticket
                                if (nowDate.AddHours(4) < ticket.MovieSchedule.ScheduleDate)
                                {
                                    isCanChange = true;
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
                                    Restricted = film.Restricted,
                                    TotalPrice = totalPrice,
                                    StringSeats = stringSeats,
                                    Email = bookingTicket.Customer.Email,
                                    Phone = bookingTicket.Customer.Phone,
                                    IsCanChange = isCanChange
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



        [HttpPut("ConfirmChangeTicket")]
        public IActionResult ConfirmChangeTicket(int ticketId, int scheduleId, int seatId)
        {
            try
            {
                Ticket ticket = context.Ticket.Where(t => t.TicketId == ticketId).Include(t => t.BookingTicket).FirstOrDefault();

                var bookingTicketOld = ticket.BookingTicket;
                bookingTicketOld.Quantity -= 1;


                BookingTicket bookingTicket = new BookingTicket
                {
                    CustomerId = ticket.BookingTicket.CustomerId,
                    Quantity = 1,
                    BookingDate = DateTime.UtcNow.AddHours(7)
                };
                context.Add(bookingTicket);
                context.SaveChanges();

                String newBookingTicketPaymentCode = bookingTicket.BookingId + RandomUtility.RandomString(9);

                bookingTicket.PaymentCode = newBookingTicketPaymentCode;
                context.Update(bookingTicket);

                if (ticket != null)
                {
                    ticket.BookingId = bookingTicket.BookingId;
                    ticket.SeatId = seatId;
                    ticket.TicketStatus = "changed";
                    ticket.ScheduleId = scheduleId;

                    MovieSchedule movieSchedule = context.MovieSchedule
                        .Where(ms => ms.ScheduleId == scheduleId)
                        .Include(ms => ms.Film)
                        .Include(ms => ms.Room).ThenInclude(r => r.Cinema)
                        .FirstOrDefault();

                    Seat seat = context.Seat.Where(s => s.SeatId == seatId).FirstOrDefault();
                    String newTicketPaymentCode = ticket.TicketId + RandomUtility.RandomString(9);

                    ticket.PaymentCode = newTicketPaymentCode;
                    String cinemaName = movieSchedule.Room.Cinema.CinemaName;

                    string content = "Quý khách đã đổi lại vé thành công!";
                    content += "Bạn đã đổi lại 1 vé \n";
                    content += "Tại " + cinemaName + "\n";
                    content += "Mã đơn hàng của bạn là " + newBookingTicketPaymentCode + "\n";
                    content += "Phim " + movieSchedule.Film.Name + "\n";
                    content += ". Ghế: " + ConstantArray.Alphabet[(int)seat.Py] + "" + ((int)seat.Px + 1) +
                                "- Mã vé: " + newTicketPaymentCode + "\n";
                    string mailSubject = "CinemaBookingTicket - Đổi lại vé thành công " + cinemaName;

                    MailUtility.SendEmail(mailSubject, content, ticket.BookingTicket.Customer.Email);

                    context.Update(ticket);
                    context.SaveChanges();
                }

                return Ok(ticket);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
