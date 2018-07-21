using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AdminWebApplication_V2.Controllers
{
    public class CrawlDataController : Controller
    {
        //function show view
        //file view: /Views/CrawlData/CrawlData.cshtml
        //file js: /Content/js/crawl.js
        public ActionResult CrawlData()
        {
            //list crawl data in here
            return View();
        }

    }
}
