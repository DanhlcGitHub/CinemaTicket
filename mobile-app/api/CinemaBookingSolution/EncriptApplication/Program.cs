using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace EncriptApplication
{
    class Program
    {
        static void Main(string[] args)
        {
        }

        public static string EncryptString(string s)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(s, "SHA1");
        }
    }
}
