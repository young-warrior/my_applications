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
                                       Title = "TITLE NEWS",
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
                                       Title = "TITLE NEWS FOR NEW NEWS",
                                       CreatedDate = DateTime.UtcNow,
                                       BodyNews = "THIS IS UNREAD NEWS",
                                       Category =
                                           new CategoryNews()
                                            { Name = CategoriesNews.WEATHER.ToString() },
                    Status = NewsStatusType.inactive
                                   }
                           };
            news.ForEach(s => context.News.Add(s));
            context.SaveChanges();

            //base.Seed(context);
        }
    }
}
