namespace NewsManager.Domain.DAL
{
    using System.Collections.Generic;
    using System.Linq;

    using NewsManager.Domain.Entities;

    public interface ICategoryNewsRepository
    {
        IQueryable<CategoryNews> CategoryNewsEntities { get; }

        CategoryNews Update(CategoryNews category);

        CategoryNews FindById(int id);

        void Add(CategoryNews category);

        void Delete(int id);

        void AddOrUpdate(CategoryNews category);
    }
}