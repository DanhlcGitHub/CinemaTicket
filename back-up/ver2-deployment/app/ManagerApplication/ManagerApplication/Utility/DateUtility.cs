﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagerApplication.Utility
{
    public class DateUtility
    {
        public List<DateTime> getSevenDateFromNow(DateTime currentDate)
        {
            List<DateTime> dates = new List<DateTime>();
            dates.Add(currentDate);
            for (int i = 1; i < 7; i++)
            {
                DateTime date = currentDate.AddDays(i);
                dates.Add(date);
            }
            return dates;
        }
    }
}