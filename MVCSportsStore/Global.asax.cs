using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using MVCSportsStore.Domain.Entities;
using MVCSportsStore.Infrastructure.Binders;

namespace MVCSportsStore
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ModelBinders.Binders.Add(typeof(Cart), new CartModelBinder());
        }
    }
}
