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
    interface IDigitalTypeService
    {
    }
    public class DigitalTypeService : IDigitalTypeService
    {
        DigitalTypeRepository digitalTypeRepository = new DigitalTypeRepository();
        public List<DigitalType> GetAll()
        {
            return digitalTypeRepository.GetAll();
        }
        public DigitalType FindByID<E>(E id)
        {
            return digitalTypeRepository.FindByID(id);
        }
        public void Create(DigitalType entity)
        {
            digitalTypeRepository.Create(entity);
        }
        public void Update(DigitalType entity)
        {
            digitalTypeRepository.Update(entity);
        }
        public void Delete<E>(E id)
        {
            digitalTypeRepository.Delete(id);
        }
        public List<DigitalType> FindBy(Expression<Func<DigitalType, bool>> predicate)
        {
            return digitalTypeRepository.FindBy(predicate);
        }
    }
}
