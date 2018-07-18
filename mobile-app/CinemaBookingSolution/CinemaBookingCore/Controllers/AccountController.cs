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
        public IActionResult Login(string username, string password)
        {
            UserAccount user = context.UserAccount.Where(u => u.UserId == username && u.UserPassword == password).FirstOrDefault();
            if (user == null)
            {
                user = new UserAccount();
            }

            return Ok(user);
        }

        [HttpGet("register")]
        public IActionResult Register(string userId, string password, string email, string phone)
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
        public IActionResult GetAllOrderByAccountId(string accountId)
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
