using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVC.Caching.Startup))]
namespace MVC.Caching
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
