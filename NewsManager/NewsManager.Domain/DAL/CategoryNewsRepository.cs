namespace NewsManager.Domain.DAL
{
    using System.Linq;

    using NewsManager.Domain.Entities;

    public class CategoryNewsRepository : ICategoryNewsRepository
    {
        readonly DBContext context = new DBContext();

        public IQueryable<CategoryNews> CategoryNewsEntities
        {
            get { return this.context.CategoriesNews; }
        }
    }
}