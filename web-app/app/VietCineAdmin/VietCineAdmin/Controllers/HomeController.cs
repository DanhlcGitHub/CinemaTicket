using VietCineAdmin.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VietCineAdmin.Utility;
using VietCineAdmin.Constant;
using VietCineAdmin.Service;

namespace VietCineAdmin.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var obj = Session[AppSession.User];
            if (obj != null)
            {
                return View("~/Views/Home/index.cshtml");
            }
            return View("~/Views/Home/login.cshtml");
        }

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
            List<AdminAccount> adminList = new AdminAccountService().FindBy(u => u.adminId == username
                && u.adminPassword == encryptedPassword);
            if (adminList != null && adminList.Count != 0)
            {
                AdminAccount p = adminList.First();
                if (p != null)
                {
                    Session[AppSession.User] = p;
                    return Json(obj);
                }
            }

            return null;
        }

        public ActionResult ManagerPartnerAccount()
        {
            GroupCinemaServcie servcie = new GroupCinemaServcie();
            var groupCinema = servcie.GetAll();

            return View();
        }

        public ActionResult ManagerGroupCinema()
        {
            return View();
        }

        public JsonResult GetFilmNowShowing()
        {
            FilmService service = new FilmService();

            var films = service.FindBy(f => f.filmStatus == 1).ToList();

            return ConvertListObjectFilmToJson(films);
        }

        //DeletePartner

        public JsonResult DeletePartner(String partnerId)
        {
            PartnerAccountService service = new PartnerAccountService();

            service.Delete(partnerId);

            var listPartnerAccount = service.FindBy(pa => pa.isAvailable == true).ToList();

            return Json(listPartnerAccount, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ConvertListObjectFilmToJson(List<Film> films)
        {
            var obj = films
                .Select(item => new
                {
                    filmId = item.filmId,
                    name = item.name,
                    dateRelease = item.dateRelease.ToString("dd/MM/yyyy"),
                    restricted = item.restricted,
                    filmLength = item.filmLength,
                    imdb = item.imdb,
                    digTypeId = item.digTypeId,
                    author = item.author,
                    movieGenre = item.movieGenre,
                    filmContent = item.filmContent,
                    actorList = item.actorList,
                    countries = item.countries,
                    trailerLink = item.trailerLink,
                    posterPicture = item.posterPicture,
                    additionPicture = item.additionPicture,
                    filmStatus = item.filmStatus
                });
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CreateFilm(int filmId, String filmName, DateTime dateRelease, int restricted, int filmLength,
            String digTypeId, String author, String movieGenre, String actorList, String countries, String trailerLink, String posterPicture,
            String additionPicture, String filmStatus, String filmContent)
        {

            FilmService service = new FilmService();
            int filmStatusInt = 0;
            switch (filmStatus)
            {
                case "Now Showing":
                    filmStatusInt = 1;
                    break;

                case "Coming Soon":
                    filmStatusInt = 0;
                    break;
            }

            if (filmId != 0)
            {
                var film = service.FindByID(filmId);
                film.name = filmName;
                if (dateRelease != null)
                {
                    film.dateRelease = dateRelease;
                }
                film.restricted = restricted;
                film.filmLength = filmLength;
                film.author = author;
                film.movieGenre = movieGenre;
                film.filmContent = filmContent;
                film.actorList = actorList;
                film.countries = countries;
                film.trailerLink = trailerLink;
                film.posterPicture = posterPicture;
                film.additionPicture = additionPicture;
                film.filmStatus = filmStatusInt;

                service.Update(film);
            }
            else
            {
                var film = new Film
                {
                    name = filmName,
                    dateRelease = dateRelease,
                    restricted = restricted,
                    filmLength = filmLength,
                    digTypeId = "2D",
                    imdb = 0,
                    author = author,
                    movieGenre = movieGenre,
                    filmContent = filmContent,
                    actorList = actorList,
                    countries = countries,
                    trailerLink = trailerLink,
                    posterPicture = posterPicture,
                    additionPicture = additionPicture,
                    filmStatus = filmStatusInt
                };

                service.Create(film);
            }

            var films = service.FindBy(f => f.filmStatus == 1).ToList();

            return ConvertListObjectFilmToJson(films);
        }

        public JsonResult GetAllPartner()
        {
            PartnerAccountService service = new PartnerAccountService();

            var listPartnerAccount = service.FindBy(pa => pa.isAvailable == true).ToList();

            return Json(listPartnerAccount, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllGroupCinema()
        {

            using (var context = new CinemaBookingDBEntities())
            {
                var listGroupCinema = context.GroupCinemas.ToList();

                return Json(listGroupCinema, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult CreatePartnerAccount(String partnerId, String partnerPassword, String partnerName, int groupCinemaid, String phone, String email, String available)
        {
            PartnerAccountService service = new PartnerAccountService();

            PartnerAccount partnerAccount = new PartnerAccount
            {
                partnerId = partnerId,
                partnerPassword = partnerPassword,
                phone = phone,
                email = email,
                isAvailable = true,
                groupOfCinemaId = groupCinemaid,
                partnerName = partnerName
            };
            service.Create(partnerAccount);
            var listPartnerAccount = service.FindBy(pa => pa.isAvailable == true).ToList();

            return Json(listPartnerAccount, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdatePartnerAccount(String partnerId, String partnerPassword, String phone, String email, String available)
        {
            PartnerAccountService service = new PartnerAccountService();

            var account = service.FindByID(partnerId);

            if (account != null)
            {
                account.phone = phone;
                account.email = email;

                service.Update(account);
            }

            var listPartnerAccount = service.FindBy(pa => pa.isAvailable == true).ToList();

            return Json(listPartnerAccount, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CreateGroupCinema(int groupId, String groupName, String address, String phone,
                                                                    String email, String logoImg, Double priceDefault)
        {
            GroupCinemaServcie service = new GroupCinemaServcie();

            if (groupId != 0)
            {
                var groupCinemaUpdate = service.FindByID(groupId);

                groupCinemaUpdate.address = address;
                groupCinemaUpdate.email = email;
                groupCinemaUpdate.name = groupName;
                groupCinemaUpdate.logoImg = logoImg;

                service.Update(groupCinemaUpdate);
            }
            else
            {
                GroupCinema groupCinema = new GroupCinema
                {
                    name = groupName,
                    address = address,
                    phone = phone,
                    email = email,
                    logoImg = logoImg
                };

                service.Create(groupCinema);

                TypeOfSeat typeOfSeat = new TypeOfSeat
                {
                    groupId = groupCinema.GroupId,
                    typeName = "vé người lớn",
                    price = priceDefault,
                    capacity = 1
                };

                TypeOfSeatService typeOfSeatService = new TypeOfSeatService();

                typeOfSeatService.Create(typeOfSeat);
            }

            var listGroupCinema = service.GetAll();

            return Json(listGroupCinema, JsonRequestBehavior.AllowGet);
        }
    }

}
