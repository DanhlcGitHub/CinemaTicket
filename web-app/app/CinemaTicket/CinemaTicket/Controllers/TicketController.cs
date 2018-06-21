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

        public JsonResult GetTicketList(string choosedList, string scheduleIdStr)
        {
            List<Ticket> ticketList = new List<Ticket>();
            int scheduleId = Convert.ToInt32(scheduleIdStr);
            JArray seatList = JArray.Parse(choosedList);
            foreach (JObject item in seatList)
            {
                int seatId = (int)item.GetValue("seatId");
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

        public JsonResult ChangeAvailableToBuying(string ticketListStr)
        {
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

        public JsonResult MakeOrder(string ticketListStr,string email, string phone)
        {
            JArray list = JArray.Parse(ticketListStr);
            List<Ticket> ticketList = new List<Ticket>();
            //create customer 
            Customer cus = new Customer();
            cus.email = email;
            cus.phone = phone;
            int cusId = new CustomerService().createCustomer(cus);//get cusId
            //create order
            BookingTicket order = new BookingTicket();
            order.quantity = list.Count;
            order.bookingDate = DateTime.Today;
            order.customerId = cusId;
            order.paymentCode = RandomUtility.RandomString(9);
            int orderId = new BookingTicketService().CreateOrder(order);
            
            foreach (JObject item in list)
            {
                int ticketId = (int)item.GetValue("ticketId");
                Ticket aTicket = new TicketService().FindByID(ticketId);
                if (aTicket.ticketStatus == TicketStatus.buying)
                {
                    aTicket.ticketStatus = TicketStatus.buyed;
                    //update ticket price
                    double price = (double) new TypeOfSeatService().FindByID(
                                        (new SeatService().FindByID(aTicket.seatId).typeSeatId)).price;
                    aTicket.price = price;
                    
                    string code = aTicket.ticketId + RandomUtility.RandomString(9);
                    aTicket.paymentCode = code;
                    aTicket.bookingId = orderId;
                    new TicketService().Update(aTicket);
                }
                ticketList.Add(aTicket);
            }
            //send email for customer
            var obj = ticketList
                .Select(item => new
                {
                    ticketId = item.ticketId,
                    ticketStatus = item.ticketStatus,
                });
            return Json(obj);
        }
    }
}
