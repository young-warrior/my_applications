namespace NewsManager.WebUI.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;

    using NewsManager.Domain.Abstract;
    using NewsManager.Domain.DAL;
    using NewsManager.Domain.Entities;
    using NewsManager.WebUI.Models;

    public class NewsController : Controller
    {
        private readonly INewsRepository repo;

        private readonly ICategoryNewsRepository categoryRepo;

        public int PageSize = 4;

        public NewsController(INewsRepository newsRepository, ICategoryNewsRepository categoryNewsRepository)
        {
            this.repo = newsRepository;
            this.categoryRepo = categoryNewsRepository;
        }

        #region Actions

        // GET: News
        public ActionResult Index(string searchString, string carrentFilter, string sortOrder, int? category, int page)
        {
            // gets news by categoryId
            IQueryable<News> query = this.GetEntities(category);

            this.ViewBag.carrentFilter = searchString;

            query = this.ApplySorting(query, sortOrder);
            query = this.ApplyFilter(query, searchString);
            this.SetFilterParameters(sortOrder);

            if (!String.IsNullOrEmpty(searchString))
            {
                query = this.ApplyFilter(query, carrentFilter);
            }

            int totalCount = query.Count();
            query = this.ApplyPaging(query, page);

            var model = new NewsListModel
                            {
                                PagingInfo =
                                    new PagingInfo
                                        {
                                            CurrentPage = page,
                                            ItemsPerPage = this.PageSize,
                                            TotalItems = totalCount,
                                        },
                                CurrentCategory = category,
                                SortOrder = sortOrder,
                                SearchString = searchString,
                                Entities = query.ToList().Select(this.ConvertEntityToModel).ToList()
                            };

            return View(model);
        }

        //GET: News/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            News news = this.repo.FindById(id.Value);
            if (news == null)
            {
                return this.HttpNotFound();
            }

            return this.View(this.ConvertEntityToModel(news));
        }

        // GET: News/Create
        public ActionResult Create()
        {
            return this.View("Edit", new NewsModel { CategoryID = null, Categories = this.GetCategories() });
        }

        // POST: News/Create
        // To protect from over posting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NewsModel news)
        {
            if (this.ModelState.IsValid)
            {
                this.repo.Add(this.ConvertModelToEntity(news));
                return this.RedirectToAction("Index");
            }

            return View(news);
        }

        // GET: News/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = this.repo.FindById(id.Value);
            if (news == null)
            {
                return this.HttpNotFound();
            }

            NewsModel model = this.ConvertEntityToModel(news);
            model.Categories = this.GetCategories();

            return View(model);
        }

        private IEnumerable<SelectListItem> GetCategories()
        {
            var categories = new List<SelectListItem>();
            categories.Add(new SelectListItem { Text = "-- None --", Value = null });

            categories.AddRange(
                this.categoryRepo.CategoryNewsEntities.Select(
                    c => new SelectListItem { Value = c.CategoryNewsID.ToString(), Text = c.Name })
                    .OrderBy(x => x.Text)
                    .ToList());

            return categories;
        }

        // POST: News/Edit/5
        // To protect from over posting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(NewsModel model)
        {
            if (this.ModelState.IsValid)
            {
                this.repo.Update(this.ConvertModelToEntity(model));
                return this.RedirectToAction("Index");
            }

            model.Categories = this.GetCategories();

            return View(model);
        }

        // POST: News/Delete/5
        [HttpPost]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = this.repo.FindById(id.Value);
            if (news == null)
            {
                return this.HttpNotFound();
            }

            this.repo.Delete(id.Value);

            return new JsonResult { Data = new { deleted = true } };
        }

        #endregion

        #region Business Logic

        private News ConvertModelToEntity(NewsModel model)
        {
            var news = new News
                           {
                               NewsID = model.NewsID,
                               BodyNews = model.BodyNews,
                               Title = model.Title,
                               Status = model.Status
                           };

            if (model.CategoryID != null)
            {
                news.Category = new CategoryNews { CategoryNewsID = model.CategoryID.Value };
            }

            return news;
        }

        private NewsModel ConvertEntityToModel(News news)
        {
            var model = new NewsModel();

            model.NewsID = news.NewsID;
            model.BodyNews = news.BodyNews;
            model.Title = news.Title;
            model.Status = news.Status;
            model.CreatedDate = news.CreatedDate;

            if (news.Category != null)
            {
                model.CategoryID = news.Category.CategoryNewsID;
                model.Category = this.ConvertCategoryEntityToModel(news.Category);
            }

            return model;
        }

        private CategoryNews ConvertCategoryModelToEntity(CategoryNewsModel category)
        {
            return new CategoryNews { CategoryNewsID = category.CategoryNewsID, Name = category.Name };
        }

        private CategoryNewsModel ConvertCategoryEntityToModel(CategoryNews category)
        {
            return new CategoryNewsModel { CategoryNewsID = category.CategoryNewsID, Name = category.Name };
        }

        
        private IQueryable<News> ApplyFilter(IQueryable<News> query, string searchString)
        {
            if (!String.IsNullOrEmpty(searchString))
            {
                query = query.Where(s => s.Title.Contains(searchString));
            }

            return query;
        }

        private IQueryable<News> ApplyPaging(IQueryable<News> query, int page)
        {
            return query.Skip((page - 1) * this.PageSize).Take(this.PageSize);
        }

        private void SetFilterParameters(string sortOrder)
        {
            // Prepare filter parameter for Title, CreatesDate
            this.ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Title" : "";
            this.ViewBag.DateSortParm = sortOrder == "CreateDate" ? "Date" : "CreateDate";
            this.ViewBag.CategoryParm = String.IsNullOrEmpty(sortOrder) ? "Category" : "";
        }

        private IQueryable<News> ApplySorting(IQueryable<News> query, string sortOrder)
        {
            switch (sortOrder)
            {
                case "Title":
                    query = query.OrderByDescending(s => s.Title);
                    break;
                case "Date":
                    query = query.OrderBy(s => s.CreatedDate);
                    break;
                case "CreateDate":
                    query = query.OrderByDescending(s => s.CreatedDate);
                    break;
                case "Category":
                    query = query.OrderByDescending(s => s.Category.Name);
                    break;
                default:
                    query = query.OrderBy(s => s.Title);
                    break;
            }

            return query;
        }

        private IQueryable<News> GetEntities(int? category)
        {
            return
                this.repo.NewsEntities.Include(x => x.Category)
                    .Where(p => category == null || (p.Category != null && p.Category.CategoryNewsID == category))
                    .OrderBy(p => p.NewsID);
        }

        #endregion
    }
}
