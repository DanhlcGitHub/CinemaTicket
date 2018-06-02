using CinemaTicket.CustomRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace CinemaTicket.Service
{
    interface IBankListService
    {
    }
    public class BankListService : IBankListService
    {
        BankListRepository BankListRepository = new BankListRepository();
        public List<BankList> GetAll()
        {
            return BankListRepository.GetAll();
        }
        public BankList FindByID<E>(E id)
        {
            return BankListRepository.FindByID(id);
        }
        public void Create(BankList entity)
        {
            BankListRepository.Create(entity);
        }
        public void Update(BankList entity)
        {
            BankListRepository.Update(entity);
        }
        public void Delete<E>(E id)
        {
            BankListRepository.Delete(id);
        }
        public List<BankList> FindBy(Expression<Func<BankList, bool>> predicate)
        {
            return BankListRepository.FindBy(predicate);
        }
    }
}