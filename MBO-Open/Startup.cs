using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MBO_Open.Startup))]
namespace MBO_Open
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
