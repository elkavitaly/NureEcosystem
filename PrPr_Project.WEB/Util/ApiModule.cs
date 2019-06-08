using Ninject.Modules;
using PrPr_Project.BL.Api;
using PrPr_Project.BL.Interfaces;

namespace PrPr_Project.WEB.Util
{
    public class ApiModule : NinjectModule
    {
        public override void Load() => Bind<IApi>().To<JsonApi>();
    }
}