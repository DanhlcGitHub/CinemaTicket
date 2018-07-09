using ManagerApplication.CustomRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace ManagerApplication.Service
{
    interface IUserAccountService
    {
    }
    public class UserAccountService : IUserAccountService
    {
        UserAccountRepository UserAccountRepository = new UserAccountRepository();
        public List<UserAccount> GetAll()
        {
            return UserAccountRepository.GetAll();
        }
        public UserAccount FindByID<E>(E id)
        {
            return UserAccountRepository.FindByID(id);
        }
        public void Create(UserAccount entity)
        {
            UserAccountRepository.Create(entity);
        }
        public void Update(UserAccount entity)
        {
            UserAccountRepository.Update(entity);
        }
        public void Delete<E>(E id)
        {
            UserAccountRepository.Delete(id);
        }
        public List<UserAccount> FindBy(Expression<Func<UserAccount, bool>> predicate)
        {
            return UserAccountRepository.FindBy(predicate);
        }
    }
}