using System;
using System.Linq;
using NewsManager.Domain.Abstract;
using NewsManager.Domain.Entities;

namespace NewsManager.Domain.DAL
{
    public class NewsRepository : INewsRepository
    {
        readonly DBContext _context = new DBContext();
        public IQueryable<News> NewsEntities {
            get { return _context.News; }
        }

        public News Update(News news)
        {
            News dbEntity = this.FindById(news.NewsID);
            if (dbEntity != null)
            {
                dbEntity.BodyNews = news.BodyNews;
                dbEntity.Category = news.Category;
                dbEntity.Status = news.Status;
            }

            _context.SaveChanges();
            return dbEntity;
        }

        public News FindById(int id)
        {
            return _context.News.Find(id);
        }

        public void Delete(int id)
        {
            News news = FindById(id);
            if (news != null)
            {
                _context.News.Remove(news);
                _context.SaveChanges();
            }
        }

        public void Add(News news)
        {
            if (news != null)
            {
              // set Created date on  intial create
                news.CreatedDate = DateTime.UtcNow;
                _context.News.Add(news);
                _context.SaveChanges();
            }
        }
    }
}