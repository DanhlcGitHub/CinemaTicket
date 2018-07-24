using CinemaTicket.Constant;
using CinemaTicket.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CinemaTicket.Controllers
{
    public class FilmController : Controller
    {
        //
        // GET: /Film/

        [HttpPost]
        public JsonResult LoadAvailableFilm()
        {
            int x = (int)FilmStatus.showingMovie;
            FilmService filmService = new FilmService();
            List<Film> filmList = filmService.FindBy(f => f.filmStatus != (int)FilmStatus.notAvailable);// 
            var obj = filmList
                .Select(item => new
                {
                    id = item.filmId,
                    name = item.name,
                    filmStatus = item.filmStatus,
                    trailerUrl = item.trailerLink,
                    imdb = item.imdb,
                    dateRelease = item.dateRelease,
                    restricted = item.restricted,
                    img = item.additionPicture.Split(';')[0],
                    length = item.filmLength,
                    star = new string[(int)Math.Ceiling((double)item.imdb / 2)]
                });
            return Json(obj);
        }
        [HttpPost]
        public JsonResult LoadFilmById(string filmId)
        {
            FilmService filmService = new FilmService();
            Film item = filmService.FindByID(Convert.ToInt32(filmId));
            string serverPath = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            string additionPictureImage = item.additionPicture;
            if (additionPictureImage != null)
            {
                if (additionPictureImage.Contains("http"))
                {
                    additionPictureImage = item.additionPicture.Split(';')[0];
                }
                else
                {
                    additionPictureImage = serverPath + item.additionPicture.Split(';')[0];
                }
            }

            var obj = new
            {
                id = item.filmId,
                name = item.name,
                filmStatus = item.filmStatus,
                trailerUrl = item.trailerLink,
                imdb = item.imdb,
                dateRelease = String.Format("{0:dd/MM/yyyy}", item.dateRelease),
                restricted = item.restricted,
                img = additionPictureImage,
                length = item.filmLength,
                star = new string[(int)Math.Ceiling((double)item.imdb / 2)],
                author = item.author,
                actorList = item.actorList,
                movieGenre = item.movieGenre,
                countries = item.countries,
                content = item.filmContent,
            };
            return Json(obj);
        }
    }
}
