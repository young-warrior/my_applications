namespace NewsManager.WebUI.Controllers
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Mvc;

    using NewsManager.Domain.Abstract;

    public class NavController : Controller
    {
        private readonly INewsRepository repository;

        public NavController(INewsRepository repo)
        {
            this.repository = repo;
        }

        // GET: Nav
        public PartialViewResult Menu()
        {
            IEnumerable<string> categories =
                this.repository.NewsEntities.Include(x => x.Category)
                    .Where(x => x.Category != null)
                    .Select(x => x.Category.Name)
                    .Distinct()
                    .OrderBy(x => x)
                    .ToList();
            return PartialView(categories);
        }
    }
}