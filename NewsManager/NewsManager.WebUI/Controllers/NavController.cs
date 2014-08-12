using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NewsManager.Domain.Abstract;
using NewsManager.Domain.DAL;

namespace NewsManager.WebUI.Controllers
{
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
              .Select(x => x.Category)
              .Distinct()
              .OrderBy(x => x)
              .ToList();
            return PartialView( categories);
        }
    }
}