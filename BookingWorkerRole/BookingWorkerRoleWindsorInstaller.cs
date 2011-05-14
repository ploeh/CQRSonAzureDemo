using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Ploeh.Samples.Booking.WorkerRole
{
    public class BookingWorkerRoleWindsorInstaller : IWindsorInstaller
    {
        #region IWindsorInstaller Members

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            new DomainModelWindsorInstaller().Install(container, store);

            new SmtpServicesWindsorInstaller().Install(container, store);
            //new AzureMailServicesWindsorInstaller().Install(container, store);

            new AzureServicesWindsorInstaller().Install(container, store);
        }

        #endregion
    }
}
