using Ninject.Modules;
using PrPr_Project.BL.Interfaces;
using PrPr_Project.BL.Services;

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