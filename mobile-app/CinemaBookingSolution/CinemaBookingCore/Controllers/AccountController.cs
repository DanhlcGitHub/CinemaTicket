using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CinemaBookingCore.Data.Entities;
using CinemaBookingCore.Data;
using CinemaBookingCore.Data.Models;

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

            return Ok(user);
        }

        [HttpGet("getAllOrderByAccountId")]
        public IActionResult GetAllOrderByAccountId(string accountId)
        {
            try
            {
                List<Customer> customers = context.Customer.Where(c => c.UserId == accountId).ToList();
                List<BookingTicket> bookingTickets = null;
                List<AccountPurchasedModel> accountPurchasedModels = new List<AccountPurchasedModel>();
                Ticket ticket = null;
                MovieSchedule movieSchedule = null;
                Film film = null;
                Room room = null;
                ShowTime showTime = null;
                Cinema cinema = null;
                GroupCinema groupCinema = null;

                foreach (var customer in customers)
                {
                    bookingTickets = context.BookingTicket.Where(b => b.CustomerId == customer.CustomerId).ToList();

                    foreach (var bookingTicket in bookingTickets)
                    {
                        List<Ticket> tickets = context.Ticket.Where(t => t.BookingId == bookingTicket.BookingId).ToList();
                        Double totalPrice = 0;


                        String stringSeats = "";
                        foreach (var item in tickets)
                        {
                            Seat seat = context.Seat.Where(s => s.SeatId == item.SeatId).FirstOrDefault();
                            Room roomForSeat = context.Room.Where(r => r.RoomId == seat.RoomId).FirstOrDefault();

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
                            movieSchedule = context.MovieSchedule.Where(m => m.ScheduleId == ticket.ScheduleId).FirstOrDefault();
                            if (movieSchedule != null)
                            {
                                film = context.Film.Where(f => f.FilmId == movieSchedule.FilmId).FirstOrDefault();
                                showTime = context.ShowTime.Where(s => s.TimeId == movieSchedule.TimeId).FirstOrDefault();

                                room = context.Room.Where(r => r.RoomId == movieSchedule.RoomId).FirstOrDefault();
                                DigitalType digitalType = context.DigitalType.Where(d => d.DigTypeId == room.DigTypeId).FirstOrDefault();

                                if (room != null)
                                {
                                    cinema = context.Cinema.Where(c => c.CinemaId == room.CinemaId).FirstOrDefault();
                                    if (cinema != null)
                                    {
                                        groupCinema = context.GroupCinema.Where(g => g.GroupId == cinema.GroupId).FirstOrDefault();
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
                                        Email = customer.Email,
                                        Phone = customer.Phone
                                    };
                                    accountPurchasedModels.Add(accountPurchasedModel);
                                }
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
