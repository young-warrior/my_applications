using System;

namespace NewsManager.Domain.DAL
{
    using System.Linq;

    using NewsManager.Domain.Entities;

    public class CategoryNewsRepository : ICategoryNewsRepository
    {
        readonly DBContext context = new DBContext();

        public IQueryable<CategoryNews> CategoryNewsEntities
        {
            get { return this.context.CategoriesNews; }
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
            return context.CategoriesNews.SingleOrDefault(x => x.Name == category);

        }

        public void Delete(int id)
        {
            CategoryNews news = FindById(id);
            if (news != null)
            {
                context.CategoriesNews.Remove(news);
                context.SaveChanges();
            }
        }

        public void Add(CategoryNews news)
        {
            if (news != null)
            {
                // Sets "Created date" on initial create
                var category = this.FindCateoryByName(news.Name);
                if (category == null)
                {
                    news = context.CategoriesNews.Add(new CategoryNews()
                    {
                        Name = news.Name
                    });
                }
                else
                {
                    news = category;
                }
                context.CategoriesNews.Add(news);
                context.SaveChanges();
            }
        }
    }
}