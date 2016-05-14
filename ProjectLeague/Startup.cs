using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ProjectLeague.Startup))]
namespace ProjectLeague
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
