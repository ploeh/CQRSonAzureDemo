using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.MicroKernel.SubSystems.Configuration;
using System.Net.Mail;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace Ploeh.Samples.Booking.WorkerRole
{
    public class MailAddressWindsorInstaller : IWindsorInstaller
    {
        #region IWindsorInstaller Members

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component
                .For<MailAddress>()
                .UsingFactoryMethod(() =>
                {
                    var senderAddress = RoleEnvironment.GetConfigurationSettingValue("EmailSenderSmtpAddress");
                    var senderDisplayName = RoleEnvironment.GetConfigurationSettingValue("EmailSenderDisplayName");
                    return new MailAddress(senderAddress, senderDisplayName);
                }));
        }

        #endregion
    }
}
