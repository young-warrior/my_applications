using System;
using System.Linq;
using NewsManager.Domain.Abstract;
using NewsManager.Domain.Entities;

namespace NewsManager.Domain.DAL
{
    using System.Collections.Generic;
    using System.Data.Entity;

    public class NewsRepository : INewsRepository
    {
        readonly DBContext _context = new DBContext();

        public IQueryable<News> NewsEntities 
        {
            get { return _context.News.Where(x => x.IsActive); }
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

        public CategoryNews FindCategoryById(int cateogryId)
        {
            return _context.CategoriesNews
                .SingleOrDefault(x => x.CategoryNewsID == cateogryId);
        }

        public IList<News> FindByCategoryId(int cateogryId)
        {
            return NewsEntities.Include(x=>x.Category)
                .Where(x => x.Category.CategoryNewsID == cateogryId)
                .ToList();
        }

        /// <summary>
        /// Soft delete of news.
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            News news = FindById(id);
             var category = this.FindCategoryById(news.Category.CategoryNewsID);
            if (news != null)
            {
                news.IsActive = false;
                category.IsKey = false;
                _context.SaveChanges();
            }
        }

        public void Create(News news)
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
                    //set category as active
                    category.IsActive = true;
                    category.IsKey = true;
                    news.Category = category;
                }
                
                news.CreatedDate = DateTime.UtcNow;
                _context.News.Add(news);
                _context.SaveChanges();
            }
        }
    }
}