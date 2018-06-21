using ManagerApplication.BaseRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagerApplication.CustomRepository
{
    interface IRoomRepository
    {

    }
    class RoomRepository : BaseRepository<Room>, IRoomRepository
    {

    }
}