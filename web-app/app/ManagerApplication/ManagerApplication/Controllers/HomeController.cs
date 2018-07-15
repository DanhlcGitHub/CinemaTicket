using ManagerApplication.Constant;
using ManagerApplication.Service;
using ManagerApplication.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ManagerApplication.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            var obj = Session[AppSession.User];
            if (obj != null)
            {
                string role = Session[AppSession.UserRole].ToString();
                if (role.Equals(Role.CinemaManager))
                {
                    return View("~/Views/Partner/partnerHome.cshtml");
                }
                else if (role.Equals(Role.Partner))
                {
                    return View("~/Views/Partner/partnerHome.cshtml");
                }
            }
            return View();
        }

       
    }
}
