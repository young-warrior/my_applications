//using System.Data.Entity;
//using System.Linq;
//using NewsManager.Domain.Abstract;
//using NewsManager.Domain.Entities;
//
//namespace NewsManager.Domain.Utils
//{
//    public class Formator
//    {
//        private readonly INewsRepository repo;
//
//        public News GetFormatTitle(string title)
//        {
//            IQueryable<News> query =
//                repo.NewsEntities.Include(x => x.Title)
//                    .Where(p => p.Title != null)
//                    .OrderBy(p => p.Title)
//                    .SkipWhile(p => p.Title != "L");
//
//
//            return News(query);
//        }
//    }
//}