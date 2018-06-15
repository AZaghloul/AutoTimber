using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Algorithm.MVC.Startup))]
namespace Algorithm.MVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
