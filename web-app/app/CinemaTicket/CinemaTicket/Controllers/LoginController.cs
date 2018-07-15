using CinemaTicket.Service;
using CinemaTicket.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CinemaTicket.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/

        public JsonResult CheckLogin(string username, string password)
        {
            var obj = new
            {
                username = "",
                email = "",
                phone = "",
                status = "notValid"
            };
            string encryptedPassword = EncryptUtility.EncryptString(password);
            UserAccount user;
            List<UserAccount> userList = new UserAccountService().FindBy(u => u.userId == username && u.userPassword == encryptedPassword);
            if (userList.Count != 0)
            {
                user = userList.First();
                if (user != null)
                {
                    Session["user"] = user.userId;
                    obj = new
                    {
                        username = user.userId,
                        email = user.email,
                        phone = user.phone,
                        status = "valid"
                    };
                }
            }
            return Json(obj);
        }

        public JsonResult CheckRegister(string username, string password, string email, string phone)
        {
            var obj = new
            {
                username = "",
                email = "",
                phone = "",
                status = "notValid"
            };
            UserAccount user;
            List<UserAccount> userList = new UserAccountService().FindBy(u => u.userId == username);
            if (userList.Count == 0)
            {
                user = new UserAccount();
                user.userId = username;
                user.userPassword = EncryptUtility.EncryptString(password);
                user.email = email;
                user.phone = phone;
                new UserAccountService().Create(user);
                obj = new
                {
                    username = user.userId,
                    email = user.email,
                    phone = user.phone,
                    status = "valid"
                };
            }
            return Json(obj);
        }

        public JsonResult Logout()
        {
            var obj = new
            {
                status = "ok"
            };
            Session.Clear();
            return Json(obj);
        }
        
    }
}
