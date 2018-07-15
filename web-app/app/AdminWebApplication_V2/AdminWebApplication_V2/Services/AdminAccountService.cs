using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AdminWebApplication_V2.Data.Entities;
using AdminWebApplication_V2.Repositories;

namespace AdminWebApplication_V2.Services
{
    interface IAdminAccountService
    {
    }
    public class AdminAccountService : IAdminAccountService
    {
        AdminAccountRepository AdminAccountRepository = new AdminAccountRepository();
        public List<AdminAccount> GetAll()
        {
            return AdminAccountRepository.GetAll();
        }
        public AdminAccount FindByID<E>(E id)
        {
            return AdminAccountRepository.FindByID(id);
        }
        public void Create(AdminAccount entity)
        {
            AdminAccountRepository.Create(entity);
        }
        public void Update(AdminAccount entity)
        {
            AdminAccountRepository.Update(entity);
        }
        public void Delete<E>(E id)
        {
            AdminAccountRepository.Delete(id);
        }
        public List<AdminAccount> FindBy(Expression<Func<AdminAccount, bool>> predicate)
        {
            return AdminAccountRepository.FindBy(predicate);
        }
    }
}
