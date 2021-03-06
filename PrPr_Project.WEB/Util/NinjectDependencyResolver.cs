﻿using Ninject;
using PrPr_Project.BL.Interfaces;
using PrPr_Project.BL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PrPr_Project.BL.Api;

namespace PrPr_Project.WEB.Util
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            kernel.Bind<IAdminService>().To<AdminService>();
            kernel.Bind<IApi>().To<JsonApi>();
        }
    }
}