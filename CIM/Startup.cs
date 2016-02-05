using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CIM.Startup))]
namespace CIM
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
