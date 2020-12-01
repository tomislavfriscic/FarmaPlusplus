using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FarmaPlus.Startup))]
namespace FarmaPlus
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
           // ConfigureAuth(app);
        }
    }
}
