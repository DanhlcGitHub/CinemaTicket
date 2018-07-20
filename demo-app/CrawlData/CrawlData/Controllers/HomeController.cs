using CrawlData.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CrawlData.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            string url = "https://www.imdb.com/movies-coming-soon/2018-07";
            string data = new CrawlUtility().CloneWeb(url);
            string filePath = Server.MapPath(Url.Content("~/Content/foo.xml"));
            FormatDataUtility format = new FormatDataUtility();
            data = format.formatScript(data);

            System.IO.File.WriteAllText(filePath, data, Encoding.ASCII);

            //string filePath = Server.MapPath(Url.Content("~/Content/data.xml"));
            XMLHandler handler = new XMLHandler();
            handler.LoadTransactionFile(filePath);

            return View();
        }

    }
}
