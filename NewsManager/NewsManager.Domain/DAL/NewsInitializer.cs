using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewsManager.Domain.Entities;

namespace NewsManager.Domain.DAL
{
    public class NewsInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<DBContext>
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
                    Category = CategoriesNews.SPORT.ToString(),
                    Status = NewsStatusType.Read
                },

                new News
                {
                    NewsID = 2,
                    Title = "TITLE NEWS FOR NEW NEWS",
                    CreatedDate = DateTime.UtcNow,
                    BodyNews = "THIS IS UNREAD NEWS",
                    Category = CategoriesNews.WEATHER.ToString(),
                    Status = NewsStatusType.Unread
                }
            };
            news.ForEach(s => context.News.Add(s));
            context.SaveChanges();
            
            //base.Seed(context);
        }
    }


}
