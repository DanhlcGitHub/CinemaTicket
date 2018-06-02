using CinemaTicket.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CinemaTicket.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public string Index()
        {
            String s = "";
            DigitalTypeService ser = new DigitalTypeService();
            List<DigitalType> list = ser.GetAll();
            foreach (var item in list)
            {
                string sub = "id: " + item.digTypeId + "name: " + item.name + "\n";
                s += sub;
            }
            return s;
        }

    }
}
