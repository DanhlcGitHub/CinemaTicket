﻿using CinemaTicket.BaseRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CinemaTicket.CustomRepository
{
    interface IShowTimeRepository
    {

    }
    class ShowTimeRepository : BaseRepository<ShowTime>, IShowTimeRepository
    {

    }
}