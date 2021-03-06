﻿using ManagerApplication.CustomRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace ManagerApplication.Service
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