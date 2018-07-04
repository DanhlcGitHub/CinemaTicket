using CinemaTicket.BaseRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CinemaTicket.Repository
{
    interface IFilmRepository
    {

    }
    class FilmRepository : BaseRepository<Film>, IFilmRepository
    {

    }
}