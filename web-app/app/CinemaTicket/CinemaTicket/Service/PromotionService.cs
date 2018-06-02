using CinemaTicket.CustomRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace CinemaTicket.Service
{
    interface IPromotionService
    {
    }
    public class PromotionService : IPromotionService
    {
        PromotionRepository PromotionRepository = new PromotionRepository();
        public List<Promotion> GetAll()
        {
            return PromotionRepository.GetAll();
        }
        public Promotion FindByID<E>(E id)
        {
            return PromotionRepository.FindByID(id);
        }
        public void Create(Promotion entity)
        {
            PromotionRepository.Create(entity);
        }
        public void Update(Promotion entity)
        {
            PromotionRepository.Update(entity);
        }
        public void Delete<E>(E id)
        {
            PromotionRepository.Delete(id);
        }
        public List<Promotion> FindBy(Expression<Func<Promotion, bool>> predicate)
        {
            return PromotionRepository.FindBy(predicate);
        }
    }
}