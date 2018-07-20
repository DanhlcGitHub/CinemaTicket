using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace ManagerApplication.Utility
{
    public class UploadUtility
    {
        public static bool Upload(byte[] uploadContent, string uri)
        {
            try
            {
                // Get the object used to communicate with the server.  
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);
                request.Method = WebRequestMethods.Ftp.UploadFile;

                // This example assumes the FTP site uses anonymous logon. 
                string username = @"CinemaBookingTicket\$CinemaBookingTicket";
                string password = "yANEo72i0QZ0HMLe1lgQxYbMXSREkYZSWnyStt6xSFZRptHJpfJKziWZltwB";//
                request.Credentials = new NetworkCredential(username, password);

                request.ContentLength = uploadContent.Length;

                using (Stream sendStream = request.GetRequestStream())
                {
                    sendStream.Write(uploadContent, 0, uploadContent.Length);
                }
                request.GetResponse();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
            
        }
    }
}