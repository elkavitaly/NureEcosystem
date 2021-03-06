﻿using Ninject;
using Ninject.Modules;
using PrPr_Project.BL.Infrastructure;
using PrPr_Project.WEB.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace PrPr_Project.WEB
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


            NinjectModule adminModule = new AdminModule();
            NinjectModule serviceModule = new ServiceModule();
            NinjectModule apiModule = new ApiModule();
            var kernel = new StandardKernel(adminModule, serviceModule, apiModule);
            DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));
        }
    }
}