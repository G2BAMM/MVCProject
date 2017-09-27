using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVCWebProject2.Startup))]
namespace MVCWebProject2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
