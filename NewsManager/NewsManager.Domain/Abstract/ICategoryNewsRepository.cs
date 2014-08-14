using System;
using System.Linq;
using NewsManager.Domain.Entities;

namespace NewsManager.Domain.DAL
{
    public interface ICategoryNewsRepository
    {
        IQueryable<CategoryNews> CategoryNewsEntities { get; }
    }
}