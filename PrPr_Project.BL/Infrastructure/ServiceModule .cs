using Ninject.Modules;
using PrPr_Project.DAL.Interfaces;
using PrPr_Project.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrPr_Project.BL.Infrastructure
{
    public class ServiceModule : NinjectModule
    {
       
        public override void Load()
        {
            Bind<IUnitOfWork>().To<EFUnitOfWork>();
        }
    }
}
