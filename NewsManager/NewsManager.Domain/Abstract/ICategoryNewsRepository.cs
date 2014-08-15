using System.Linq;
using NewsManager.Domain.Entities;

namespace NewsManager.Domain.DAL
{
    public interface ICategoryNewsRepository
    {
        IQueryable<CategoryNews> CategoryNewsEntities { get; }
        CategoryNews Update(CategoryNews category);
        CategoryNews FindById(int id);
        void Add(CategoryNews category);
        void Delete(int id);
    }
}