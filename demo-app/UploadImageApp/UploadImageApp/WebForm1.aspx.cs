using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UploadImageApp
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string filename = @"C:\Users\hi\Pictures\Screenshots\pic1.png";
            string uriString = @"ftp://fptstudent.000webhostapp.com/public_html/Lotteria/pic1.png";
            UploadUtility.Upload(filename, uriString);
        }
    }
}