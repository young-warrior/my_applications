using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;
using NewsManager.Domain.Abstract;
using NewsManager.Domain.DAL;
using NewsManager.Domain.Entities;
using Ninject;

namespace NewsManager.WebUI.Infrastructure
{

    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private IKernel ninjectKernel;

        public NinjectControllerFactory()
        {
            // создание контейнера
            ninjectKernel = new StandardKernel();
            AddBindings();
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            // получение объекта контроллера из контейнера 
            // используя его тип
            return controllerType == null
                ? null
                : (IController) ninjectKernel.Get(controllerType);
        }

        private void AddBindings()
        {
            // конфигурирование контейнера
//            Mock<INewsRepository> mock = new Mock<INewsRepository>();
//            mock.Setup(m => m.NewsEntities).Returns(new List<News> {
//          new News { Title = "News1", CreatedDate = 00.00, BodyNews = "first news" },
//          new News { Title = "News2", CreatedDate = 05.00, BodyNews = "second news" },
//          new News { Title = "News3", CreatedDate = 10.00, BodyNews = "therd news" }
//        }.AsQueryable());

            
//            ninjectKernel.Bind<INewsRepository>().ToConstant(mock.Object);
            ninjectKernel.Bind<INewsRepository>().To<NewsRepository>();
        }
        
    }
}