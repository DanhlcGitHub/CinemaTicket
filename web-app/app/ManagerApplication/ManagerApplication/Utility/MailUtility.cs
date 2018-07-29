using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace ManagerApplication.Utility
{
    public class MailUtility
    {
        public static void SendEmail(string subject, string content, string toEmail)
        {
            string systemEmail = System.Configuration.ConfigurationManager.AppSettings["mail_username"];
            string systemPassword = System.Configuration.ConfigurationManager.AppSettings["mail_password"];
            MailMessage mailMessage = new MailMessage(systemEmail, toEmail);
            mailMessage.Subject = subject;
            mailMessage.Body = content;

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.Credentials = new System.Net.NetworkCredential()
            {
                UserName = systemEmail,
                Password = systemPassword
            };
            smtpClient.EnableSsl = true;
            smtpClient.Send(mailMessage);
        }
    }
}