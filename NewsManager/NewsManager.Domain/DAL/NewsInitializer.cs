namespace NewsManager.Domain.DAL
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;

    using NewsManager.Domain.Entities;

    public class NewsInitializer : DropCreateDatabaseIfModelChanges<DBContext>
    {
        protected override void Seed(DBContext context)
        {
            var news = new List<News>
                           {
                               new News
                                   {
                                       NewsID = 1,
                                       Title = "TITLE NEWS 1",
                                       CreatedDate = DateTime.UtcNow,
                                       BodyNews = "BODY NEWS",
                                       Category =
                                           new CategoryNews()
                                            { Name = CategoriesNews.SPORT.ToString() },
                    Status = NewsStatusType.active
                                   },
                               new News
                                   {
                                       NewsID = 2,
                                       Title = "TITLE NEWS 2",
                                       CreatedDate = DateTime.UtcNow,
                                       BodyNews = "THIS IS UNREAD NEWS",
                                       Category =
                                           new CategoryNews()
                                            { Name = CategoriesNews.WEATHER.ToString() },
                    Status = NewsStatusType.inactive
                                   },
                                   new News
                                   {
                                       NewsID = 3,
                                       Title = "TITLE NEWS 3",
                                       CreatedDate = DateTime.UtcNow,
                                       BodyNews = "THIS IS UNREAD NEWS",
                                       Category =
                                           new CategoryNews()
                                            { Name = CategoriesNews.NEWS_WORLD.ToString() },
                    Status = NewsStatusType.inactive
                                   },
                                   new News
                                   {
                                       NewsID = 4,
                                       Title = "TITLE NEWS 4",
                                       CreatedDate = DateTime.UtcNow,
                                       BodyNews = "THIS IS UNREAD NEWS",
                                       Category =
                                           new CategoryNews()
                                            { Name = CategoriesNews.NEWS_WORLD.ToString() },
                    Status = NewsStatusType.inactive
                                   },
                                   new News
                                   {
                                       NewsID = 5,
                                       Title = "TITLE NEWS 5",
                                       CreatedDate = DateTime.UtcNow,
                                       BodyNews = "THIS IS UNREAD NEWS",
                                       Category =
                                           new CategoryNews()
                                            { Name = CategoriesNews.NEWS_WORLD.ToString() },
                    Status = NewsStatusType.inactive
                                   },
                                   new News
                                   {
                                       NewsID = 6,
                                       Title = "TITLE NEWS 6",
                                       CreatedDate = DateTime.UtcNow,
                                       BodyNews = "THIS IS UNREAD NEWS",
                                       Category =
                                           new CategoryNews()
                                            { Name = CategoriesNews.NEWS_WORLD.ToString() },
                    Status = NewsStatusType.inactive
                                   }
                           };
            news.ForEach(s => context.News.Add(s));
            context.SaveChanges();

            //base.Seed(context);
        }
    }
}
