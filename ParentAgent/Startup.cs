using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ParentAgent.Startup))]
namespace ParentAgent
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
