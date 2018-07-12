using ManagerApplication.BaseRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagerApplication.CustomRepository
{
    interface IRoomRepository
    {
        int CreateRoom(Room entity);
    }
    class RoomRepository : BaseRepository<Room>, IRoomRepository
    {
        public int CreateRoom(Room entity)
        {
            using (var db = new CinemaBookingDBEntities())
            {
                db.Set<Room>().Add(entity);
                db.SaveChanges();
                return entity.roomId;
            }
        }
    }
}