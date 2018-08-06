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
        public string test()
        {
            return EncryptUtility.EncryptString1("123456");
        }
        public ActionResult Index()
        {
            var obj = Session[AppSession.User];
            if (obj != null)
            {
                string role = Session[AppSession.UserRole].ToString();
                if (role.Equals(Role.CinemaManager))
                {
                    CinemaManager cm = (CinemaManager)obj;
                    ViewBag.cinemaId = cm.cinemaId;
                    return View("~/Views/CinemaManager/CinemaManagerHome.cshtml");
                }
                else if (role.Equals(Role.Partner))
                {
                    PartnerAccount pa = (PartnerAccount)obj;
                    ViewBag.groupId = pa.groupOfCinemaId;
                    return View("~/Views/Partner/partnerHome.cshtml");
                }
            }
            return View();
        }
    }
}
