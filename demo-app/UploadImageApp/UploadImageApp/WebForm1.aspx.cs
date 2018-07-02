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
            // path của cái hình lấy từ máy mình
            string localPath = @"C:\Users\hi\Pictures\Screenshots\pic1.png";
            
            // cái uri string phải cắt tên cái hình ra rồi + vào như bên dưới
            // @"ftp://files.000webhost.com/public_html/CinemaImage/" + imagename
            // imageName ở đây là pic1.png
            string uriString = @"ftp://files.000webhost.com/public_html/CinemaImage/pic1.png";

            // hàm hỗ trợ upload
            UploadUtility.Upload(localPath, uriString);
            
            ///https://fptstudent.000webhostapp.com/CinemaImage/pic1.png dòng này bị comment
            /// chuỗi ở trên là chuỗi lưu xuống db
        }
    }
}