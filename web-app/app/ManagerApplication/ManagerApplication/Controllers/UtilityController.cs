using ManagerApplication.Constant;
using ManagerApplication.Service;
using ManagerApplication.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManagerApplication.Controllers
{
    public class UtilityController : Controller
    {
        //
        // GET: /Utility/

        public JsonResult logout()
        {
            var obj = new
            {
                status = "ok"
            };
            Session.Clear();
            return Json(obj);
        }

        public JsonResult CheckLogin(string username, string password, string role)
        {
            var obj = new
            {
                valid = "true",
            };
            string encryptedPassword = EncryptUtility.EncryptString(password);
            if (role == Role.Partner)
            {
                List<PartnerAccount> partnerList = new PartnerAccountService().FindBy(u => u.partnerId == username
                    && u.partnerPassword == encryptedPassword);
                if (partnerList != null && partnerList.Count != 0)
                {
                    PartnerAccount p = partnerList.First();
                    if (p != null)
                    {
                        Session[AppSession.User] = p;
                        Session[AppSession.UserRole] = role;
                        return Json(obj);
                    }
                }
            }
            else if (role == Role.CinemaManager)
            {
                List<CinemaManager> managerList = new CinemaManagerService().FindBy(u => u.managerId == username
                    && u.managerPassword == encryptedPassword);
                if (managerList!=null && managerList.Count != 0)
                {
                    CinemaManager c = managerList.First();
                    if (c != null)
                    {
                        Session[AppSession.User] = c;
                        Session[AppSession.UserRole] = role;
                        return Json(obj);
                    }
                }
            }
            return null;
        }

    }
}
