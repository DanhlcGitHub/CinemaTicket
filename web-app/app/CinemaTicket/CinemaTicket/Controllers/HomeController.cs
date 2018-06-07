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

        [HttpPost]
        public JsonResult LoadAvailableFilm()
        {
            FilmService filmService = new FilmService();
            List<Film> filmList = filmService.FindBy(f => f.filmStatus != -1);// tao enum cho film status
            var obj = filmList
                .Select(item => new
                {
                    name = item.name,
                    filmStatus = item.filmStatus,
                    imdb = item.imdb,
                    dateRelease = item.dateRelease,
                    restricted = item.restricted,
                    img = item.additionPicture.Split(';')[0],
                    length = item.filmLength
                });                
            return Json(obj);
        }

        [HttpPost]
        public JsonResult LoadCinema()
        {
            GroupCinemaServcie gcService = new GroupCinemaServcie();
            CinemaService cService = new CinemaService();
            List<GroupCinema> groupCinemaList = gcService.GetAll();
            var obj = groupCinemaList
                .Select(item => new
                {
                    name = item.name,
                    img = item.logoImg,
                    cinemas = cService.FindBy(c => c.groupId == item.GroupId)
                                      .Select(cine => new
                                                      {
                                                         id = cine.cinemaId,
                                                         img = cine.profilePicture,
                                                         name = cine.cinemaName,
                                                         address = cine.cinemaAddress
                                                      })
                });
            return Json(obj);

        }
    }
}
