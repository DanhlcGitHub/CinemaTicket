using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace VietCineAdmin.Utility
{
    public class EncryptUtility
    {
        public static string EncryptString(string s)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(s, "SHA1");
        }
    }
}