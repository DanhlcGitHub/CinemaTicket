using ManagerApplication.BaseRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagerApplication.CustomRepository
{
    interface ITicketRepository
    {

    }
    class TicketRepository : BaseRepository<Ticket>, ITicketRepository
    {

    }
}