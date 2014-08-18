using System.Linq;
using NewsManager.Domain.Entities;

namespace NewsManager.Domain.Abstract
{
    using System.Collections.Generic;

    public interface INewsRepository
    {
        IQueryable<News> NewsEntities { get; }

        News Update(News news);
        News FindById(int id);
        IList<News> FindByCategoryId(int cateogryId);
        void Create(News news);
        void Delete(int id);
    }
}
