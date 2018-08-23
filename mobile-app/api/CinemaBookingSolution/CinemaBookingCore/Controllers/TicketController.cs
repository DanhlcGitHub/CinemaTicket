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
using CinemaTicket.Utility;
using CinemaBookingCore.Constant;

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
                List<Ticket> tickets = context.Ticket.Where(t => t.BookingId == bookingTicketId)
                                                        .Include(t => t.Seat)
                                                        .Include(t => t.MovieSchedule).ThenInclude(ms => ms.Room)
                                                            .ThenInclude(r => r.Cinema).ThenInclude(c => c.GroupCinema)
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

                    String position = resultAbc[seat.LocationY -1].ToString() + (seat.LocationX);

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
                        IndexDate = (int)span.TotalDays,
                        FilmId = ticket.MovieSchedule.FilmId,
                        CinemaName = ticket.MovieSchedule.Room.Cinema.CinemaName,
                        FilmName = ticket.MovieSchedule.Film.Name,
                        GroupCinemaName = ticket.MovieSchedule.Room.Cinema.GroupCinema.Name
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
        public IActionResult ResellTicket(int ticketId, String resellDescription)
        {
            try
            {
                Ticket ticket = context.Ticket.Where(t => t.TicketId == ticketId).FirstOrDefault();
                if (ticket != null)
                {
                    ticket.TicketStatus = "reselling";
                    ticket.ResellDescription = resellDescription;
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
                Ticket ticket = context.Ticket.Where(t => t.TicketId == ticketId)
                                                .Include(t => t.BookingTicket).ThenInclude(bt => bt.Customer)
                                                .Include(t => t.Seat).ThenInclude(s => s.Room).ThenInclude(r => r.Cinema)
                                                .Include(t => t.MovieSchedule).ThenInclude(ms => ms.Film)
                                                .FirstOrDefault();
                if (ticket != null)
                {
                    ticket.TicketStatus = "reselled";
                    String newTicketPaymentCode = ticket.TicketId + RandomUtility.RandomString(9);
                    ticket.PaymentCode = newTicketPaymentCode;

                    string content = "Chúc mừng quý khách đã mua lại vé thành công!";
                    content += "Bạn đã mua lại 1 vé của " + ticket.BookingTicket.Customer.Email + "\n";

                    content += "Tại " + ticket.Seat.Room.Cinema.CinemaName + "\n";
                    content += "Mã vé mới của bạn là " + newTicketPaymentCode + "\n";
                    content += "Phim " + ticket.MovieSchedule.Film.Name + "\n";
                    content += ". Ghế: " + ConstantArray.Alphabet[(int) ticket.Seat.Py] + "" + ((int)ticket.Seat.Px + 1) +
                                "- Mã vé: " + newTicketPaymentCode + "\n";
                    string mailSubject = "CinemaBookingTicket - Mua lại vé thành công " + ticket.Seat.Room.Cinema.CinemaName;

                    MailUtility.SendEmail(mailSubject, content, email);

                    context.SaveChanges();

                }

                return Ok(ticket);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        

        [HttpGet("confirmChangeTicket")]
        public IActionResult ConfirmChangeTicket(int ticketId, int scheduleId, int seatId)
        {
            try
            {
                Ticket ticket = context.Ticket.Where(t => t.TicketId == ticketId).Include(t => t.BookingTicket).ThenInclude(bt => bt.Customer)
                    .FirstOrDefault();

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
            catch (Exception ex)
            {
                Console.Write(ex);
                return BadRequest();
            }
        }
    }
}
