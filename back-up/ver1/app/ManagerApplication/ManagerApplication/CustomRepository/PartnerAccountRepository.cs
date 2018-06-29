using ManagerApplication.BaseRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagerApplication.CustomRepository
{
    interface IPartnerAccountRepository
    {

    }
    class PartnerAccountRepository : BaseRepository<PartnerAccount>, IPartnerAccountRepository
    {

    }
}