using ManagerApplication.BaseRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagerApplication.CustomRepository
{
    interface ICinemaManagerRepository
    {
        List<CinemaManager> FindCinemaManagerByGroupId(int groupId);
    }
    class CinemaManagerRepository : BaseRepository<CinemaManager>, ICinemaManagerRepository
    {
        public List<CinemaManager> FindCinemaManagerByGroupId(int groupId)
        {
            using (var db = new CinemaBookingDBEntities())
            {
                List<CinemaManager> list = db.CinemaManagers.Include("Cinema")
                                    .Where(s => s.Cinema.groupId == groupId
                                            && s.isAvailable == true).ToList();
                return list;
            }
        }
    }
}