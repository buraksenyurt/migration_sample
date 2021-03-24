using Autofac;
using Autofac.Integration.Mvc;
using ClassicGames.DAL;
using Serilog;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace WebClient
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ConfigureDI();
        }

        private static void ConfigureDI()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterModule(new ClassicGamesDBModule());

            IContainer container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        protected void Application_Error()
        {
            var exc = Server.GetLastError();
            Log.Error(exc, $"Hata oluştu.{exc.Message}");
        }
    }
}
