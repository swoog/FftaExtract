using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(FftaExtract.Web.App_Start.Startup))]

namespace FftaExtract.Web.App_Start
{
    using Ninject.Web.WebApi;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Pour plus d'informations sur la façon de configurer votre application, consultez http://go.microsoft.com/fwlink/?LinkID=316888
            //app.Use<NinjectDependencyResolver>();
        }
    }
}
