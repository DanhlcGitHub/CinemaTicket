using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaBookingCore.Data.Models
{
    public class MessageModel
    {

        public String to { get; set; }
        public NotificationModel notification { get; set; }
        public Dictionary<string, string> data { get; set; }
    }
}
