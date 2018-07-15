using CinemaTicket.Constant;
using CinemaTicket.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CinemaTicket.Controllers
{
    public class SeatController : Controller
    {
        //
        // GET: /Seat/

        public JsonResult FindAllSeatByScheduleId(string scheduleId)
        {
            int scheduleIdData = Convert.ToInt32(scheduleId);
            int roomIdData = (int)new MovieScheduleService().FindByID(scheduleIdData).roomId;
            Room room = new RoomService().FindByID(roomIdData);
            List<Seat> seats = new SeatService().FindBy(s => s.roomId == roomIdData);
            List<Ticket> ticketList = new TicketService().FindBy(tic => tic.scheduleId == scheduleIdData);

            List<object> objectList = new List<object>();
            foreach (var item in seats)
            {
                Ticket ticket = ticketList.Find(t => t.seatId == item.seatId) == null
                                        ? null : ticketList.Find(t => t.seatId == item.seatId);
                string emailOwner = "";
                if (ticket != null && ticket.bookingId != null)
                {
                    int cusId = (int)new BookingTicketService().FindByID(ticket.bookingId).customerId;
                    emailOwner = new CustomerService().FindByID(cusId).email;
                }
                var aObj = new
                {
                    id = item.seatId,
                    seatStatus = ticket == null ? TicketStatus.available : ticket.ticketStatus,
                    type = item.typeSeatId,
                    px = item.px,
                    py = item.py,
                    locationX = item.locationX,
                    locationY = item.locationY,
                    resellDescription = ticket == null ? "" : ticket.resellDescription,
                    emailOwner = emailOwner,
                };
                objectList.Add(aObj);
            }
            var seatData = from s in objectList
                           select s;
            var obj = new
            {
                matrixX = room.matrixSizeX,
                matrixY = room.matrixSizeY,
                seats = seatData
            };
            return Json(obj);
        }

    }
}