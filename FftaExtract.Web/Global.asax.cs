using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace FftaExtract.Web
{
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Web.Http;

    using FftaExtract.DatabaseModel;

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<FftaDatabase, FftaConfiguration>());

            Database.SetInitializer<FftaDatabase>(null);
            var configuration = new FftaConfiguration();
            var migrator = new DbMigrator(configuration);
            migrator.Update();

            log4net.Config.XmlConfigurator.Configure();

            GlobalConfiguration.Configure(WebApiConfig.Register);
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
    public class FftaConfiguration : DbMigrationsConfiguration<FftaDatabase>
    {
        public FftaConfiguration()
        {
            this.AutomaticMigrationsEnabled = true;
        }
    }
}
