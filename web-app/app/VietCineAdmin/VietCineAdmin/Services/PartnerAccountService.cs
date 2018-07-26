using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VietCineAdmin.Repositories;

namespace VietCineAdmin.Services
{
    interface IPartnerAccountService
    {
    }
    public class PartnerAccountService : IPartnerAccountService
    {
        PartnerAccountRepository PartnerAccountRepository = new PartnerAccountRepository();
        public List<PartnerAccount> GetAll()
        {
            return PartnerAccountRepository.GetAll();
        }
        public PartnerAccount FindByID<E>(E id)
        {
            return PartnerAccountRepository.FindByID(id);
        }
        public void Create(PartnerAccount entity)
        {
            PartnerAccountRepository.Create(entity);
        }
        public void Update(PartnerAccount entity)
        {
            PartnerAccountRepository.Update(entity);
        }
        public void Delete<E>(E id)
        {
            PartnerAccountRepository.Delete(id);
        }
        public List<PartnerAccount> FindBy(Expression<Func<PartnerAccount, bool>> predicate)
        {
            return PartnerAccountRepository.FindBy(predicate);
        }
    }
}
