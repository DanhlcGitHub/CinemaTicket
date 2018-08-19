using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaBookingCore.Data.Models
{
    public class AccountModel
    {
        public string UserId { get; set; }
        public string UserPassword { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public Boolean IsExited { get; set; }


    }
}
