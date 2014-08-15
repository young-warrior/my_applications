using System;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
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

        public CategoriesController()
        {
            repo = new CategoryNewsRepository();
        }

        // GET: Categories/List

        #region Actions

        // GET: News
        public ActionResult List( int page = 1)
        {
            // gets news by category
            IQueryable<CategoryNews> query = GetEntities(page);

            var model = new CategoriesNewListModel
            {
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = GetNewsTotalCount()
                },
                Entities = query.ToList().Select(x => ConvertEntityToModel(x)).ToList()
            };


            return View(model);
        }

        #endregion

        #region Business Logic

        //private News ConvertModelToEntity(CategoryNewsModel model)
        //{
        //    var news = new CategoryNewsModel();

           
        //        news.CategoryNewsID = ConvertCategoryModelToEntity(model.Category);
           

        //    return news;
        //}

        private CategoryNewsModel ConvertEntityToModel(CategoryNews category)
        {
            var model = new CategoryNewsModel();
            model.CategoryNewsID = category.CategoryNewsID;
            model.Name = category.Name;

            return model;
        }


        private CategoryNews ConvertCategoryModelToEntity(CategoriesModel category)
        {
            return new CategoryNews
            {
                CategoryNewsID = category.Category.CategoryNewsID,
                Name = category.Category.Name
                
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
//
        private int GetNewsTotalCount()
        {
            return repo.CategoryNewsEntities.Count();
        }


        private IQueryable<CategoryNews> GetEntities(int page)
        {
            return repo.CategoryNewsEntities
                .OrderBy(p => p.CategoryNewsID)
                .Skip((page - 1)*PageSize)
                .Take(PageSize);
        }

        
        public ActionResult Create()
        {
            return View("Edit", new CategoriesModel());
        }

        // POST: Category/Create
        // To protect from over posting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CategoriesModel category)
        {
            if (ModelState.IsValid)
            {
                repo.Add(ConvertCategoryModelToEntity(category));
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

            return View(ConvertCategoryEntityToModel(category));
        }

        // POST: Catelog/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CategoriesModel model)
        {
            if (ModelState.IsValid)
            {
                repo.Update(ConvertCategoryModelToEntity(model));
                return RedirectToAction("List");
            }
            return View(model);
        }

        // POST: Catalog/Delete/5
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
    }
}


    