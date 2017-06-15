using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Micro.Startup))]
namespace Micro
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
