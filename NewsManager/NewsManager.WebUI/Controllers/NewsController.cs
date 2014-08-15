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

namespace NewsManager.WebUI.Controllers
{
    public class NewsController : Controller
    {
        private readonly INewsRepository repo;
        private readonly ICategoryNewsRepository categoryRepo;
        public int PageSize = 5;

        public NewsController()
        {
            repo = new NewsRepository();
            categoryRepo = new CategoryNewsRepository();
        }

        #region Actions

        // GET: News
        public ActionResult Index(string searchString, string sortOrder, int? category, int page)
        {
            // gets news by categoryId
            IQueryable<News> query = GetEntities(category, page);

            query = ApplySorting(query, sortOrder);
            query = ApplyFilter(query, searchString);

            SetFilterParameters(sortOrder);


            var model = new NewsListModel
            {
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = GetNewsTotalCount(category)
                },
                CurrentCategory = category,
                Entities = query.ToList().Select(x => ConvertEntityToModel(x)).ToList()
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

            News news = repo.FindById(id.Value);
            if (news == null)
            {
                return HttpNotFound();
            }

            return View(ConvertEntityToModel(news));
        }

        // GET: News/Create
        public ActionResult Create()
        {
            return View("Edit", new NewsModel
            {
                CategoryID = null,
                Categories = GetCategories()
            });
        }

        // POST: News/Create
        // To protect from over posting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NewsModel news)
        {
            if (ModelState.IsValid)
            {
                repo.Add(ConvertModelToEntity(news));
                return RedirectToAction("Index");
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
            News news = repo.FindById(id.Value);
            if (news == null)
            {
                return HttpNotFound();
            }


            NewsModel model = ConvertEntityToModel(news);
            model.Categories = GetCategories();

            return View(model);
        }

        private IEnumerable<SelectListItem> GetCategories()
        {
            var categories = new List<SelectListItem>();
            categories.Add(new SelectListItem
            {
                Text = "-- None --",
                Value = null
            });

            categories.AddRange(categoryRepo.CategoryNewsEntities.Select(c => new SelectListItem
            {
                Value = c.CategoryNewsID.ToString(),
                Text = c.Name
            }).OrderBy(x => x.Text).ToList());

            return categories;
        }

        // POST: News/Edit/5
        // To protect from over posting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(NewsModel model)
        {
            if (ModelState.IsValid)
            {
                repo.Update(ConvertModelToEntity(model));
                return RedirectToAction("Index");
            }

            model.Categories = GetCategories();

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
            News news = repo.FindById(id.Value);
            if (news == null)
            {
                return HttpNotFound();
            }

            repo.Delete(id.Value);

            return new JsonResult
            {
                Data = new
                {
                    deleted = true
                }
            };
        }

        #endregion

        #region Business Logic

        private News ConvertModelToEntity(NewsModel model)
        {
            var news = new News();

            news.NewsID = model.NewsID;
            news.BodyNews = model.BodyNews;
            news.Title = model.Title;
            news.Status = model.Status;

            if (model.CategoryID != null)
            {
                news.Category = new CategoryNews
                {
                    CategoryNewsID = model.CategoryID.Value
                };
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

            if (news.Category != null)
            {
                model.CategoryID = news.Category.CategoryNewsID;
                model.Category = ConvertCategoryEntityToModel(news.Category);
            }

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

        private CategoryNewsModel ConvertCategoryEntityToModel(CategoryNews category)
        {
            return new CategoryNewsModel
            {
                CategoryNewsID = category.CategoryNewsID,
                Name = category.Name
            };
        }

        private int GetNewsTotalCount(int? categoryId)
        {
            return !categoryId.HasValue
                ? repo.NewsEntities.Count()
                : repo.NewsEntities.Include(x => x.Category)
                    .Count(e => e.Category != null && e.Category.CategoryNewsID == categoryId);
        }

        private void SetFilterParameters(string sortOrder)
        {
            // Prepare filter parameter for Title, CreatesDate
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Title" : "";
            ViewBag.DateSortParm = sortOrder == "CreateDate" ? "Date" : "CreateDate";
        }

        private IQueryable<News> ApplyFilter(IQueryable<News> query, string searchString)
        {
            if (!String.IsNullOrEmpty(searchString))
            {
                query = query.Where(s => s.Title.Contains(searchString));
            }

            return query;
        }

        private IQueryable<News> ApplySorting(IQueryable<News> query, string sortOrder)
        {
            switch (sortOrder)
            {
                case "Title":
                    query = query.OrderByDescending(s => s.Title);
                    break;
                case "CreateDate":
                    query = query.OrderBy(s => s.CreatedDate);
                    break;
                case "Date":
                    query = query.OrderByDescending(s => s.CreatedDate);
                    break;
                default:
                    query = query.OrderBy(s => s.Title);
                    break;
            }

            return query;
        }

        private IQueryable<News> GetEntities(int? category, int page)
        {
            return repo.NewsEntities
                .Include(x => x.Category)
                .Where(p => category == null
                            || (p.Category != null && p.Category.CategoryNewsID == category))
                .OrderBy(p => p.NewsID)
                .Skip((page - 1)*PageSize)
                .Take(PageSize);
        }

        #endregion
    }
}
