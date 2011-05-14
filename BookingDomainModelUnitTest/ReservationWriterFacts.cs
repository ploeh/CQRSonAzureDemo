using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit.Extensions;
using Xunit;
using Ploeh.Samples.Booking.DomainModel;
using Ploeh.AutoFixture.Xunit;
using Moq;
using Ploeh.SemanticComparison.Fluent;
using Ploeh.AutoFixture;

namespace Ploeh.Samples.Booking.DomainModel.UnitTest
{
    public class ReservationWriterFacts
    {
        [Theory, AutoDomainData]
        public void SutIsMessageConsumer(ReservationWriter sut)
        {
            Assert.IsAssignableFrom<IMessageConsumer<MakeReservationCommand>>(sut);
        }

        [Theory, AutoDomainData]
        public void ConsumeAddsReservationToRepository([Frozen]Mock<IReservationRepository> repositoryMock, ReservationWriter sut, MakeReservationCommand cmd)
        {
            var expectedReseveration = cmd.Accept().AsSource().OfLikeness<ReservationAcceptedEvent>();
            sut.Consume(cmd);
            repositoryMock.Verify(r => r.AddReservation(It.Is<ReservationAcceptedEvent>(e => expectedReseveration.Equals(e))));
        }
    }
}
