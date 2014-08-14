using NewsManager.Domain.Abstract;

namespace NewsManager.WebUI.Controllers
{
    using System.Web.Mvc;

    using NewsManager.Domain.DAL;

    public class CategoriesController : Controller
    {
        private readonly INewsRepository repo;
        

        public CategoriesController()
        {
            repo = new NewsRepository();
        }

        // GET: Categories/List
        public ActionResult List()
        {
            
            return View();
        }
    }
}