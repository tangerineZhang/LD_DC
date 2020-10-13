using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LD_DC.Startup))]
namespace LD_DC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
