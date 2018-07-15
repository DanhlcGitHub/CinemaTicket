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
            return View();
        }

        public JsonResult CheckLogin(string username, string password, string role)
        {
            string encryptedPassword = EncryptUtility.EncryptString(password);
            if (role == "partner")
            {
                List<PartnerAccount> partnerList = new PartnerAccountService().FindBy(u => u.partnerId == username 
                    && u.partnerPassword == encryptedPassword);
                if (partnerList.Count != 0)
                {
                    PartnerAccount p = partnerList.First();
                    if (p != null)
                    {
                        Session["user"] = p;
                        Session["user_role"] = role;
                        //return view
                    }
                }
            }
            else if (role == "cinemaManager")
            {

            }
            UserAccount user;
            List<UserAccount> userList = new UserAccountService().FindBy(u => u.userId == username && u.userPassword == encryptedPassword);
            
            return Json(obj);
        }
    }
}
