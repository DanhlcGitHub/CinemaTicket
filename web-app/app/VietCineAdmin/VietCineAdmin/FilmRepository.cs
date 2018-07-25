using CrawlCinemaFilm;
using ManagerApplication.BaseRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CrawlController.Repository
{
    interface IFilmRepository
    {

    }
    class FilmRepository : BaseRepository<Film>, IFilmRepository
    {

    }
}