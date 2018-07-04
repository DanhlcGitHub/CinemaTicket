using ManagerApplication.BaseRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagerApplication.CustomRepository
{
    interface IAdminAccountRepository
    {

    }
    class AdminAccountRepository : BaseRepository<AdminAccount>, IAdminAccountRepository
    {

    }
}