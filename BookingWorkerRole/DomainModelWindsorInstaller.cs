using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Ploeh.Samples.Booking.Azure;
using Ploeh.Samples.Booking.DomainModel;
using Ploeh.Samples.Booking.Smtp;
using Ploeh.Samples.Booking.WebModel;

namespace Ploeh.Samples.Booking.WorkerRole
{
    public class DomainModelWindsorInstaller : IWindsorInstaller
    {
        #region IWindsorInstaller Members

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component
                .For<IMessageConsumer<MakeReservationCommand>>()
                .UsingFactoryMethod(k =>
                {
                    var guard = k.Resolve<CapacityGuard>();
                    var first = new CompositeMessageConsumer<MakeReservationCommand>(guard, k.Resolve<ReservationWriter>(), k.Resolve<AcceptingReservationConsumer>());
                    var second = k.Resolve<RejectingReservationConsumer>();
                    return new ConditionalMessageConsumer<MakeReservationCommand>(guard.HasCapacity, first, second);
                }));
            container.Register(Component
                .For<IMessageConsumer<ReservationAcceptedEvent>>()
                .ImplementedBy<AcceptedReserverationMailer>());
            container.Register(Component
                .For<IMessageConsumer<ReservationRejectedEvent>>()
                .ImplementedBy<RejectedReservationMailer>());
            container.Register(Component
                .For<IMessageConsumer<SoldOutEvent>>()
                .ImplementedBy<MonthViewModelUpdater>());
            container.Register(Component
                .For<ReservationWriter>());
            container.Register(Component
                .For<AcceptingReservationConsumer>());
            container.Register(Component
                .For<RejectingReservationConsumer>());
        }

        #endregion
    }
}
