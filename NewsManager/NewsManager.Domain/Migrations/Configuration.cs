using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using NewsManager.Domain.DAL;
using NewsManager.Domain.Entities;

namespace NewsManager.Domain.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<DBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(DBContext context)
        {
            try
            {
                var categories = new List<CategoryNews>
                {
                    new CategoryNews
                    {
                        CategoryNewsID = 1,
                        Name = "SPORT"
                    },
                    new CategoryNews
                    {
                        CategoryNewsID = 2,
                        Name = "TITLE"
                    },
                    new CategoryNews
                    {
                        CategoryNewsID = 3,
                        Name = "TITLE 3"
                    },
                    new CategoryNews
                    {
                        CategoryNewsID = 4,
                        Name = "WEATHER"
                    },
                    new CategoryNews
                    {
                        CategoryNewsID = 5,
                        Name = "ART"
                    },
                    new CategoryNews
                    {
                        CategoryNewsID = 6,
                        Name = "ARTasda"
                    },
                };

                var news = new List<News>
                {
                    new News
                    {
                        NewsID = 1,
                        Title = "TITLE NEWS 1",
                        CreatedDate = DateTime.Today,
                        BodyNews = "BODY NEWS",
                        Category = categories[0],
                        Status = NewsStatusType.active,
                    },
                    new News
                    {
                        NewsID = 2,
                        Title = "TITLE NEWS 2",
                        CreatedDate = DateTime.Today,
                        BodyNews = "THIS IS UNREAD NEWS",
                        Category = categories[1],
                        Status = NewsStatusType.inactive
                    },
                    new News
                    {
                        NewsID = 3,
                        Title = "TITLE NEWS 3",
                        CreatedDate = DateTime.Today,
                        BodyNews = "THIS IS UNREAD NEWS",
                        Category = categories[2],
                        Status = NewsStatusType.inactive
                    },
                    new News
                    {
                        NewsID = 4,
                        Title = "TITLE NEWS 4",
                        CreatedDate = DateTime.Today,
                        BodyNews = "THIS IS UNREAD NEWS",
                        Category = categories[3],
                        Status = NewsStatusType.inactive
                    },
                    new News
                    {
                        NewsID = 5,
                        Title = "TITLE NEWS 5",
                        CreatedDate = DateTime.Today,
                        BodyNews = "THIS IS UNREAD NEWS",
                        Category = categories[4],
                        Status = NewsStatusType.inactive
                    },
                    new News
                    {
                        NewsID = 6,
                        Title = "TITLE NEWS 6",
                        CreatedDate = DateTime.Today,
                        BodyNews = "THIS IS UNREAD NEWS",
                        Category = categories[5],
                        Status = NewsStatusType.inactive
                    }
                };
                context.CategoriesNews.AddOrUpdate(c => c.CategoryNewsID, categories.ToArray());
                context.News.AddOrUpdate(s => s.NewsID, news.ToArray());

                context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (DbEntityValidationResult eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (DbValidationError ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }
    }
}