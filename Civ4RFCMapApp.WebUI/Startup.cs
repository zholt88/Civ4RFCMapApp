using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Civ4RFCMapApp.WebUI.Startup))]
namespace Civ4RFCMapApp.WebUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
