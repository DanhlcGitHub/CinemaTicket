using CinemaTicket.Constant;
using CinemaTicket.Service;
using CinemaTicket.Utility;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CinemaTicket.Controllers
{
    public class TicketController : Controller
    {
        //
        // GET: /Ticket/

        public JsonResult GetTicketList()
        {
            string ticketDataStr = Request.Params["ticketData"];
            JObject ticketData = JObject.Parse(ticketDataStr);
            List<Ticket> ticketList = new List<Ticket>();
            string scheduleIdStr = (string)ticketData.GetValue("scheduleIdStr");
            int scheduleId = Convert.ToInt32(scheduleIdStr);
            string choosedListStr = (string)ticketData.GetValue("choosedList");
            JArray seatList = JArray.Parse(choosedListStr);
            foreach (JObject item in seatList)
            {
                int seatId = (int)item.GetValue("id");
                List<Ticket> tickets = new TicketService()
                            .FindBy(tic => tic.scheduleId == scheduleId && tic.seatId == seatId);
                Ticket ticket = null;
                if (tickets.Count == 0)
                {
                    ticket = new Ticket();
                    ticket.scheduleId = scheduleId;
                    ticket.seatId = seatId;
                    ticket.ticketStatus = TicketStatus.available;//set status
                    //insert ticket to db
                    new TicketService().Create(ticket);
                }
                else
                {
                    ticket = tickets.First();
                }
                ticketList.Add(ticket);
            }
            // loop each ticket , edit ticketStatus base on timeoutDate
            var obj = ticketList
                .Select(item => new
                {
                    ticketId = item.ticketId,
                    ticketStatus = item.ticketStatus,
                });
            return Json(obj);
        }

        public JsonResult ChangeAvailableToBuying()
        {
            string ticketListStr = Request.Params["ticketListStr"];
            JArray list = JArray.Parse(ticketListStr);
            List<Ticket> ticketList = new List<Ticket>();
            foreach (JObject item in list)
            {
                int ticketId = (int)item.GetValue("ticketId");
                Ticket aTicket = new TicketService().FindByID(ticketId);
                if (aTicket.ticketStatus == TicketStatus.available)
                {
                    aTicket.ticketStatus = TicketStatus.buying;
                    new TicketService().Update(aTicket);
                }
                ticketList.Add(aTicket);
            }
            var obj = ticketList
                .Select(item => new
                {
                    ticketId = item.ticketId,
                    ticketStatus = item.ticketStatus,
                });
            return Json(obj);
        }

        public JsonResult BuyingToAvailable(string ticketListStr)
        {
            JArray list = JArray.Parse(ticketListStr);
            List<Ticket> ticketList = new List<Ticket>();
            foreach (JObject item in list)
            {
                int ticketId = (int)item.GetValue("ticketId");
                Ticket aTicket = new TicketService().FindByID(ticketId);
                if (aTicket.ticketStatus == TicketStatus.buying)
                {
                    aTicket.ticketStatus = TicketStatus.available;
                    new TicketService().Update(aTicket);
                }
                ticketList.Add(aTicket);
            }
            var obj = ticketList
                .Select(item => new
                {
                    ticketId = item.ticketId,
                    ticketStatus = item.ticketStatus,
                });
            return Json(obj);
        }

        public JsonResult MakeOrder()
        {
            string orderDataStr = Request.Params["orderData"];
            JObject orderData = JObject.Parse(orderDataStr);
            string ticketListStr = (string) orderData.GetValue("ticketListStr");
            string email = (string)orderData.GetValue("email");
            string phone = (string)orderData.GetValue("phone");
            string filmName = (string)orderData.GetValue("filmName");
            string cinemaName = (string)orderData.GetValue("cinemaName");
            string date = (string)orderData.GetValue("date");
            string roomName = (string)orderData.GetValue("roomName");
            string startTime = (string)orderData.GetValue("startTime");
            string userId = (string)orderData.GetValue("userId");
            JArray list = JArray.Parse(ticketListStr);
            List<Ticket> ticketList = new List<Ticket>();
            //create customer 
            Customer cus = new Customer();
            cus.email = email;
            cus.phone = phone;
            if (userId != null) cus.userId = userId;
            int cusId = new CustomerService().createCustomer(cus);//get cusId
            //create order
            BookingTicket order = new BookingTicket();
            order.quantity = list.Count;
            order.bookingDate = DateTime.Today;
            order.customerId = cusId;
            int orderId = new BookingTicketService().CreateOrder(order);
            order.paymentCode = orderId +RandomUtility.RandomString(9);
            
            var obj = new
                {
                    isSuccess = "true"
                };
            try
            {
                
                foreach (JObject item in list)
                {
                    int ticketId = (int)item.GetValue("ticketId");
                    Ticket aTicket = new TicketService().FindByID(ticketId);
                    if (aTicket.ticketStatus == TicketStatus.buying)
                    {
                        aTicket.ticketStatus = TicketStatus.buyed;
                        //update ticket price
                        double price = (double)new TypeOfSeatService().FindByID(
                                            (new SeatService().FindByID(aTicket.seatId).typeSeatId)).price;
                        aTicket.price = price;

                        string code = aTicket.ticketId + RandomUtility.RandomString(9);
                        aTicket.paymentCode = code;
                        aTicket.bookingId = orderId;
                    }
                    ticketList.Add(aTicket);
                }

                //send email for customer
                string mailContent = getEmailContent(ticketList, order, filmName, cinemaName, date, roomName, startTime);
                string mailSubject = "VietCine - Mã vé xem phim tại " + cinemaName;
                MailUtility.SendEmail(mailSubject, mailContent, email);
                // insert into db
                foreach (var aTicket in ticketList)
                {
                    new TicketService().Update(aTicket);
                }
            }
            catch (Exception)
            {
                new BookingTicketService().Delete(orderId);
                obj = new 
                {
                    isSuccess = "false"
                };
            }
            
            /*var obj = ticketList
                .Select(item => new
                {
                    ticketId = item.ticketId,
                    ticketStatus = item.ticketStatus,
                });*/
            return Json(obj);
        }

        private string getEmailContent(List<Ticket> ticketList, BookingTicket bt, string filmName, string cinemaName,
            string date, string roomName, string startTime)
        {
            string content = "Chúc mừng quý khách đã đặt vé thành công!";
            content += "Phim " + filmName + "\n";
            content += "Tại " + cinemaName + "\n";
            content += date + " - " + startTime + " - " + roomName + "\n";
            content += "Mã đặt vé của bạn là: " + bt.paymentCode + "\n";
            content += "Mã từng vé của bạn là: \n";
            for (int i = 0; i < ticketList.Count; i++)
            {
                Ticket aTicket = ticketList[i];
                Seat seat = new SeatService().FindByID(aTicket.seatId);
                content += "    " + (i + 1) +
                            ". Ghế: " + ConstantArray.Alphabet[(int)seat.py] + "" + ((int)seat.px + 1) +
                            "- Mã vé: " + aTicket.paymentCode + "\n";
            }
            content += "Chúc quý khách xem phim vui vẻ";
            return content;
        }
        public void SendResellConfirmCode(string email)
        {
            string confirmCode = RandomUtility.RandomString(4);
            string mailContent = "Mã xác nhận email của bạn là: " + confirmCode;
            string mailSubject = "CinemaBookingTicket - Mã xác nhận bán lại vé";
            MailUtility.SendEmail(mailSubject, mailContent, email);
            Session["resellConfirmCode"] = confirmCode;
        }

        public JsonResult GetTicketListBelongToMail(string confirmCode, string email)
        {
            string sessionConfirmCode = (string)Session["resellConfirmCode"];
            if (sessionConfirmCode != null && sessionConfirmCode.Equals(confirmCode))
            {
                List<object> objectList = new List<object>();
                List<Ticket> ticketList = new TicketService().getTicketByEmail(email);
                foreach (var item in ticketList)
                {
                    MovieSchedule schedule = new MovieScheduleService().FindByID(item.scheduleId);
                    Room room = new RoomService().FindByID(schedule.roomId);
                    Seat seat = new SeatService().FindByID(item.seatId);
                    string startTime = new ShowTimeService().FindByID(schedule.timeId).startTime;
                    string filmName = new FilmService().FindByID(schedule.filmId).name;
                    string cinemaName = new CinemaService().FindByID(room.cinemaId).cinemaName;
                    string roomName = room.name;
                    int seatId = seat.seatId;
                    string seatName = ConstantArray.Alphabet[(int)seat.py] + "" + ((int)seat.px + 1);
                    string date = String.Format("{0:dd/MM/yyyy}", schedule.scheduleDate);
                    var aObj = new
                    {
                        ticketId = item.ticketId,
                        bookingId = item.bookingId,
                        scheduleId = item.scheduleId,
                        seatId = seatId,
                        filmName = filmName,
                        cinemaNames = cinemaName.Split('-'),
                        roomName = roomName,
                        seatName = seatName,
                        date = date,
                        startTime = startTime,
                        status = item.ticketStatus,
                        isOverDay = DateTime.Now > schedule.scheduleDate ? true : false,
                        statusvn = TicketStatus.ViStatus[item.ticketStatus],
                    };
                    objectList.Add(aObj);
                }
                var returnObj = from s in objectList
                                select s;
                return Json(returnObj);
            }
            else
            {
                var aObj = new
                {
                    isWrong = "true"
                };
                return Json(aObj);
            }
        }
        //
        public JsonResult PostSellingTicket(string ticketId, string description)
        {
            Ticket ticket = new TicketService().FindByID(Convert.ToInt32(ticketId));
            ticket.ticketStatus = TicketStatus.resell;
            ticket.resellDescription = description;
            new TicketService().Update(ticket);
            var aObj = new
            {
                status = ticket.ticketStatus,
                statusvn = TicketStatus.ViStatus[ticket.ticketStatus]
            };
            return Json(aObj);
        }


        public JsonResult ResellTicket(string ticketId, string buyerEmail, string sellerEmail)
        {
            Ticket ticket = new TicketService().FindByID(Convert.ToInt32(ticketId));
            ticket.ticketStatus = TicketStatus.reselled;
            string newCode = ticket.ticketId + RandomUtility.RandomString(9);

            string content = "Chúc mừng quý khách đã mua lại vé thành công!";
            content += "Bạn đã mua lại 1 vé của " + sellerEmail + "\n";
            Seat seat = new SeatService().FindByID(ticket.seatId);
            Room room = new RoomService().FindByID(seat.roomId);
            Cinema cinema = new CinemaService().FindByID(room.cinemaId);
            MovieSchedule schedule = new MovieScheduleService().FindByID(ticket.scheduleId);
            Film film = new FilmService().FindByID(schedule.filmId);
            content += "Tại " + cinema.cinemaName + "\n";
            content += "Mã vẽ mới của bạn là " + newCode + "\n";
            content += "Phim " + film.name + "\n";
            content += ". Ghế: " + ConstantArray.Alphabet[(int)seat.py] + "" + ((int)seat.px + 1) +
                        "- Mã vé: " + newCode + "\n";
            string mailSubject = "CinemaBookingTicket - Mua lại vé thành công " + cinema.cinemaName;
            try
            {
                MailUtility.SendEmail(mailSubject, content, buyerEmail);
                ticket.paymentCode = newCode;
                new TicketService().Update(ticket);
            }
            catch (Exception)
            {
                var obj = new
                {
                    isSuccess = "false"
                };
                return Json(obj);
            }

            var aObj = new
            {
                isSuccess = "true",
                status = ticket.ticketStatus,
                statusvn = TicketStatus.ViStatus[ticket.ticketStatus]
            };
            return Json(aObj);
        }
    }
}
