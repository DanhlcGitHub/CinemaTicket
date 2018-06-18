using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Optimization;

namespace ManagerApplication.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new ScriptBundle("~/js").Include(
                      "~/Content/plugins/jQuery/jQuery-2.1.4.min.js",
                      "~/Content/plugins/datatables/jquery.dataTables.min.js",
                      "~/Content/plugins/datatables/dataTables.bootstrap.min.js",
                      "~/Content/bootstrap/js/bootstrap.min.js",
                      "~/Content/plugins/slimScroll/jquery.slimscroll.min.js",
                      "~/Content/plugins/fastclick/fastclick.min.js",
                      "~/Content/dist/js/app.min.js",
                      "~/Content/dist/js/demo.js"
                      ));

            bundles.Add(new StyleBundle("~/css").Include(
                      "~/Content/bootstrap/css/bootstrap.min.css",
                      "~/Content/dist/css/AdminLTE.min.css",
                      "~/Content/plugins/datatables/dataTables.bootstrap.css",
                      "~/Content/dist/css/skins/_all-skins.min.css"
                      ));

            BundleTable.EnableOptimizations = true;
        }
    }
}
