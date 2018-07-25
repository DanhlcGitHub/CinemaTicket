using CinemaBookingCore.Data;
using CinemaBookingCore.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CinemaBookingCore.Data.Models;

namespace CinemaBookingCore.Controllers
{
    [Route("ticket")]
    public class TicketController : Controller
    {
        private readonly CinemaBookingDBContext context;

        public TicketController(CinemaBookingDBContext context)
        {
            this.context = context;
        }

        [HttpGet("GetAllTicketByBookingTicketId")]
        public IActionResult GetAllTicketByBookingTicketId(int bookingTicketId)
        {
            try
            {
                List<Ticket> tickets = context.Ticket.Where(t => t.BookingId == bookingTicketId && t.MovieSchedule.ScheduleDate > DateTime.Now)
                                                        .Include(t => t.Seat)
                                                        .Include(t => t.MovieSchedule).ThenInclude(ms => ms.Room).ThenInclude(r => r.Cinema)
                                                        .Include(t => t.MovieSchedule).ThenInclude(ms => ms.Film)
                                                        .ToList();
                List<TicketModel> ticketModels = new List<TicketModel>();

                foreach (var ticket in tickets)
                {
                    Seat seat = ticket.Seat;
                    Room roomForSeat = ticket.MovieSchedule.Room;

                    Char character = 'A';
                    List<Char> resultAbc = new List<Char>();
                    int ascii = (int)character;

                    for (int i = 0; i < roomForSeat.MatrixSizeX; i++)
                    {
                        Char tmp = (char)(ascii + i);
                        resultAbc.Add(tmp);
                    }

                    String position = resultAbc[seat.Py].ToString() + (seat.Px + 1);

                    TimeSpan span = ticket.MovieSchedule.ScheduleDate.Subtract(DateTime.Now);

                    TicketModel ticketModel = new TicketModel
                    {
                        BookingId = ticket.BookingId,
                        Price = ticket.Price,
                        QrCode = ticket.QrCode,
                        ScheduleId = ticket.ScheduleId,
                        SeatId = ticket.SeatId,
                        TicketId = ticket.TicketId,
                        TicketStatus = ticket.TicketStatus,
                        SeatPosition = position,
                        CinemaId = roomForSeat.CinemaId,
                        IndexDate = (int) span.TotalDays,
                        FilmId = ticket.MovieSchedule.FilmId
                    };

                    ticketModels.Add(ticketModel);
                }

                return Ok(ticketModels);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut("resellTicket")]
        public IActionResult ResellTicket(int ticketId)
        {
            try
            {
                Ticket ticket = context.Ticket.Where(t => t.TicketId == ticketId).FirstOrDefault();
                if (ticket != null)
                {
                    ticket.TicketStatus = "resell";
                    context.SaveChanges();
                }


                return Ok(ticket);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut("cancelResellTicket")]
        public IActionResult CancelResellTicket(int ticketId)
        {
            try
            {
                Ticket ticket = context.Ticket.Where(t => t.TicketId == ticketId).FirstOrDefault();
                if (ticket != null)
                {
                    ticket.TicketStatus = "buyed";
                    context.SaveChanges();
                }


                return Ok(ticket);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut("confirmResellTicket")]
        public IActionResult ConfirmResellTicket(int ticketId, String email)
        {
            try
            {
                Customer customer = context.Customer.Where(u => u.Email == email).FirstOrDefault();

                if (customer == null)
                {
                    customer = new Customer
                    {
                        Email = email
                    };
                    context.SaveChanges();
                }

                DateTime nowDate = DateTime.UtcNow.AddHours(7);

                BookingTicket bookingTicket = new BookingTicket
                {
                    BookingDate = nowDate,
                    Quantity = 1,
                    CustomerId = customer.CustomerId
                };
                context.Add(bookingTicket);
                context.SaveChanges();

                Ticket ticket = context.Ticket.Where(t => t.TicketId == ticketId).FirstOrDefault();
                if (ticket != null)
                {
                    ticket.BookingId = bookingTicket.BookingId;
                    ticket.TicketStatus = "buyed";
                    context.SaveChanges();
                }

                return Ok(ticket);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut("confirmChangeTicket")]
        public IActionResult ConfirmChangeTicket(int ticketId, int scheduleId, int seatId)
        {
            try
            {
                var ticket = context.Ticket.Where(t => t.TicketId == ticketId).Include(t => t.BookingTicket).FirstOrDefault();

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

                if(ticket != null)
                {
                    ticket.BookingId = bookingTicket.BookingId;
                    ticket.SeatId = seatId;
                    ticket.ScheduleId = scheduleId;
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
