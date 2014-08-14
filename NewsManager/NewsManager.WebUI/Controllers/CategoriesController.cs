namespace NewsManager.WebUI.Controllers
{
    using System.Web.Mvc;

    using NewsManager.Domain.DAL;

    public class CategoriesController : Controller
    {
        private readonly ICategoryNewsRepository repo;

        public CategoriesController(ICategoryNewsRepository repo)
        {
            this.repo = repo;
        }

        // GET: Categories/List
        public ActionResult List(int page = 1)
        {
            return this.View();
        }
    }
}