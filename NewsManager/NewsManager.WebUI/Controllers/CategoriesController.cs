using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using NewsManager.Domain.DAL;
using NewsManager.Domain.Entities;
using NewsManager.WebUI.Models;

namespace NewsManager.WebUI.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryNewsRepository repo;

        public int PageSize = 10;

        public CategoriesController(ICategoryNewsRepository categoryNewsRepository)
        {
            repo = categoryNewsRepository;
        }

        // GET: Categories/List

        #region Actions

        // GET: News
        public ActionResult List(string sortOrder, int page = 1)
        {
            // gets news by category
            IQueryable<CategoryNews> query = repo.CategoryNewsEntities;

            query = ApplySorting(query, sortOrder);
            SetFilterParameters(sortOrder);

            if (!String.IsNullOrEmpty(sortOrder))
            {
                query = ApplySorting(query, sortOrder);
            }

            int totalCount = query.Count();
            query = ApplyPaging(query, page);
            var model = new CategoriesNewListModel
            {
                PagingInfo =
                    new PagingInfo
                    {
                        CurrentPage = page,
                        ItemsPerPage = PageSize,
                        TotalItems = totalCount
                    },
                SortOrder = sortOrder,
                Entities =
                    query.ToList()
                        .Select(ConvertEntityToModel)
                        .ToList()
            };

            return View(model);
        }

        private IQueryable<CategoryNews> ApplyPaging(IQueryable<CategoryNews> query, int page)
        {
            return query.Skip((page - 1)*PageSize).Take(PageSize);
        }

        private void SetFilterParameters(string sortOrder)
        {
            // Prepare filter parameter for Title, CreatesDate
            ViewBag.CategoryParm = String.IsNullOrEmpty(sortOrder) ? "Category" : "";
        }

        private IQueryable<CategoryNews> ApplySorting(IQueryable<CategoryNews> query, string sortOrder)
        {
            switch (sortOrder)
            {
                case "Category":
                    query = query.OrderByDescending(s => s.Name);
                    break;
                default:
                    query = query.OrderBy(s => s.Name);
                    break;
            }

            return query;
        }

        #endregion

        #region Business Logic

        private CategoryNewsModel ConvertEntityToModel(CategoryNews category)
        {
            var model = new CategoryNewsModel {CategoryNewsID = category.CategoryNewsID, Name = category.Name};

            return model;
        }

        private CategoryNews ConvertCategoryModelToEntity(CategoryNewsModel category)
        {
            return new CategoryNews {CategoryNewsID = category.CategoryNewsID, Name = category.Name};
        }

        public ActionResult Create()
        {
            return View("Edit", new CategoryNewsModel());
        }

        // POST: Category/Create
        // To protect from over posting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CategoryNewsModel category)
        {
            if (ModelState.IsValid)
            {
                repo.Create(ConvertCategoryModelToEntity(category));
                return RedirectToAction("List");
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
            CategoryNews category = repo.FindById(id.Value);
            if (category == null)
            {
                return HttpNotFound();
            }

            return View(ConvertEntityToModel(category));
        }

        // POST: Categories/Edit/5
        // To protect from over posting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CategoryNewsModel model)
        {
            if (ModelState.IsValid)
            {
                repo.CreateOrUpdate(ConvertCategoryModelToEntity(model));
                return RedirectToAction("List");
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
            CategoryNews news = repo.FindById(id.Value);
          
            if (news == null)
            {
                return HttpNotFound();
            }
            if (news.IsKey)
            {
                return new JsonResult {Data = new {deleted = false}};
            }
            repo.Delete(id.Value);
            return new JsonResult {Data = new {deleted = true}};
        }

        public ActionResult DeleteNews(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoryNews news = repo.FindById(id.Value);
            
            if (news == null)
            {
                return HttpNotFound();
            }

            repo.Delete(id.Value);
            return new JsonResult {Data = new {deleted = true}};
        }
    }

    #endregion
}
