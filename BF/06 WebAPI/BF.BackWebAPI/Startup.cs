using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BF.BackWebAPI.Startup))]
namespace BF.BackWebAPI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
        }
    }
}
