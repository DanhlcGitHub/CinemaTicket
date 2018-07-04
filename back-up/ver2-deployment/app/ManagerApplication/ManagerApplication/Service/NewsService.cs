using ManagerApplication.CustomRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace ManagerApplication.Service
{
    interface INewsService
    {
    }
    public class NewsService : INewsService
    {
        NewsRepository NewsRepository = new NewsRepository();
        public List<News> GetAll()
        {
            return NewsRepository.GetAll();
        }
        public News FindByID<E>(E id)
        {
            return NewsRepository.FindByID(id);
        }
        public void Create(News entity)
        {
            NewsRepository.Create(entity);
        }
        public void Update(News entity)
        {
            NewsRepository.Update(entity);
        }
        public void Delete<E>(E id)
        {
            NewsRepository.Delete(id);
        }
        public List<News> FindBy(Expression<Func<News, bool>> predicate)
        {
            return NewsRepository.FindBy(predicate);
        }
    }
}