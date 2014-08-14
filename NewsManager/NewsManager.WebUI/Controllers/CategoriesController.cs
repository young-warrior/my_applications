using System;
using System.Linq;
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
        public int PageSize = 5;

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

        #endregion
    }
}


    