using Ninject.Modules;
using PrPr_Project.BL.Interfaces;
using PrPr_Project.BL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrPr_Project.WEB.Util
{
    public class AdminModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IAdminService>().To<AdminService>();
        }
    }
}