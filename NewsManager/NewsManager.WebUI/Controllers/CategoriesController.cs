using System;

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

        private NewsRepository newsRepo;

        public int PageSize = 10;
        

        public CategoriesController()
        {
            this.repo = new CategoryNewsRepository();
            this.newsRepo = new NewsRepository();
        }

        // GET: Categories/List

        #region Actions

        // GET: News
        public ActionResult List(string sortOrder,int page = 1)
        {
            // gets news by category
            IQueryable<CategoryNews> query = this.GetEntities();

            query = ApplySorting(query, sortOrder);
            SetFilterParameters(sortOrder);
            int count = GetNewsTotalCount(query);

            if (!String.IsNullOrEmpty(sortOrder))
            {
                query = ApplySorting(query, sortOrder);
            }
            query = ApplyPaging(query, page);
            var model = new CategoriesNewListModel
                            {
                                PagingInfo =
                                    new PagingInfo
                                        {
                                            CurrentPage = page,
                                            ItemsPerPage = this.PageSize,
                                            TotalItems = count
                                        },
                                SortOrder = sortOrder,
                                Entities =
                                    query.ToList().Select(x => this.ConvertEntityToModel(x)).ToList()
                            };

            return View(model);
        }
        private IQueryable<CategoryNews> ApplyPaging(IQueryable<CategoryNews> query, int page)
        {
            return query.Skip((page - 1) * PageSize)
               .Take(PageSize);

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

        private int GetNewsTotalCount(IQueryable<CategoryNews> query)
        {
            return this.repo.CategoryNewsEntities.Count();
        }

        private IQueryable<CategoryNews> GetEntities()
        {
            return this.repo.CategoryNewsEntities;


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

            this.repo.Delete(id.Value, newsRepo.FindByCategoryId(id.Value));

            return new JsonResult { Data = new { deleted = true } };
        }

        #endregion
    }
}


    