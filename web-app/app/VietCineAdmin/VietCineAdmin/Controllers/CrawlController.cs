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
            List<Film> allFilmList = new FilmService().GetAll();
            int year = DateTime.Today.Year;
            var url = "https://www.imdb.com/movies-coming-soon/" + monthParam;
            var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(url);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);
            var divs = htmlDocument.DocumentNode.Descendants("div")
               .Where(node => node.GetAttributeValue("itemtype", "")
                       .Equals("http://schema.org/Movie")).ToList();
            List<object> filmList = new List<object>();
            foreach (var div in divs)
            {
                object aFilm = null;
                try
                {
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
                    var releaseDate = monthParam;
                    var restristedNode = div.SelectSingleNode("(.//img[@class='absmiddle certimage'])");
                    if (restristedNode != null)
                        restristed = restristedNode.ChildAttributes("title").FirstOrDefault().Value;

                    var genreElement = div.SelectSingleNode("(.//p[@class='cert-runtime-genre'])");
                    var genreNodes = genreElement.SelectNodes(".//span");

                    if (genreNodes != null)
                    {
                        foreach (var node in genreNodes)
                        {
                            genre += node.InnerText;
                        }
                    }

                    var descriptionNode = div.SelectSingleNode("(.//div[@class='outline'])");
                    if (descriptionNode != null)
                        description = descriptionNode.InnerText.Trim();
                    var imdbNode = div.SelectSingleNode("(.//div[@class='rating_txt'])");
                    if (imdbNode != null)
                        imdb = imdbNode.InnerText.Replace("\n", "").Replace("Metascore", "").Trim();
                    var filmLengthNode = div.SelectSingleNode(".//time");
                    if (filmLengthNode != null)
                        filmLength = filmLengthNode.InnerText;
                    var authorNode = div.SelectSingleNode("(.//div[@class='txt-block'])[1]");
                    if (authorNode != null)
                        author = authorNode.InnerText;
                    author = author.Replace("\n", "").Replace("Director:", "")
                        .Replace("Directors:", "").Replace(" ", "").Replace("|", ", ");
                    var actorNodes = div.SelectNodes("(.//div[@class='txt-block'])[2]");
                    if (actorNodes != null)
                    {
                        foreach (var node in actorNodes)
                        {
                            actors += node.InnerText.Trim() + ", ";
                        }
                    }
                    actors = actors.Replace("\n", "").Replace("Stars:", "").Replace("Star:", "");

                    var trailerLinkNode = div.SelectSingleNode("(.//a[@itemprop='trailer'])");
                    if (trailerLinkNode != null)
                        trailerLink = trailerLinkNode.ChildAttributes("href").FirstOrDefault().Value;
                    var pictureNode = div.Descendants("img");
                    if (pictureNode != null)
                        picture = pictureNode.FirstOrDefault().ChildAttributes("src").FirstOrDefault().Value;
                    var nameNode = div.SelectSingleNode("(.//img[@class='poster shadowed'])");
                    if (nameNode != null)
                        name = nameNode.ChildAttributes("title").FirstOrDefault().Value;
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
                        releaseDate = releaseDate,
                    };
                    if (name.Contains(year + "") && !isFilmExist(name, allFilmList))
                        filmList.Add(aFilm);

                }
                catch (Exception e)
                {
                }
            }
            return filmList;
        }

        private bool isFilmExist(string name,List<Film> filmList)
        {
            foreach (var item in filmList)
            {
                if (name.Trim().Equals(item.name.Trim())) return true;
            }
            return false;
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
                    bool flag = double.TryParse(imdbStr, out imdb);
                    if (flag) 
                        aFilm.imdb = imdb;
                }

                if (restristedStr != null && restristedStr.Contains("PG-"))// 111 min
                {
                    restristedStr = restristedStr.Split('-')[1].Trim();
                    int.TryParse(restristedStr, out restricted);
                    if (restricted == null) restricted = 0;
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
