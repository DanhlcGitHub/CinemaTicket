using CinemaTicket.BaseRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CinemaTicket.CustomRepository
{
    interface IPartnerAccountRepository
    {

    }
    class PartnerAccountRepository : BaseRepository<PartnerAccount>, IPartnerAccountRepository
    {

    }
}