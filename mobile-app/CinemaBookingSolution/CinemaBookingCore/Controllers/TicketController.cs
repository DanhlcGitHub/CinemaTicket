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

        [HttpGet("getAllTicketByBookingTicketId")]
        public IActionResult GetAllTicketByBookingTicketId(int bookingTicketId)
        {
            try
            {
                List<Ticket> tickets = context.Ticket.Where(t => t.BookingId == bookingTicketId).ToList();
                List<TicketModel> ticketModels = new List<TicketModel>();

                foreach (var ticket in tickets)
                {
                    Seat seat = context.Seat.Where(s => s.SeatId == ticket.SeatId).FirstOrDefault();
                    Room roomForSeat = context.Room.Where(r => r.RoomId == seat.RoomId).FirstOrDefault();

                    Char character = 'A';
                    List<Char> resultAbc = new List<Char>();
                    int ascii = (int)character;

                    for (int i = 0; i < roomForSeat.MatrixSizeX; i++)
                    {
                        Char tmp = (char)(ascii + i);
                        resultAbc.Add(tmp);
                    }

                    String position = resultAbc[seat.Py].ToString() + (seat.Px + 1);

                    TicketModel ticketModel = new TicketModel
                    {
                        BookingId = ticket.BookingId,
                        Price = ticket.Price,
                        QrCode = ticket.QrCode,
                        ScheduleId = ticket.ScheduleId,
                        SeatId = ticket.SeatId,
                        TicketId = ticket.TicketId,
                        TicketStatus = ticket.TicketStatus,
                        SeatPosition = position
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

                if(customer == null)
                {
                    customer = new Customer {
                        Email = email
                    };
                    context.SaveChanges();
                }

                BookingTicket bookingTicket = new BookingTicket
                {
                    BookingDate = DateTime.Now,
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

    }
}
