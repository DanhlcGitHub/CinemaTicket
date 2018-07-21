using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietCineAdmin.Data.Entities;

namespace VietCineAdmin.Repositories
{
    interface IFilmRepository
    {

    }
    class FilmRepository : BaseRepository<Film>, IFilmRepository
    {

    }
}
