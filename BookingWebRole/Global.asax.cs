using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Ploeh.Samples.Booking.WebModel;
using Castle.Windsor;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace Ploeh.Samples.Booking.WebUI
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        private readonly IWindsorContainer container;

        public MvcApplication()
        {
            this.container = new WindsorContainer().Install(new BookingWebRoleWindsorInstaller());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Date",
                url: "{controller}/{action}/{year}-{month}-{day}",
                defaults: new { controller = "Home", action = "Index" },
                namespaces: new[] { typeof(HomeController).Namespace }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { typeof(HomeController).Namespace }
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            MvcApplication.RegisterRoutes(RouteTable.Routes);

            CloudStorageAccount.SetConfigurationSettingPublisher((configName, configSetter) =>
                configSetter(RoleEnvironment.GetConfigurationSettingValue(configName)));

            ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(this.container));
        }

        public override void Dispose()
        {
            this.container.Dispose();
            base.Dispose();
        }
    }
}