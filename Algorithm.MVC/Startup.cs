using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AutoTimber.MVC.Startup))]
namespace AutoTimber.MVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
