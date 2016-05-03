using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BetterThanMooshak.Startup))]
namespace BetterThanMooshak
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
