using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DemoPaypal.Controllers
{
    public class PaypalController : Controller
    {
        //
        // GET: /Paypal/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult demo1()
        {
            return View();
        }

        public ActionResult demo2()
        {
            return View();
        }

        public JsonResult checkout()
        {
            var obj = new
            {
                paymentID = 123456
            };
            return Json(obj); 
        }
    }
}
