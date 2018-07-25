using CinemaBookingCore.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CinemaBookingCore.Data.Entities;
using CinemaBookingCore.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaBookingCore.Controllers
{
    [Route("[controller]")]
    public class OrdersController : Controller
    {
        private readonly CinemaBookingDBContext context;

        public OrdersController(CinemaBookingDBContext context)
        {
            this.context = context;
        }

        [HttpGet("orderStepOne")]
        public IActionResult getChoiceTicketOrder(int scheduleId)
        {
            MovieSchedule schedule = context.MovieSchedule.Where(s => s.ScheduleId == scheduleId)
                                                            .Include(s => s.ShowTime)
                                                            .Include(s => s.Room).ThenInclude(r => r.DigitalType)
                                                            .Include(s => s.Room).ThenInclude(r => r.Cinema).ThenInclude(c => c.GroupCinema).ThenInclude(g => g.TypeOfSeats)
                                                            .Include(s => s.Film)
                                                            .Include(s => s.ShowTime)
                                                            .FirstOrDefault();

            List<TypeOfSeatModel> typeOfSeatModels = new List<TypeOfSeatModel>();
            List<TypeOfSeat> typeOfSeats = schedule.Room.Cinema.GroupCinema.TypeOfSeats.ToList();

            for (int i = 0; i < typeOfSeats.Count(); i++)
            {
                TypeOfSeatModel type = new TypeOfSeatModel { TypeName = typeOfSeats[i].TypeName, Price = typeOfSeats[i].Price.ToString() };
                typeOfSeatModels.Add(type);
            }
            var room = schedule.Room;
            var film = schedule.Film;

            OrderChoiceTicketModel result = new OrderChoiceTicketModel
            {
                CinemaName = room.Cinema.CinemaName,
                DigType = room.DigitalType.Name,
                RoomName = room.Name,
                FilmLength = film.FilmLength.ToString(),
                FilmName = film.Name,
                Restricted = "C" + film.Restricted,
                TimeShow = schedule.ShowTime.StartTime,
                GroupCinemaName = schedule.Room.Cinema.GroupCinema.Name,
                TypeOfSeats = typeOfSeatModels,
                FilmImage = film.AdditionPicture
            };

            return Ok(result);
        }

        [HttpGet("seats")]
        public IActionResult getSeatsInRoom(int roomId, int scheduleId)
        {

            var items = context.Seat.Include(s => s.TypeOfSeat).Include(s => s.Tickets).Where(s => s.RoomId == roomId).ToList();

            List<SeatModel> seatModels = new List<SeatModel>();

            foreach (var seat in items)
            {
                SeatModel seatModel = new SeatModel
                {
                    RoomId = seat.RoomId,
                    Px = seat.Px,
                    Py = seat.Py,
                    SeatId = seat.SeatId,
                    Price = seat.TypeOfSeat.Price,
                    TypeSeatId = seat.TypeSeatId,
                    isBooked = false,
                    TicketStatus = "available"
                };

                foreach (var ticket in seat.Tickets)
                {
                    if (ticket.ScheduleId == scheduleId)
                    {
                        seatModel.TicketStatus = ticket.TicketStatus;
                        if (ticket.TicketStatus == "resell")
                        {
                            BookingTicket bookingTicket = context.BookingTicket.Where(b => b.BookingId == ticket.BookingId).FirstOrDefault();
                            Customer customer = context.Customer.Where(c => c.CustomerId == bookingTicket.CustomerId).FirstOrDefault();

                            

                            seatModel.Email = customer.Email;
                            seatModel.Phone = customer.Phone;
                        }
                        seatModel.TicketId = ticket.TicketId;
                    }
                }
                seatModels.Add(seatModel);
            }

            return Ok(seatModels);
        }

        [HttpPost("bookingTicket")]
        public IActionResult bookingTicket([FromBody] SeatCollectionModel seatCollectionModel)
        {
            try
            {
                List<Ticket> tickets = seatCollectionModel.TicketModels;
                int listTicketSize = tickets.Count();
                for (int i = 0; i < listTicketSize; i++)
                {
                    var item = tickets[i];

                    Ticket ticket = context.Ticket.Where(t => t.ScheduleId == item.ScheduleId && t.SeatId == item.SeatId).FirstOrDefault();
                    if (ticket == null)
                    {
                        ticket = new Ticket();
                        ticket.BookingId = null;
                        ticket.TicketStatus = "buying";
                        ticket.ScheduleId = item.ScheduleId;
                        ticket.Price = item.Price;
                        ticket.SeatId = item.SeatId;

                        context.Add(ticket);
                        context.SaveChanges();

                        seatCollectionModel.TicketModels[i] = ticket;
                        seatCollectionModel.isSuccesBookingTicket = true;
                    }
                    else
                    {
                        switch (ticket.TicketStatus)
                        {
                            case "available":
                                ticket.TicketStatus = "buying";
                                item.TicketStatus = "buying";
                                context.Update(ticket);
                                context.SaveChanges();
                                seatCollectionModel.isSuccesBookingTicket = true;
                                break;

                            case "buying":
                                seatCollectionModel.isSuccesBookingTicket = false;
                                break;

                            case "buyed":
                                seatCollectionModel.isSuccesBookingTicket = true;
                                break;

                            case "resell":
                                seatCollectionModel.isSuccesBookingTicket = false;
                                break;

                            case "reselled":
                                seatCollectionModel.isSuccesBookingTicket = true;
                                break;
                        }
                    }
                }
            }
            catch (System.Exception)
            {
                seatCollectionModel.isSuccesBookingTicket = false;
            }

            return Ok(seatCollectionModel);
        }

        public List<int> listTicketId = new List<int>();

        [HttpPost("finishPaypalPayment")]
        public IActionResult finishPaypalPayment([FromBody] BookingTicket bookingTicketModel)
        {
            try
            {
                Customer customer = context.Customer.Where(c => c.Phone == bookingTicketModel.Customer.Phone && c.Email == bookingTicketModel.Customer.Email).FirstOrDefault();
                if (customer == null)
                {
                    customer = new Customer
                    {
                        Email = bookingTicketModel.Customer.Email,
                        Phone = bookingTicketModel.Customer.Phone,
                        UserId = bookingTicketModel.Customer.UserId,
                        

                    };

                    context.Add(customer);
                    context.SaveChanges();
                }

                DateTime nowDate = DateTime.UtcNow;
                nowDate = nowDate.AddHours(7);
                BookingTicket bookingTicket = new BookingTicket
                {
                    CustomerId = customer.CustomerId,
                    Quantity = bookingTicketModel.Quantity,
                    BookingDate = nowDate,
                };

                context.Add(bookingTicket);
                context.SaveChanges();

                foreach (var item in bookingTicketModel.Tickets)
                {
                    listTicketId.Add(item.TicketId);
                }

                foreach (var item in listTicketId)
                {
                    Ticket ticket = context.Ticket.Where(t => t.TicketId == item).FirstOrDefault();
                    ticket.BookingId = bookingTicket.BookingId;
                    ticket.QrCode = "Content/img/QRCode/qr.png";
                    ticket.TicketStatus = "buyed";
                    context.Update(ticket);
                }
                context.SaveChanges();

                foreach (var item in bookingTicketModel.Tickets)
                {
                    item.TicketStatus = "buyed";
                }
            }
            catch (System.Exception)
            {
            }

            return Ok(bookingTicketModel);
        }


        [HttpPut("changeStatusTicket")]
        public IActionResult changeStatusTicket([FromBody]List<Ticket> tickets)
        {

            Ticket ticket;
            foreach (var item in tickets)
            {
                if(item.TicketStatus != "buyed")
                {
                    ticket = context.Ticket.Where(t => t.TicketId == item.TicketId).FirstOrDefault();
                    ticket.TicketStatus = "available";
                }
            }
            context.SaveChanges();

            return Ok(tickets);
        }

        [HttpGet("ordersByAccountId")]
        public IActionResult ordersByAccountId(int accountId)
        {
            List<Ticket> tickets = context.Ticket.ToList();

            return Ok();
        }

        [HttpGet("getOrderByAccountId")]
        public IActionResult getTicketByAccountId(string username)
        {
            try
            {
                UserAccount user = context.UserAccount.Where(u => u.UserId == username).FirstOrDefault();
                List<Customer> customers = new List<Customer>();
                List<BookingTicket> bookingTickets = new List<BookingTicket>();

                if (user != null)
                {
                    customers = context.Customer.Where(c => c.UserId == user.UserId).ToList();
                    foreach (var customer in customers)
                    {
                        bookingTickets = context.BookingTicket.Where(b => b.CustomerId == customer.CustomerId).ToList();
                        foreach (var bookingTicket in bookingTickets)
                        {
                            Ticket ticket = context.Ticket
                                .Where(t => t.BookingId == bookingTicket.BookingId)
                                .Include(t => t.MovieSchedule)
                                .ThenInclude(s => s.ShowTime)
                                .FirstOrDefault();

                            Film film = context.Film.Where(f => f.FilmId == ticket.MovieSchedule.FilmId).FirstOrDefault();
                        }
                    }
                }
                return Ok(bookingTickets);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
