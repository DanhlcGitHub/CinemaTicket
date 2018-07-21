using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietCineAdmin.Data.Entities;

namespace VietCineAdmin.Repositories
{
    interface IAdminAccountRepository
    {

    }
    class AdminAccountRepository : BaseRepository<AdminAccount>, IAdminAccountRepository
    {

    }
}
