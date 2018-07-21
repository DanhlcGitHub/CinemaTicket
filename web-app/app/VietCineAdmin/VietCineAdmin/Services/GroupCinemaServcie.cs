using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VietCineAdmin.Data.Entities;
using VietCineAdmin.Repositories;

namespace VietCineAdmin.Services
{
    interface IGroupCinemaServcie
    {
    }
    public class GroupCinemaServcie : IGroupCinemaServcie
    {
        GroupCinemaRepository groupCinemaRepository = new GroupCinemaRepository();
        public List<GroupCinema> GetAll()
        {
            return groupCinemaRepository.GetAll();
        }
        public GroupCinema FindByID<E>(E id)
        {
            return groupCinemaRepository.FindByID(id);
        }
        public void Create(GroupCinema entity)
        {
            groupCinemaRepository.Create(entity);
        }
        public void Update(GroupCinema entity)
        {
            groupCinemaRepository.Update(entity);
        }
        public void Delete<E>(E id)
        {
            groupCinemaRepository.Delete(id);
        }
        public List<GroupCinema> FindBy(Expression<Func<GroupCinema, bool>> predicate)
        {
            return groupCinemaRepository.FindBy(predicate);

        }
    }
}
