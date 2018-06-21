using ManagerApplication.CustomRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace ManagerApplication.Service
{
    interface ITypeOfSeatService
    {
    }
    public class TypeOfSeatService : ITypeOfSeatService
    {
        TypeOfSeatRepository typeOfSeatRepository = new TypeOfSeatRepository();
        public List<TypeOfSeat> GetAll()
        {
            return typeOfSeatRepository.GetAll();
        }
        public TypeOfSeat FindByID<E>(E id)
        {
            return typeOfSeatRepository.FindByID(id);
        }
        public void Create(TypeOfSeat entity)
        {
            typeOfSeatRepository.Create(entity);
        }
        public void Update(TypeOfSeat entity)
        {
            typeOfSeatRepository.Update(entity);
        }
        public void Delete<E>(E id)
        {
            typeOfSeatRepository.Delete(id);
        }
        public List<TypeOfSeat> FindBy(Expression<Func<TypeOfSeat, bool>> predicate)
        {
            return typeOfSeatRepository.FindBy(predicate);
        }
    }
}