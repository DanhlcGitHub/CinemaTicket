using CinemaTicket.Service;
using CinemaTicket.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CinemaTicket.Controllers
{
    public class CinemaController : Controller
    {
        //
        // GET: /Cinema/

        [HttpPost]
        public JsonResult LoadCinema()
        {
            GroupCinemaServcie gcService = new GroupCinemaServcie();
            CinemaService cService = new CinemaService();
            ShowTimeService tService = new ShowTimeService();
            FilmService fService = new FilmService();

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
                                          address = cine.cinemaAddress,
                                      })
                });
            return Json(obj);
        }

        [HttpPost]
        public JsonResult LoadGroupCinema()
        {
            GroupCinemaServcie gcService = new GroupCinemaServcie();
            CinemaService cService = new CinemaService();
            ShowTimeService tService = new ShowTimeService();
            FilmService fService = new FilmService();
            string serverPath = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            List<GroupCinema> groupCinemaList = gcService.GetAll();
            var obj = groupCinemaList
                .Select(item => new
                {
                    id = item.GroupId,
                    name = item.name,
                    img = serverPath + item.logoImg
                });
            return Json(obj);
        }

       
    }
}
