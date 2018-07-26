using VietCineAdmin.Services;
using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VietCineAdmin.Constant;

namespace VietCineAdmin.Controllers
{
    public class CrawlController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View("~/Views/Crawl/crawl.cshtml");
        }

        public async Task<JsonResult> CrawlFilmData(string monthParam)
        {
            List<object> filmUrl = await StartCrawlerAsync(monthParam);
            return Json(filmUrl);
        }

        public JsonResult GetCurrentMonth()
        {
            DateTime today = DateTime.Today;
            var obj = new
            {
                month = today.Month,
                year = today.Year
            };
            return Json(obj);
        }

        private async Task<List<object>> StartCrawlerAsync(string monthParam)
        {
            List<Film> filmList = new FilmService().FindBy(f => f.filmStatus != (int)FilmStatus.notAvailable);// 
            int year = DateTime.Today.Year;
            var url = "https://www.imdb.com/movies-coming-soon/" + monthParam;
            var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(url);

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            var divs = htmlDocument.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("itemtype", "")
                        .Equals("http://schema.org/Movie")).ToList();

            List<object> filmUrl = new List<object>();
            foreach (var div in divs)
            {
                var name = "";
                var detailUrl = "";
                try
                {
                    var nameNode = div.SelectSingleNode("(.//a[@itemprop='url'])");
                    if (nameNode != null)
                    {
                        name = nameNode.InnerText;
                        detailUrl = "https://www.imdb.com/" + nameNode.ChildAttributes("href").FirstOrDefault().Value;
                        var obj = new
                        {
                            name = name,
                            url = detailUrl,
                        };
                        if (filmList.Find(f => f.name.Trim() == name.Trim()) == null)
                        {
                            if (name.Contains(year + ""))
                            {
                                filmUrl.Add(obj);
                            }
                        }

                    }
                }
                catch (Exception e)
                {

                }
            }
            return filmUrl;
        }

        public async Task<JsonResult> CrawlFilmObjectAsync(string url, string monthParam)
        {
            object aFilm;

            var httpClient = new HttpClient();
            bool isDateValid = false;
            var description = "";
            var restristed = "";
            var filmLength = "";
            var author = "";
            var actors = "";
            var picture = "";
            var trailerLink = "";
            var name = "";
            var genre = "";
            var imdb = "0";
            var releaseDateStr = "";
            DateTime releaseDate;
            try
            {
                var detailHtml = await httpClient.GetStringAsync(url);
                var detailHtmlDocument = new HtmlDocument();
                detailHtmlDocument.LoadHtml(detailHtml);

                var div = detailHtmlDocument.DocumentNode.Descendants("div")
                    .Where(node => node.GetAttributeValue("id", "")
                            .Equals("title-overview-widget")).FirstOrDefault();

                var releaseDateNode = div.SelectSingleNode("(.//a[@title='See more release dates'])");
                if (releaseDateNode != null)
                {
                    int day = 0;
                    releaseDateStr = releaseDateNode.InnerText.Trim();
                    string dayStr = releaseDateStr.Split(' ')[0].Trim();
                    bool isDayOk = int.TryParse(dayStr, out day);
                    if (isDayOk)
                    {
                        //inforValid = true;
                        releaseDateStr = monthParam + "-" + day;
                        isDateValid = DateTime.TryParse(releaseDateStr, out releaseDate);
                    }

                    if (isDateValid)
                    {
                        var restristedNode = div.SelectSingleNode("(.//meta[@itemprop='contentRating'])");
                        if (restristedNode != null)
                            restristed = restristedNode.InnerText.Trim();
                        var genreNodes = div.SelectNodes("(.//span[@itemprop='genre'])");

                        if (genreNodes != null)
                        {
                            foreach (var node in genreNodes)
                            {
                                genre += node.InnerText + ", ";
                            }
                        }

                        var nameNode = div.SelectSingleNode("(.//h1[@itemprop='name'])");
                        if (nameNode != null)
                        {
                            name = nameNode.InnerText.Trim();
                            name = name.Replace("&nbsp;", " ");
                        }


                        var descriptionNode = div.SelectSingleNode("(.//div[@class='summary_text'])");
                        if (descriptionNode != null)
                            description = descriptionNode.InnerText;

                        var imdbNode = div.SelectSingleNode("(.//span[@itemprop='ratingValue'])");
                        if (imdbNode != null)
                            imdb = imdbNode.InnerText.Trim();

                        var filmLengthNode = div.SelectSingleNode("(.//time[@itemprop='duration'])");
                        if (filmLengthNode != null)
                            filmLength = filmLengthNode.InnerText;

                        var authorNode = div.SelectSingleNode("(.//span[@itemprop='director'])");
                        if (authorNode != null)
                            author = authorNode.InnerText;

                        var actorNodes = div.SelectNodes("(.//span[@itemprop='actors'])");
                        foreach (var node in actorNodes)
                        {
                            actors += node.InnerText.Trim() + ", ";
                        }

                        var trailerLinkNode = div.SelectSingleNode("(.//a[@itemprop='trailer'])");
                        if (trailerLinkNode != null)
                            trailerLink = trailerLinkNode.ChildAttributes("href").FirstOrDefault().Value;

                        var pictureNode = div.SelectSingleNode("(.//img[@itemprop='image'])");
                        if (pictureNode != null)
                            picture = pictureNode.ChildAttributes("src").FirstOrDefault().Value;


                        aFilm = new
                        {
                            picture = picture,
                            name = name,
                            imdb = imdb,
                            genre = genre,
                            filmLength = filmLength,
                            description = description,
                            restristed = restristed,
                            author = author,
                            actors = actors,
                            trailerLink = "https://www.imdb.com/" + trailerLink,
                            releaseDate = releaseDateStr,
                        };
                        return Json(aFilm);
                    }
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public JsonResult SaveFilmToDB()
        {
            List<Film> filmList = new FilmService().FindBy(f => f.filmStatus != (int)FilmStatus.notAvailable);// 
            List<object> errorList = new List<object>();

            string addedFilmList = Request.Params["addedFilmList"];
            JArray dataArray = JArray.Parse(addedFilmList);
            foreach (JObject data in dataArray)
            {
                Film aFilm = new Film();
                DateTime releaseDate;
                
                int filmLength = 0;
                int restricted = 0;
                double imdb = 0;
                string description = (string)data.GetValue("description");
                string name = (string)data.GetValue("name");
                string genre = (string)data.GetValue("genre");
                string filmLengthStr = (string)data.GetValue("filmLength");
                string author = (string)data.GetValue("author");
                string actors = (string)data.GetValue("actors");
                string picture = (string)data.GetValue("picture");
                string trailerLink = (string)data.GetValue("trailerLink");
                string restristedStr = (string)data.GetValue("restristed");
                string imdbStr = (string)data.GetValue("imdb");
                string releaseDateStr = (string)data.GetValue("releaseDate");

                aFilm.filmContent = description;
                aFilm.name = name;
                aFilm.movieGenre = genre;
                aFilm.author = author;
                aFilm.actorList = actors;
                aFilm.additionPicture = picture + ";";
                aFilm.trailerLink = trailerLink;
                aFilm.posterPicture = "https://www.valmorgan.com.au/wp-content/uploads/2016/06/default-movie-1-3.jpg";
                aFilm.digTypeId = "1";
                if (filmLengthStr != null && filmLengthStr.Contains("min"))// 111 min
                {
                    filmLengthStr = filmLengthStr.Split(' ')[0].Trim();
                    int.TryParse(filmLengthStr, out filmLength);
                    aFilm.filmLength = filmLength;
                }

                if (imdbStr != null)// 111 min
                {
                    imdbStr = imdbStr.Split(' ')[0].Trim();
                    bool flag = double.TryParse(imdbStr, out imdb);
                    if (flag) 
                        aFilm.imdb = imdb;
                }

                if (restristedStr != null && restristedStr.Contains("PG-"))// 111 min
                {
                    restristedStr = restristedStr.Split('-')[1].Trim();
                    int.TryParse(restristedStr, out restricted);
                    aFilm.restricted = restricted;
                }
                if (releaseDateStr != null)
                {
                    bool validDate = DateTime.TryParse(releaseDateStr, out  releaseDate);
                    if (validDate)
                    {
                        aFilm.dateRelease = releaseDate;
                        if (releaseDate > DateTime.Today)
                        {
                            aFilm.filmStatus = (int) FilmStatus.upcomingMovie;
                        }
                        else
                        {
                            aFilm.filmStatus = (int)FilmStatus.showingMovie;
                        }

                        if (filmList.Find(f => f.name == name) == null)
                        {
                            try
                            {
                                new FilmService().Create(aFilm);
                            }
                            catch (Exception e)
                            {
                                var errObj = new
                                {
                                    picture = picture,
                                    name = name,
                                    imdb = imdb,
                                    genre = genre,
                                    filmLength = filmLengthStr,
                                    description = description,
                                    restristed = restristedStr,
                                    author = author,
                                    actors = actors,
                                    trailerLink = trailerLink,
                                };
                                errorList.Add(errObj);
                            }
                        }
                        else
                        {
                            var errObj = new
                            {
                                picture = picture,
                                name = name,
                                imdb = imdb,
                                genre = genre,
                                filmLength = filmLengthStr,
                                description = description,
                                restristed = restristedStr,
                                author = author,
                                actors = actors,
                                trailerLink = trailerLink,
                            };
                            errorList.Add(errObj);
                        }
                    }
                }
                else
                {
                    errorList.Add(aFilm);
                }
            }

            return Json(errorList);
        }

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
                    filmStatus = item.filmStatus | 1,
                    trailerUrl = item.trailerLink,
                    imdb = item.imdb == null ? 0 : item.imdb,
                    dateRelease = item.dateRelease,
                    restricted = item.restricted == null ? 0 : item.restricted,
                    img = item.additionPicture.Split(';')[0],
                    length = item.filmLength,
                    star = new string[(int)Math.Ceiling((double)item.imdb / 2)]
                });
            return Json(obj);
        }


    }
}
