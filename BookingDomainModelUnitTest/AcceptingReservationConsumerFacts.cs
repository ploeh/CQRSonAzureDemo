using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit.Extensions;
using Xunit;
using Ploeh.AutoFixture.Xunit;
using Ploeh.SemanticComparison.Fluent;
using Moq;
using Ploeh.AutoFixture;

namespace Ploeh.Samples.Booking.DomainModel.UnitTest
{
    public class AcceptingReservationConsumerFacts
    {
        [Theory, AutoDomainData]
        public void SutIsCorrectMessageConsumer(AcceptingReservationConsumer sut)
        {
            Assert.IsAssignableFrom<IMessageConsumer<MakeReservationCommand>>(sut);
        }

        [Theory, AutoDomainData]
        public void ConsumePublishesEvent([Frozen]Mock<IChannel> channelMock, AcceptingReservationConsumer sut, MakeReservationCommand cmd)
        {
            var expectedEvent = cmd.Accept().AsSource().OfLikeness<ReservationAcceptedEvent>();
            sut.Consume(cmd);
            channelMock.Verify(c => c.Send(expectedEvent));
        }
    }
}
