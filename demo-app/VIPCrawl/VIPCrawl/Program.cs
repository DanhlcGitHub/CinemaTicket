using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace VIPCrawl
{
    class Program
    {
        static void Main(string[] args)
        {
            PrintData();

            Console.ReadLine();
        }

        private static async Task PrintData()
        {
            List<object> filmList = await StartCrawlerAsync();
        }
        private static async Task<List<object>> StartCrawlerAsync()
        {
            var url = "https://www.imdb.com/movies-coming-soon/2018-07";
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
                    var restristedNode = div.SelectSingleNode("(.//img[@class='absmiddle certimage'])");
                    if(restristedNode!=null)
                        restristed = restristedNode.ChildAttributes("title").FirstOrDefault().Value;
                    var genreNodes = div.SelectNodes("(.//span[@itemprop='genre'])");
                    var genre = "";
                    foreach (var node in genreNodes)
                    {
                        genre+= node.InnerText + ", ";
                    }
                    description = div.SelectSingleNode("(.//div[@itemprop='description'])").InnerText;
                    filmLength = div.SelectSingleNode("(.//time[@itemprop='duration'])").InnerText;
                    author = div.SelectSingleNode("(.//a[@itemprop='url'])[2]").InnerText;
                    var actorNodes = div.SelectNodes("(.//span[@itemprop='actors'])");
                    foreach (var node in actorNodes)
                    {
                        actors += node.InnerText.Trim() + ", ";
                    }
                    trailerLink = div.SelectSingleNode("(.//a[@itemprop='trailer'])").ChildAttributes("href").FirstOrDefault().Value;
                    picture = div.Descendants("img").FirstOrDefault().ChildAttributes("src").FirstOrDefault().Value;
                    name = div.SelectSingleNode("(.//a[@itemprop='url'])").InnerText;
                    filmLength = div.SelectSingleNode("(.//time[@itemprop='duration'])").InnerText;
                    aFilm = new{
                        picture = picture,
                        name = name,
                        filmLength = filmLength,
                        description = description,
                        restristed = restristed,
                        author = author,
                        actors = actors,
                        trailerLink = trailerLink,
                    };
                    filmList.Add(aFilm);
                }
                catch (Exception e)
                {
                   
                   
                }
            }
            return filmList;
        }
    }

}
