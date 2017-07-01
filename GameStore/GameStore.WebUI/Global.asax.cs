using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using GameStore.WebUI.Controllers;
using GameStore.WebUI.Infrastructure;
using GameStore.WebUI.Infrastructure.Binders;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace GameStore.WebUI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

			ModelBinders.Binders.Add(typeof(Cart), new CartModelBinder());

			var container = new UnityContainer().LoadConfiguration();
			DependencyResolver.SetResolver(new UnityDependencyResolver(container));
			/*
			IUnityContainer container = new UnityContainer();
			container.RegisterType<GameController>();
			container.RegisterType<IGameRepository, LocalRepository>();
			DependencyResolver.SetResolver(new DemoUnityDependencyResolver(container));
			*/
		}
    }
}
