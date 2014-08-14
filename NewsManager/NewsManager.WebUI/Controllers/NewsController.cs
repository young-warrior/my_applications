using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using NewsManager.Domain.Abstract;
using NewsManager.Domain.DAL;
using NewsManager.Domain.Entities;
using NewsManager.WebUI.Models;

namespace NewsManager.WebUI.Controllers
{
    using System.Data.Entity;

    public class NewsController : Controller
    {
        private readonly INewsRepository repo;
        public int PageSize = 5;

        public NewsController()
        {
            this.repo = new NewsRepository();
        }

        #region Actions

        // GET: News
        public ActionResult Index(string searchString, string sortOrder, string category, int page)
        {
            // gets news by category
            IQueryable<News> query = GetEntities(category, page);
            
            query = this.ApplySorting(query, sortOrder);
            query = this.ApplyFilter(query, searchString);

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
                Entities = query.ToList()
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
                return HttpNotFound();
            }
            return View(news);
        }

        // GET: News/Create
        public ActionResult Create()
        {
            return View("Edit", new News());
        }

        // POST: News/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(News news)
        {
            if (ModelState.IsValid)
            {
                this.repo.Add(news);
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
            News news = this.repo.FindById(id.Value);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // POST: News/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(News news)
        {
            if (ModelState.IsValid)
            {
                this.repo.Update(news);
                return RedirectToAction("Index");
            }
            return View(news);
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
                return HttpNotFound();
            }

            this.repo.Delete(id.Value);
            
            return new JsonResult()
                       {
                           Data = new {
                               deleted = true
                           }
                       };
        }

        #endregion 

        #region Business Logic

        private int GetNewsTotalCount(string category)
        {
            return category == null
                ? this.repo.NewsEntities.Count()
                : this.repo.NewsEntities.Include(x => x.Category)
                    .Count(e => e.Category != null && e.Category.Name == category);
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

        private IQueryable<News> GetEntities(String category, int page)
        {
            return this.repo.NewsEntities
               .Include(x => x.Category)
               .Where(p => string.IsNullOrEmpty(category)
                   || (p.Category != null && p.Category.Name == category))
               .OrderBy(p => p.NewsID)
               .Skip((page - 1) * PageSize)
               .Take(PageSize);
        }

        #endregion
    }
}
