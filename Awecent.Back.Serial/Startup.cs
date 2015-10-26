using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Awecent.Back.Serial.Startup))]
namespace Awecent.Back.Serial
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
