namespace FftaExtract.Web
{
    using System.Web.Http;

    using FftaExtract.Web.App_Start;

    using Ninject;
    using Ninject.Extensions.Logging;
    using Ninject.Web.WebApi;

    using Swashbuckle.Application;
    using Swashbuckle.Swagger;

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var kernel = NinjectWebCommon.bootstrapper.Kernel;
            config.DependencyResolver = new NinjectDependencyResolver(kernel);

            config.Filters.Add(kernel.Get<LogError>());

            config.MapHttpAttributeRoutes();
        }
    }
}
