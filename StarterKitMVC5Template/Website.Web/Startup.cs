using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof($safeprojectname$.Startup))]
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config", Watch = true)]
namespace $safeprojectname$
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
