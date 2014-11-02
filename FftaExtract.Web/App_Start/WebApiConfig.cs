namespace FftaExtract.Web
{
    using System.Web.Http;

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "PalmaresController",
                routeTemplate: "api/{controller}/{code}");

            config.Routes.MapHttpRoute(
                name: "ClassmentController",
                routeTemplate: "api/{controller}/{year}/{category}/{competitionType}/{bowType}/{page}",
                defaults: new { page = 0 });


            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });
        }
    }
}
