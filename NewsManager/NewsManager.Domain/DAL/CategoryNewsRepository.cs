using System;
using System.IO;
using System.Runtime.Remoting.Messaging;

namespace NewsManager.Domain.DAL
{
    
    using System.Linq;

    using NewsManager.Domain.Abstract;
    using NewsManager.Domain.Entities;

    public class CategoryNewsRepository : ICategoryNewsRepository
    {
        private readonly INewsRepository newsRepo;

        readonly DBContext context = new DBContext();

        public CategoryNewsRepository(INewsRepository newsRepo)
        {
            this.newsRepo = newsRepo;
        }

        public IQueryable<CategoryNews> CategoryNewsEntities
        {
            get
            {
                return this.context.CategoriesNews.Where(x=>x.IsActive);
            }
        }

        public CategoryNews Update(CategoryNews news)
        {
            CategoryNews dbEntity = this.FindById(news.CategoryNewsID);
            if (dbEntity != null)
            {
                
                dbEntity.CategoryNewsID = news.CategoryNewsID;
                dbEntity.Name = news.Name;
            }

            context.SaveChanges();
            return dbEntity;
        }

        public CategoryNews FindById(int id)
        {
            return context.CategoriesNews.Find(id);
        }

        public CategoryNews FindCateoryByName(String category)
        {
            
            return context.CategoriesNews.SingleOrDefault(b => b.Name == category); 

        }

        /// <summary>
        /// Soft delete of categories
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            CategoryNews category = FindById(id);
            if (category != null)
            {
                category.IsActive = false;

                foreach (var news in newsRepo.FindByCategoryId(id))
                {
                    newsRepo.Delete(news.NewsID);
                }

                context.SaveChanges();
            }
        }

        public void CreateOrUpdate(CategoryNews category)
        {
            if (category.CategoryNewsID == 0)
            {
                this.Create(category);
            }
            else
            {
                this.Update(category);
            }
        }

        public void Create(CategoryNews news)
        {
            if (news != null)
            {
                // Sets "Created date" on initial create
                var category = this.FindCateoryByName(news.Name);
                if (category == null)
                {
                    news = context.CategoriesNews.Add(new CategoryNews() { Name = news.Name });
                    context.CategoriesNews.Add(news);
                }
                else
                {
                    category.IsActive = true;
                    
                }

            }
            
            context.SaveChanges();
        }
    }
}