using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdminWebApplication_V2.Data.Entities;

namespace AdminWebApplication_V2.Repositories
{
    interface ICinemaManagerRepository
    {

    }
    class CinemaManagerRepository : BaseRepository<CinemaManager>, ICinemaManagerRepository
    {

    }
}
