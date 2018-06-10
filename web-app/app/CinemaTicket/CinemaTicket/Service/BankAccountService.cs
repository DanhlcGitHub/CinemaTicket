using CinemaTicket.CustomRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace CinemaTicket.Service
{
    interface IBankAccountService
    {
    }
    public class BankAccountService : IBankAccountService
    {
        BankAccountRepository BankAccountRepository = new BankAccountRepository();
        public List<BankAccount> GetAll()
        {
            return BankAccountRepository.GetAll();
        }
        public BankAccount FindByID<E>(E id)
        {
            return BankAccountRepository.FindByID(id);
        }
        public void Create(BankAccount entity)
        {
            BankAccountRepository.Create(entity);
        }
        public void Update(BankAccount entity)
        {
            BankAccountRepository.Update(entity);
        }
        public void Delete<E>(E id)
        {
            BankAccountRepository.Delete(id);
        }
        public List<BankAccount> FindBy(Expression<Func<BankAccount, bool>> predicate)
        {
            return BankAccountRepository.FindBy(predicate);
        }
    }
}