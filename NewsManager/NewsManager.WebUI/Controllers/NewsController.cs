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
    public class NewsController : Controller
    {
        private readonly INewsRepository _repo;
        public int PageSize = 5;

        public NewsController()
        {
            _repo = new NewsRepository();
        }

        // GET: News
        public ActionResult Index(string id, string sortOrder, string category, int page)
        {
            
            IQueryable<News> entities = _repo.NewsEntities
                .Where(p => string.IsNullOrEmpty(category) || p.Category == category)
                .OrderBy(p => p.NewsID)
                .Skip((page - 1)*PageSize)
                .Take(PageSize);
            //сортировка по категории
            var model = new NewsModel
            {
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    //TotalItems = _repo.NewsEntities.Count()
                    TotalItems = category == null
                        ? _repo.NewsEntities.Count()
                        : _repo.NewsEntities.Count(e => e.Category == category)
                },
                CurrentCategory = category
            };
            //фильтрация в колонках Title, CreatesDate
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Title" : "";
            ViewBag.DateSortParm = sortOrder == "CreateDate" ? "Date" : "CreateDate";

            IQueryable<News> ns = entities;

            switch (sortOrder)
            {
                case "Title":
                    ns = ns.OrderByDescending(s => s.Title);
                    break;
                case "CreateDate":
                    ns = ns.OrderBy(s => s.CreatedDate);
                    break;
                case "Date":
                    ns = ns.OrderByDescending(s => s.CreatedDate);
                    break;
                default:
                    ns = ns.OrderBy(s => s.Title);
                    break;
            }

            model.Entities = ns.ToList();
            
            //Filter
            string searchString = id;
            var filter = from m in _repo.NewsEntities
                         select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                filter = filter.Where(s => s.Title.Contains(searchString));
            } 

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
            ;
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


        // GET: News/Delete/5
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
            return View(news);
        }

        // POST: News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _repo.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
