using ManagerApplication.BaseRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagerApplication.Repository
{
    interface IFilmRepository
    {

    }
    class FilmRepository : BaseRepository<Film>, IFilmRepository
    {

    }
}