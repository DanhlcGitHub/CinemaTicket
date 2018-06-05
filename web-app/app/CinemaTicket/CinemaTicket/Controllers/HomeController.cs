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

        public ActionResult Index()
        {
            FilmService filmService = new FilmService();
            List<Film> hightLight = filmService.FindByTop(4);
            ViewBag.hightLight = hightLight;
            /*foreach (var item in hightLight)
            {
                item.posterPicture = System.Web.HttpContext.Current.Server.MapPath(item.posterPicture);
            }*/
            return View();
        }
    }
}
