﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietCineAdmin.Repositories
{
    interface ICinemaRepository
    {
    }
    class CinemaRepository : BaseRepository<Cinema>, ICinemaRepository
    {

        
    }
}