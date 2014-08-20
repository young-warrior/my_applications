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
    using NewsManager.WebUI.Helpers;

    public class NewsController : Controller
    {
        private readonly INewsRepository repo;

        private readonly ICategoryNewsRepository categoryRepo;

        public NewsController(INewsRepository newsRepository, ICategoryNewsRepository categoryNewsRepository)
        {
            repo = newsRepository;
            categoryRepo = categoryNewsRepository;
        }

        #region Actions

        // GET: News
        public ActionResult Index(string searchBy, string searchString, 
            int? category, string sortBy = "Title", bool ascending = true, int page = 1, int pageSize = 7)
        {
            // creates view model
            var model = new NewsListModel
            {
                SortAscending = ascending,
                SortBy = sortBy,
                CurrentCategory = category,
                
                // Sorting-related properties
                SearchString = searchString,
                SearchBy = searchBy,

                // Paging-related properties
                CurrentPageIndex = page,
                PageSize = pageSize

            };

            // gets news by categoryId
            IQueryable<NewsModel> query = GetEntities(category)
                .Select( news=> 
                    new NewsModel
                        {
                            NewsID = news.NewsID,
                            BodyNews = news.BodyNews,
                //            model.TitleShort = GetShortTitle(news.Title);
                            Title = news.Title,
                            Status = news.Status,
                            CreatedDate = news.CreatedDate,
                            CategoryName = news.Category.Name

                        });

            
            query = ApplySorting(query, model.SortExpression);
            query = ApplyFilter(query, searchString, searchBy);
            

            if (!String.IsNullOrEmpty(searchString))
            {
                query = ApplyFilter(query, searchString, searchBy);
            }

            int totalCount = Queryable.Count(query);
            query = ApplyPaging(query, page, pageSize);

            model.TotalRecordCount = totalCount;
            model.Entities = query.ToList();

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
            ViewBag.Categories = GetCategories();
            return View("Edit", new NewsModel {CategoryID = null });
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
                repo.Create(ConvertModelToEntity(news));
                return RedirectToAction("Index");
            }

            return View("Edit", news);
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
            ViewBag.Categories = GetCategories();

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

            ViewBag.Categories = GetCategories();

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
            var model = new NewsModel
                            {
                                NewsID = news.NewsID,
                                BodyNews = news.BodyNews,
                                Title = news.Title,
                                Status = news.Status,
                                CreatedDate = news.CreatedDate
                            };

            if (news.Category != null)
            {
                model.CategoryID = news.Category.CategoryNewsID;
                model.CategoryName = news.Category.Name;
            }

            return model;
        }
        //Receiving from the base short title
//        private string GetShortTitle(string title)
//        {
//            string[] b = title.Split(new[] {' '});
//            var words = new List<string>();
//            int count;
//            foreach (string word in b)
//            {
//                if (!string.IsNullOrEmpty(word))
//                {
//                    words.Add(word);
//                    if (words.Count() == 5)
//                    {
//                        words.Add("...");
//                        break;
//                    }
//                }
//            }
//            return String.Join(" ", words);
//        }

        private IQueryable<NewsModel> ApplyFilter(IQueryable<NewsModel> query, string searchString, string searchBy)
        {
            if (!String.IsNullOrEmpty(searchString))
            {
                if (searchBy == "Categories")
                {
                    query = query.Where(s => s.CategoryName.Contains(searchString));
                }
                else
                {
                    query = query.Where(s => s.Title.Contains(searchString));
                }
            }

            return query;
        }

        private IQueryable<NewsModel> ApplyPaging(IQueryable<NewsModel> query, int page, int pageSize)
        {
            return Queryable.Take(Queryable.Skip(query, (page - 1) * pageSize), pageSize);

        }

//        private void SetFilterParameters(string sortOrder)
//        {
//            // Prepare filter parameter for Title, CreatesDate
//            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Title" : "";
//            ViewBag.DateSortParm = sortOrder == "CreateDate" ? "CreateDateDown" : "CreateDate";
//            ViewBag.CategoryParm = sortOrder == "Category" ? "CategoryDown" : "Category";
//            ViewBag.StatusParm = sortOrder == "Status" ? "StatusDown" : "Status";
//        }

        private IQueryable<NewsModel> ApplySorting(IQueryable<NewsModel> query, String sortExpression)
        {
//            switch (sortOrder)
//            {
//                case "Title":
//                    query = query.OrderByDescending(s => s.Title);
//                    break;
//                case "CreateDate":
//                    query = query.OrderBy(s => s.CreatedDate);
//                    break;
//                case "CreateDateDown":
//                    query = query.OrderByDescending(s => s.CreatedDate);
//                    break;
//                case "Category":
//                    query = query.OrderBy(s => s.Category.Name);
//                    break;
//                case "CategoryDown":
//                    query = query.OrderByDescending(s => s.Category.Name);
//                    break;
//                case "Status":
//                    query = query.OrderByDescending(s => s.Status);
//                    break;
//                case "StatusDown":
//                    query = query.OrderBy(s => s.Status);
//                    break;
//                default:
//                    query = query.OrderBy(s => s.Title);
//                    break;
//            }

            return query.OrderBy(sortExpression);
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
