namespace FftaExtract.Web
{
    using System.Web.Http;

    using FftaExtract.Web.App_Start;

    using Ninject;
    using Ninject.Extensions.Logging;
    using Ninject.Web.WebApi;

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var kernel = NinjectWebCommon.bootstrapper.Kernel;
            config.DependencyResolver  =new NinjectDependencyResolver(kernel);

            config.Filters.Add(new LogError(kernel.Get<ILogger>()));

            config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    name: "PalmaresController",
            //    routeTemplate: "api/Palmares/{code}",
            //    defaults: new { controller = "Palmares", year = RouteParameter.Optional });

            config.Routes.MapHttpRoute(
                name: "ResultatController",
                routeTemplate: "api/resultat/{code}/{beginDate}/{endDate}",
                defaults: new
                {
                    controller = "Resultat",
                    code = RouteParameter.Optional,
                    beginDate = RouteParameter.Optional,
                    endDate = RouteParameter.Optional
                });


            config.Routes.MapHttpRoute(
                name: "PalmaresController",
                routeTemplate: "api/Palmares/{code}/{year}/{category}/{competitionType}/{bowType}",
                defaults: new
                              {
                                  controller = "Palmares", 
                                  year = RouteParameter.Optional,
                                  category = RouteParameter.Optional,
                                  competitionType = RouteParameter.Optional,
                                  bowType = RouteParameter.Optional
                              });


            config.Routes.MapHttpRoute(
                name: "ClassmentController",
                routeTemplate: "api/{controller}/{year}/{category}/{competitionType}/{bowType}",
                defaults: new
                {
                    category = RouteParameter.Optional,
                    competitionType = RouteParameter.Optional,
                    bowType = RouteParameter.Optional
                });


            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });
        }
    }
}
