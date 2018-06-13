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
            var obj = new
            {
                matrixX = room.matrixSizeX,
                matrixY = room.matrixSizeY,
                seats = seats.Select(s => new
                {
                    id = s.seatId,
                    type = s.typeSeatId,
                    px = s.px,
                    py = s.py,
                })
            };
            return Json(obj);
        }

    }
}

