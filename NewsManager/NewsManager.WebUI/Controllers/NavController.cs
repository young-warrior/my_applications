using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NewsManager.Domain.Abstract;
using NewsManager.Domain.DAL;

namespace NewsManager.WebUI.Controllers
{
    using System.Data.Entity;

    public class NavController : Controller
    {
        // GET: Nav
        private INewsRepository repository;

        public NavController(INewsRepository repo)
        {
             repository = repo;
        }

        public PartialViewResult Menu()
        {
            IEnumerable<string> categories = repository.NewsEntities
              .Include(x => x.Category)
              .Where(x => x.Category != null)
              .Select(x => x.Category.Name)
              .Distinct()
              .OrderBy(x => x)
              .ToList();
            return PartialView( categories);
        }
    }
}