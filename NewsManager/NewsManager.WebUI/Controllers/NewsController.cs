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

        public int PageSize = 7;

        public NewsController(INewsRepository newsRepository, ICategoryNewsRepository categoryNewsRepository)
        {
            repo = newsRepository;
            categoryRepo = categoryNewsRepository;
        }

        #region Actions

        // GET: News
        public ActionResult Index(string searchBy, string searchString, string carrentFilter, string sortOrder,
            int? category, int page)
        {
            // gets news by categoryId
            IQueryable<News> query = GetEntities(category);

            ViewBag.carrentFilter = searchString;

            query = ApplySorting(query, sortOrder);

            query = ApplyFilter(query, searchString, searchBy);

            SetFilterParameters(sortOrder);

            if (!String.IsNullOrEmpty(searchString))
            {
                query = ApplyFilter(query, carrentFilter, searchBy);
            }

            int totalCount = query.Count();
            query = ApplyPaging(query, page);

            var model = new NewsListModel
            {
                PagingInfo =
                    new PagingInfo
                    {
                        CurrentPage = page,
                        ItemsPerPage = PageSize,
                        TotalItems = totalCount,
                    },
                CurrentCategory = category,
                SortOrder = sortOrder,
                SearchString = searchString,
                SearchBy = searchBy,
                Entities = query.ToList().Select(ConvertEntityToModel).ToList()
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
            return View("Edit", new NewsModel {CategoryID = null, Categories = GetCategories()});
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
            categories.Add(new SelectListItem {Text = "-- None --", Value = null});

            categories.AddRange(
                categoryRepo.CategoryNewsEntities.Select(
                    c => new SelectListItem {Value = c.CategoryNewsID.ToString(), Text = c.Name})
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

            return new JsonResult {Data = new {deleted = true}};
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
                news.Category = new CategoryNews {CategoryNewsID = model.CategoryID.Value};
            }

            return news;
        }

        private NewsModel ConvertEntityToModel(News news)
        {
            var model = new NewsModel();

            model.NewsID = news.NewsID;
            model.BodyNews = news.BodyNews;
            model.TitleShort = GetShortTitle(news.Title);
            model.Title = news.Title;
            model.Status = news.Status;
            model.CreatedDate = news.CreatedDate;

            if (news.Category != null)
            {
                model.CategoryID = news.Category.CategoryNewsID;
                model.Category = ConvertCategoryEntityToModel(news.Category);
            }

            return model;
        }

        private string GetShortTitle(string title)
        {
            string[] b = title.Split(new[] {' '});
            var words = new List<string>();
            int count;
            foreach (string word in b)
            {
                if (!string.IsNullOrEmpty(word))
                {
                    words.Add(word);
                    if (words.Count() == 5)
                    {
                        words.Add("...");
                        break;
                    }
                }
            }
            return String.Join(" ", words);
        }

        private CategoryNews ConvertCategoryModelToEntity(CategoryNewsModel category)
        {
            return new CategoryNews {CategoryNewsID = category.CategoryNewsID, Name = category.Name};
        }

        private CategoryNewsModel ConvertCategoryEntityToModel(CategoryNews category)
        {
            return new CategoryNewsModel {CategoryNewsID = category.CategoryNewsID, Name = category.Name};
        }


        private IQueryable<News> ApplyFilter(IQueryable<News> query, string searchString, string searchBy)
        {
            if (!String.IsNullOrEmpty(searchString))
            {
                if (searchBy == "Categories")
                {
                    query = query.Where(s => s.Category.Name.Contains(searchString));
                }
                else
                {
                    query = query.Where(s => s.Title.Contains(searchString));
                }
            }

            return query;
        }

        private IQueryable<News> ApplyPaging(IQueryable<News> query, int page)
        {
            return query.Skip((page - 1)*PageSize).Take(PageSize);
        }

        private void SetFilterParameters(string sortOrder)
        {
            // Prepare filter parameter for Title, CreatesDate
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Title" : "";
            ViewBag.DateSortParm = sortOrder == "CreateDate" ? "Date" : "CreateDate";
            ViewBag.CategoryParm = String.IsNullOrEmpty(sortOrder) ? "Category" : "";
            ViewBag.StatusParm = sortOrder == "Status" ? "StatusDown" : "Status";
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
                case "Status":
                    query = query.OrderByDescending(s => s.Status);
                    break;
                case "StatusDown":
                    query = query.OrderBy(s => s.Status);
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
                repo.NewsEntities.Include(x => x.Category)
                    .Where(p => category == null || (p.Category != null && p.Category.CategoryNewsID == category))
                    .OrderBy(p => p.NewsID);
        }

        #endregion
    }
}
