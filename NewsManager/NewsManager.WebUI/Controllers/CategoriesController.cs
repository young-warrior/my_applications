namespace NewsManager.WebUI.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;

    using NewsManager.Domain.DAL;
    using NewsManager.Domain.Entities;
    using NewsManager.WebUI.Helpers;
    using NewsManager.WebUI.Models;

    public class CategoriesController : Controller
    {
        private readonly ICategoryNewsRepository repo;

        public CategoriesController(ICategoryNewsRepository categoryNewsRepository)
        {
            this.repo = categoryNewsRepository;
        }

        // GET: Categories/List

        #region Actions

        // GET: News
        public ActionResult List(string sortBy = "Name", bool ascending = true, int page = 1, int pageSize = 7)
        {
            var model = new CategoriesNewListModel
                            {
                                SortAscending = ascending,
                                SortBy = sortBy,
                
                                // Paging-related properties
                                CurrentPageIndex = page,
                                PageSize = pageSize  
                            };


            // gets news by category
            IQueryable<CategoryNewsModel> query = this.repo.CategoryNewsEntities.Select(x=>new CategoryNewsModel
                                                                                               {
                                                                                                   CategoryNewsID = x.CategoryNewsID, 
                                                                                                   Name = x.Name
                                                                                               });
            query = this.ApplySorting(query, model.SortExpression);
            
            int totalCount = query.Count();
            query = this.ApplyPaging(query, page, pageSize);
            
            model.TotalRecordCount = totalCount;
            model.Entities = query.ToList();

            return View(model);
        }

        private IQueryable<CategoryNewsModel> ApplyPaging(IQueryable<CategoryNewsModel> query, int page, int pageSize)
        {
            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }

        private IQueryable<CategoryNewsModel> ApplySorting(IQueryable<CategoryNewsModel> query, String sortExpression)
        {
            return query.OrderBy(sortExpression);
        }

        #endregion

        #region Business Logic

        private CategoryNewsModel ConvertEntityToModel(CategoryNews category)
        {
            var model = new CategoryNewsModel { CategoryNewsID = category.CategoryNewsID, Name = category.Name };

            return model;
        }

        private CategoryNews ConvertCategoryModelToEntity(CategoryNewsModel category)
        {
            return new CategoryNews { CategoryNewsID = category.CategoryNewsID, Name = category.Name };
        }

        public ActionResult Create()
        {
            ViewBag.IsNew = true;
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
                this.repo.Create(this.ConvertCategoryModelToEntity(category));
                return this.RedirectToAction("List");
            }

            ViewBag.IsNew = true;
            return View("Edit", category);
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


            ViewBag.IsNew = false;
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
                this.repo.CreateOrUpdate(this.ConvertCategoryModelToEntity(model));
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
            if (this.repo.HasNews(id.Value))
            {
                return new JsonResult { Data = new { deleted = false } };
            }
            this.repo.Delete(id.Value);
            return new JsonResult { Data = new { deleted = true } };
        }

        // POST: Categories/DeleteWithNews/5
        [HttpPost]

        public ActionResult DeleteWithNews(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            this.repo.Delete(id.Value);
            return new JsonResult { Data = new
                                               {
                                                   deleted = true
                                               } };
        }
    }

    #endregion
}
