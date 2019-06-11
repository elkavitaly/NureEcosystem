using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PrPr_Project.WEB.Startup))]
namespace PrPr_Project.WEB
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
