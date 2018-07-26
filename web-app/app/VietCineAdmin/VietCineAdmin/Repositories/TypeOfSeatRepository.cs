using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VietCineAdmin.Repositories;

namespace VietCineAdmin.CustomRepository
{
    interface ITypeOfSeatRepository
    {

    }
    class TypeOfSeatRepository : BaseRepository<TypeOfSeat>, ITypeOfSeatRepository
    {

    }
}