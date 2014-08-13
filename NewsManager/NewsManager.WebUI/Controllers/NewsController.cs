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
        private readonly INewsRepository _repo;
        public int PageSize = 5;

        public NewsController()
        {
            _repo = new NewsRepository();
        }

        // GET: News
        public ActionResult Index(string searchString, string sortOrder, string category, int page)
        {
            // gets news by category
            IQueryable<News> entities = _repo.NewsEntities
                .Include(x=>x.Category)
                .Where(p => string.IsNullOrEmpty(category) 
                    || (p.Category != null && p.Category.Name == category))
                .OrderBy(p => p.NewsID)
                .Skip((page - 1)*PageSize)
                .Take(PageSize);
            
            var model = new NewsModel
            {
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = category == null
                        ? _repo.NewsEntities.Count()
                        : _repo.NewsEntities.Include(x => x.Category).Count(e => e.Category != null 
                            && e.Category.Name == category)
                },
                CurrentCategory = category
            };
            //            filter order by Title, CreatesDate
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Title" : "";
            ViewBag.DateSortParm = sortOrder == "CreateDate" ? "Date" : "CreateDate";

            IQueryable<News> query = entities;
            
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

            if (!String.IsNullOrEmpty(searchString))
            {
                query = query.Where(s => s.Title.Contains(searchString));
            }

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

            News news = _repo.FindById(id.Value);
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
                _repo.Add(news);
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
            News news = _repo.FindById(id.Value);
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
//        public ActionResult Edit([Bind(Include = "NewsID,Title,BodyNews,Status,Category")] News news)
        public ActionResult Edit(News news)
        {
            if (ModelState.IsValid)
            {
                _repo.Update(news);
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
            News news = _repo.FindById(id.Value);
            if (news == null)
            {
                return HttpNotFound();
            }

            _repo.Delete(id.Value);
            
            return new JsonResult()
                       {
                           Data = new {
                               deleted = true
                           }
                       };
        }
    }
}
