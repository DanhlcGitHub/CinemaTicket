using CinemaTicket.BaseRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CinemaTicket.CustomRepository
{
    interface ITicketRepository
    {

    }
    class TicketRepository : BaseRepository<Ticket>, ITicketRepository
    {

    }
}