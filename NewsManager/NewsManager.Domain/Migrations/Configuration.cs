using System.Collections.Generic;
using NewsManager.Domain.DAL;
using NewsManager.Domain.Entities;

namespace NewsManager.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    

    internal sealed class Configuration : DbMigrationsConfiguration<DBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(DBContext context)
        {
            var news = new List<News>
            {
                new News
                {
                    NewsID = 1,
                    Title = "TITLE NEWS 1",
                    CreatedDate = DateTime.Today,
                    BodyNews = "BODY NEWS",
                    Category =
                        new CategoryNews
                        {
                            Name = "SPORT"
                        },
                    Status = NewsStatusType.active
                },
                new News
                {
                    NewsID = 2,
                    Title = "TITLE NEWS 2",
                    CreatedDate = DateTime.Today,
                    BodyNews = "THIS IS UNREAD NEWS",
                    Category =
                        new CategoryNews
                        {
                            Name = "TITLE"
                        },
                    Status = NewsStatusType.inactive
                },
                new News
                {
                    NewsID = 3,
                    Title = "TITLE NEWS 3",
                    CreatedDate = DateTime.Today,
                    BodyNews = "THIS IS UNREAD NEWS",
                    Category =
                        new CategoryNews
                        {
                            Name = "TITLE 3"
                        },
                    Status = NewsStatusType.inactive
                },
                new News
                {
                    NewsID = 4,
                    Title = "TITLE NEWS 4",
                    CreatedDate = DateTime.Today,
                    BodyNews = "THIS IS UNREAD NEWS",
                    Category =
                        new CategoryNews
                        {
                            Name = "WEATHER"
                        },
                    Status = NewsStatusType.inactive
                },
                new News
                {
                    NewsID = 5,
                    Title = "TITLE NEWS 5",
                    CreatedDate = DateTime.Today,
                    BodyNews = "THIS IS UNREAD NEWS",
                    Category =
                        new CategoryNews
                        {
                            Name ="WEATHER"
                        },
                    Status = NewsStatusType.inactive
                },
                new News
                {
                    NewsID = 6,
                    Title = "TITLE NEWS 6",
                    CreatedDate = DateTime.Today,
                    BodyNews = "THIS IS UNREAD NEWS",
                    Category =
                        new CategoryNews
                        {
                            Name = "ART"
                        },
                    Status = NewsStatusType.inactive
                }
            };
        }
    }
}