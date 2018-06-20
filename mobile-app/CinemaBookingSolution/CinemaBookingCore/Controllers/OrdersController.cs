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
        public IActionResult getChoiceTicketOrder(int filmId, int roomId, int groupId, int scheduleId)
        {
            Room room = context.Room.Where(r => r.RoomId == roomId).Include(r => r.Cinema).Include(r => r.DigitalType).FirstOrDefault();
            Film film = context.Film.Where(f => f.FilmId == filmId).FirstOrDefault();
            GroupCinema groupCinema = context.GroupCinema.Where(g => g.GroupId == groupId).Include(g => g.TypeOfSeats).FirstOrDefault();
            MovieSchedule schedule = context.MovieSchedule.Where(s => s.ScheduleId == scheduleId).Include(s => s.ShowTime).FirstOrDefault();
            List<TypeOfSeatModel> typeOfSeatModels = new List<TypeOfSeatModel>();
            List<TypeOfSeat> typeOfSeats = groupCinema.TypeOfSeats.ToList();
            for (int i = 0; i < typeOfSeats.Count(); i++)
            {
                TypeOfSeatModel type = new TypeOfSeatModel { TypeName = typeOfSeats[i].TypeName, Price = typeOfSeats[i].Price.ToString() };
                typeOfSeatModels.Add(type);
            }

            OrderChoiceTicketModel result = new OrderChoiceTicketModel
            {
                CinemaName = room.Cinema.CinemaName,
                DigType = room.DigitalType.Name,
                RoomName = room.Name,
                FilmLength = film.FilmLength.ToString(),
                FilmName = film.Name,
                Restricted = "C" + film.Restricted,
                TimeShow = schedule.ShowTime.StartTime,
                GroupCinemaName = groupCinema.Name,
                TypeOfSeats = typeOfSeatModels,
                FilmImage = film.PosterPicture
            };

            return Ok(result);
        }

        [HttpGet("seats")]
        public IActionResult getSeatsInRoom(int roomId, int scheduleId)
        {

            //var items = from s in context.Seat
            //            join tos in context.TypeOfSeat on s.TypeSeatId equals tos.TypeSeatId
            //            where s.RoomId == roomId
            //            select new
            //            {
            //                Seat = s,
            //                TypeOfSeat = tos
            //            };

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
                    isBooked = false
                };

                foreach (var ticket in seat.Tickets)
                {
                    if (ticket.ScheduleId == scheduleId && ticket.TicketStatus == "Success")
                    {
                        seatModel.isBooked = true;
                    }
                }
                seatModels.Add(seatModel);
            }

            return Ok(seatModels);
        }

        [HttpPost("bookingTicket")]
        public IActionResult BookingTicket([FromBody] SeatCollectionModel seatCollectionModel)
        {
            try
            {
                foreach (var item in seatCollectionModel.TicketModels)
                {

                    Ticket ticket = context.Ticket.Where(t => t.ScheduleId == item.ScheduleId && t.SeatId == item.SeatId).FirstOrDefault();
                    if (ticket == null)
                    {
                        item.TicketStatus = "Buyding";
                        item.TicketTimeout = DateTime.Now.AddMinutes(5);
                        context.Add(item);
                        context.SaveChanges();

                        seatCollectionModel.isSuccesBookingTicket = true;
                    }
                    else
                    {
                        switch (ticket.TicketStatus)
                        {
                            case "Available":
                                ticket.TicketStatus = "Buyding";
                                context.Update(ticket);
                                context.SaveChanges();

                                seatCollectionModel.isSuccesBookingTicket = true;
                                break;

                            case "Buyding":
                                seatCollectionModel.isSuccesBookingTicket = false;
                                break;

                            case "Success":
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
        public IActionResult FinishPaypalPayment([FromBody] BookingTicket bookingTicketModel)
        {
            try
            {
                Customer customer = context.Customer.Where(c => c.Phone == bookingTicketModel.Customer.Phone && c.Email == bookingTicketModel.Customer.Email).FirstOrDefault();
                if (customer == null)
                {
                    customer = new Customer
                    {
                        Email = bookingTicketModel.Customer.Email,
                        Phone = bookingTicketModel.Customer.Phone
                    };
                    context.Add(customer);
                    context.SaveChanges();
                }

                BookingTicket bookingTicket = new BookingTicket
                {
                    CustomerId = customer.CustomerId,
                    Quantity = bookingTicketModel.Quantity,
                    BookingDate = DateTime.Now
                };

                context.Add(bookingTicket);
                context.SaveChanges();

                foreach (var item in bookingTicketModel.BookingDetails)
                {
                    item.BookingId = bookingTicket.BookingId;
                    listTicketId.Add(item.TicketId);
                    context.Add(item);
                }
                context.SaveChanges();

                foreach (var item in listTicketId)
                {
                    Ticket ticket = context.Ticket.Where(t => t.TicketId == item).FirstOrDefault();
                    ticket.TicketStatus = "Success";
                    context.Update(ticket);
                }
                context.SaveChanges();
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
                ticket = context.Ticket.Where(t => t.TicketId == item.TicketId).FirstOrDefault();
                ticket.TicketStatus = "Available";
            }
            context.SaveChanges();

            return Ok(tickets);
        }
    }
}
