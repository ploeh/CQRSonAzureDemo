using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Ploeh.Samples.Booking.WebUI
{
    public class BookingWebRoleWindsorInstaller : IWindsorInstaller
    {
        #region IWindsorInstaller Members

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            new WebModelWindsorInstaller().Install(container, store);
            new AzureServicesWindsorInstaller().Install(container, store);
        }

        #endregion
    }
}