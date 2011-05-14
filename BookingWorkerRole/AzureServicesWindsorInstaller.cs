using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.MicroKernel.SubSystems.Configuration;
using Ploeh.Samples.Booking.DomainModel;
using Ploeh.Samples.Booking.Azure;
using Ploeh.Samples.Booking.WebModel;
using Microsoft.WindowsAzure.StorageClient;
using Microsoft.WindowsAzure;

namespace Ploeh.Samples.Booking.WorkerRole
{
    public class AzureServicesWindsorInstaller : IWindsorInstaller
    {

        #region IWindsorInstaller Members

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component
                .For<IReservationRepository>()
                .ImplementedBy<AzureReservationRepository>());

            container.Register(AllTypes.FromAssemblyContaining<ReservationContext>()
                .Where(t => t.Name.EndsWith("Context") && typeof(IRequiresInitialization).IsAssignableFrom(t))
                .WithService.Select((type, baseTypes) => new[] { type, typeof(IRequiresInitialization) }));

            container.Register(Component
                .For<IChannel>()
                .ImplementedBy<AzureChannel>());
            container.Register(Component
                .For<CapacityGuard>()
                .ServiceOverrides(new { capacityContainer = "capacityContainer" }));
            container.Register(Component
                .For<IMonthViewWriter>()
                .ImplementedBy<AzureMonthViewStore>()
                .ServiceOverrides(new { viewContainer = "viewContainer" }));

            container.Register(Component
                .For<CloudQueue>()
                .UsingFactoryMethod(k =>
                {
                    var queueName = "messagequeue";
                    var client = k.Resolve<CloudQueueClient>();
                    var queue = client.GetQueueReference(queueName);
                    queue.CreateIfNotExist();
                    return queue;
                }));
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
                .For<CloudQueueClient>()
                .UsingFactoryMethod(k => k.Resolve<CloudStorageAccount>().CreateCloudQueueClient()));
            container.Register(Component
                .For<CloudBlobClient>()
                .UsingFactoryMethod(k => k.Resolve<CloudStorageAccount>().CreateCloudBlobClient()));
            container.Register(Component
                .For<CloudStorageAccount>()
                .UsingFactoryMethod(() => CloudStorageAccount.FromConfigurationSetting("DataConnectionString")));

            foreach (var i in container.ResolveAll<IRequiresInitialization>())
            {
                i.Initialize();
            }
        }

        #endregion
    }
}
