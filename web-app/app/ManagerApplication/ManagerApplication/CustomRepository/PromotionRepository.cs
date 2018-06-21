using ManagerApplication.BaseRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagerApplication.CustomRepository
{
    interface IPromotionRepository
    {

    }
    class PromotionRepository : BaseRepository<Promotion>, IPromotionRepository
    {

    }
}