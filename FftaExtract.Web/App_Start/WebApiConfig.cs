namespace FftaExtract.Web
{
    using System.Web.Http;

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    name: "PalmaresController",
            //    routeTemplate: "api/Palmares/{code}",
            //    defaults: new { controller = "Palmares", year = RouteParameter.Optional });

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
                routeTemplate: "api/{controller}/{year}/{category}/{competitionType}/{bowType}/{page}",
                defaults: new
                {
                    page = 0,
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
