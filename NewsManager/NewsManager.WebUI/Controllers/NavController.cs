namespace NewsManager.WebUI.Controllers
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Mvc;

    using NewsManager.Domain.Abstract;
    using NewsManager.WebUI.Models;

    public class NavController : Controller
    {
        private readonly INewsRepository repository;

        public NavController(INewsRepository repo)
        {
            this.repository = repo;
        }

        // GET: Nav
        public PartialViewResult Menu(string category = null)
        {
            ViewBag.SelectedCategory = category;
            IEnumerable<NavigationModel> categories =
                this.repository.NewsEntities.Include(x => x.Category)
                    .Where(x => x.Category != null)
                    .Select(x => new NavigationModel()
                                     {
                                         Title = x.Category.Name,
                                         CategoryId = x.Category.CategoryNewsID
                                     } )
                    .Distinct()
                    .OrderBy(x => x.Title)
                    .ToList();
            return PartialView(categories);
        }
    }
}