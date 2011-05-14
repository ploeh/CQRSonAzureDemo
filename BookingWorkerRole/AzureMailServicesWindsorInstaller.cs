using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using System.Net.Mail;
using Ploeh.Samples.Booking.Azure;

namespace Ploeh.Samples.Booking.WorkerRole
{
    public class AzureMailServicesWindsorInstaller : IWindsorInstaller
    {
        #region IWindsorInstaller Members

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component
                .For<Action<MailMessage>>()
                .UsingFactoryMethod<Action<MailMessage>>(k => k.Resolve<TableStorageMailClient>().Send));
            container.Register(Component
                .For<TableStorageMailClient>());

            new MailAddressWindsorInstaller().Install(container, store);
        }

        #endregion
    }
}
