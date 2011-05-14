using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Ploeh.Samples.Booking.WebModel;

namespace Ploeh.Samples.Booking.WebUI
{
    public class WebModelWindsorInstaller : IWindsorInstaller
    {
        #region IWindsorInstaller Members

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(AllTypes
                .FromAssemblyContaining<HomeController>()
                .BasedOn<IController>()
                .Configure(r => r.LifeStyle.PerWebRequest));
        }

        #endregion
    }
}