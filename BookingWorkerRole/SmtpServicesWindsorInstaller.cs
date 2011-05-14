using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using System.Net.Mail;
using Microsoft.WindowsAzure.ServiceRuntime;
using System.Net;

namespace Ploeh.Samples.Booking.WorkerRole
{
    public class SmtpServicesWindsorInstaller : IWindsorInstaller
    {
        #region IWindsorInstaller Members

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component
                .For<Action<MailMessage>>()
                .UsingFactoryMethod<Action<MailMessage>>(k => k.Resolve<SmtpClient>().Send));
            container.Register(Component
                .For<SmtpClient>()
                .UsingFactoryMethod(() =>
                {
                    var smtpServerAddress = RoleEnvironment.GetConfigurationSettingValue("SmtpServerAddress");
                    var smtpUserName = RoleEnvironment.GetConfigurationSettingValue("SmtpUserName");
                    var smtpPassword = RoleEnvironment.GetConfigurationSettingValue("SmtpPassword");
                    var client = new SmtpClient(smtpServerAddress);
                    client.Credentials = new NetworkCredential(smtpUserName, smtpPassword);
                    return client;
                }));

            new MailAddressWindsorInstaller().Install(container, store);
        }

        #endregion
    }
}
