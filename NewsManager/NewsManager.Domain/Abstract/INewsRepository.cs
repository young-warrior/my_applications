using System.Linq;
using NewsManager.Domain.Entities;

namespace NewsManager.Domain.Abstract
{
    public interface INewsRepository
    {
        IQueryable<News> NewsEntities { get; }

        News Update(News news);
        News FindById(int id);
        void Add(News news);
        void Delete(int id);
    }
}
