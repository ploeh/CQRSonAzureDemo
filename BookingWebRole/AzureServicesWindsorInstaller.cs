using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.MicroKernel.SubSystems.Configuration;
using Ploeh.Samples.Booking.WebModel;
using Ploeh.Samples.Booking.Azure;
using Ploeh.Samples.Booking.DomainModel;
using Microsoft.WindowsAzure.StorageClient;
using Microsoft.WindowsAzure;

namespace Ploeh.Samples.Booking.WebUI
{
    public class AzureServicesWindsorInstaller : IWindsorInstaller
    {
        #region IWindsorInstaller Members

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component
                .For<IMonthViewReader>()
                .ImplementedBy<AzureMonthViewStore>()
                .ServiceOverrides(new { viewContainer = "viewContainer" }));
            container.Register(Component
                .For<IDayViewReader>()
                .ImplementedBy<CapacityGuard>()
                .ServiceOverrides(new { viewContainer = "capacityContainer" }));
            container.Register(Component
                .For<IChannel>()
                .ImplementedBy<AzureChannel>()
                .LifeStyle.PerWebRequest);
            container.Register(Component
                .For<CloudQueue>()
                .UsingFactoryMethod(k =>
                {
                    var queueName = "messagequeue";
                    var client = k.Resolve<CloudQueueClient>();
                    var queue = client.GetQueueReference(queueName);
                    queue.CreateIfNotExist();
                    return queue;
                })
                .LifeStyle.PerWebRequest);
            container.Register(Component
                .For<CloudQueueClient>()
                .UsingFactoryMethod(k => k.Resolve<CloudStorageAccount>().CreateCloudQueueClient())
                .LifeStyle.PerWebRequest);
            container.Register(Component
                .For<CloudBlobContainer>()
                .UsingFactoryMethod(k =>
                {
                    var c = k.Resolve<CloudBlobClient>().GetContainerReference("capacity");
                    c.CreateIfNotExist();
                    return c;
                })
                .Named("capacityContainer"));
            container.Register(Component
                .For<CloudBlobContainer>()
                .UsingFactoryMethod(k =>
                {
                    var c = k.Resolve<CloudBlobClient>().GetContainerReference("view");
                    c.CreateIfNotExist();
                    return c;
                })
                .Named("viewContainer"));
            container.Register(Component
                .For<CloudBlobClient>()
                .UsingFactoryMethod(k => k.Resolve<CloudStorageAccount>().CreateCloudBlobClient()));
            container.Register(Component
                .For<CloudStorageAccount>()
                .UsingFactoryMethod(() => CloudStorageAccount.FromConfigurationSetting("DataConnectionString"))
                .LifeStyle.PerWebRequest);
        }

        #endregion
    }
}