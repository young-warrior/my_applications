using System;
using System.Linq;
using NewsManager.Domain.Abstract;
using NewsManager.Domain.Entities;
using NewsManager.Domain.DAL;

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
                dbEntity.Title = news.Title;
                dbEntity.BodyNews = news.BodyNews;
                dbEntity.Category = this.FindCategoryById(news.Category.CategoryNewsID);
                dbEntity.Status = news.Status;
            }

            _context.SaveChanges();
            return dbEntity;
        }

        public News FindById(int id)
        {
            return _context.News.Find(id);
        }

        public CategoryNews FindCategoryByName(String cateogry)
        {
            return _context.CategoriesNews.SingleOrDefault(x => x.Name == cateogry);
            
        }

        public CategoryNews FindCategoryById(int cateogryId)
        {
            return _context.CategoriesNews
                .SingleOrDefault(x => x.CategoryNewsID == cateogryId);
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
               // Sets "Created date" on initial create
                var category = this.FindCategoryById(news.Category.CategoryNewsID);
                if (category == null)
                {
                    news.Category = _context.CategoriesNews.Add(new CategoryNews()
                                                    {
                                                        Name = news.Category.Name
                                                    });
                }
                else
                {
                    news.Category = category;
                }

                news.CreatedDate = DateTime.UtcNow;
                _context.News.Add(news);
                _context.SaveChanges();
            }
        }
    }
}