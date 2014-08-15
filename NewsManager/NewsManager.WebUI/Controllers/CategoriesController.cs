namespace NewsManager.WebUI.Controllers
{
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;

    using NewsManager.Domain.DAL;
    using NewsManager.Domain.Entities;
    using NewsManager.WebUI.Models;

    public class CategoriesController : Controller
    {
        private readonly ICategoryNewsRepository repo;

        public int PageSize = 10;

        public CategoriesController()
        {
            this.repo = new CategoryNewsRepository();
        }

        // GET: Categories/List

        #region Actions

        // GET: News
        public ActionResult List(int page = 1)
        {
            // gets news by category
            IQueryable<CategoryNews> query = this.GetEntities(page);

            var model = new CategoriesNewListModel
                            {
                                PagingInfo =
                                    new PagingInfo
                                        {
                                            CurrentPage = page,
                                            ItemsPerPage = this.PageSize,
                                            TotalItems = this.GetNewsTotalCount()
                                        },
                                Entities =
                                    query.ToList()
                                    .Select(x => this.ConvertEntityToModel(x))
                                    .ToList()
                            };

            return View(model);
        }

        #endregion

        #region Business Logic

        private CategoryNewsModel ConvertEntityToModel(CategoryNews category)
        {
            var model = new CategoryNewsModel
                            {
                                CategoryNewsID = category.CategoryNewsID, 
                                Name = category.Name
                            };

            return model;
        }

        private CategoryNews ConvertCategoryModelToEntity(CategoryNewsModel category)
        {
            return new CategoryNews
                       {
                           CategoryNewsID = category.CategoryNewsID, 
                           Name = category.Name
                       };
        }

        private int GetNewsTotalCount()
        {
            return this.repo.CategoryNewsEntities.Count();
        }

        private IQueryable<CategoryNews> GetEntities(int page)
        {
            return
                this.repo.CategoryNewsEntities.OrderBy(p => p.Name)

                    .Skip((page - 1) * this.PageSize)
                    .Take(this.PageSize);
        }

        public ActionResult Create()
        {
            return this.View("Edit", new CategoryNewsModel());
        }

        // POST: Category/Create
        // To protect from over posting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CategoryNewsModel category)
        {
            if (this.ModelState.IsValid)
            {
                this.repo.Add(this.ConvertCategoryModelToEntity(category));
                return this.RedirectToAction("List");
            }

            return View(category);
        }

        // GET: News/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoryNews category = this.repo.FindById(id.Value);
            if (category == null)
            {
                return this.HttpNotFound();
            }

            return this.View(this.ConvertEntityToModel(category));
        }

        // POST: Categories/Edit/5
        // To protect from over posting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CategoryNewsModel model)
        {
            if (this.ModelState.IsValid)
            {
                this.repo.AddOrUpdate(this.ConvertCategoryModelToEntity(model));
                return this.RedirectToAction("List");
            }
            return View(model);
        }

        // POST: Categories/Delete/5
        [HttpPost]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoryNews news = this.repo.FindById(id.Value);
            if (news == null)
            {
                return this.HttpNotFound();
            }

            this.repo.Delete(id.Value);

            return new JsonResult { Data = new { deleted = true } };
        }

        #endregion
    }
}


    