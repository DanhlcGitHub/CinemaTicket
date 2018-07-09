using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace UploadImageApp
{
    public class UploadUtility
    {
        
        public static void Upload(string fileName, string uri)
        {
            // Get the object used to communicate with the server.  
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);
            request.Method = WebRequestMethods.Ftp.UploadFile;

            // This example assumes the FTP site uses anonymous logon. 
            string username = "fptstudent";
            string password = "123456789";//
            request.Credentials = new NetworkCredential(username, password);

            // Copy the contents of the file to the request stream.  
            byte[] uploadContent;
            uploadContent = File.ReadAllBytes(fileName);
            request.ContentLength = uploadContent.Length;

            using (Stream sendStream = request.GetRequestStream())
            {
                sendStream.Write(uploadContent, 0, uploadContent.Length);
            }
            request.GetResponse();
        }

        public static void Upload1(string fileName, string uri)
        {
            // Get the object used to communicate with the server.  
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);
            request.Method = WebRequestMethods.Ftp.UploadFile;

            // This example assumes the FTP site uses anonymous logon. 
            string username = @"CinemaBookingTicket\$CinemaBookingTicket";
            string password = "yANEo72i0QZ0HMLe1lgQxYbMXSREkYZSWnyStt6xSFZRptHJpfJKziWZltwB";//
            request.Credentials = new NetworkCredential(username, password);

            // Copy the contents of the file to the request stream.  
            byte[] uploadContent;
            uploadContent = File.ReadAllBytes(fileName);
            request.ContentLength = uploadContent.Length;

            using (Stream sendStream = request.GetRequestStream())
            {
                sendStream.Write(uploadContent, 0, uploadContent.Length);
            }
            request.GetResponse();
        }
    }
}