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
    public class RejectingReservationConsumerFacts
    {
        [Theory, AutoDomainData]
        public void SutIsCorrectConsumer(RejectingReservationConsumer sut)
        {
            Assert.IsAssignableFrom<IMessageConsumer<MakeReservationCommand>>(sut);
        }

        [Theory, AutoDomainData]
        public void ConsumePublishesEvent([Frozen]Mock<IChannel> channelMock, RejectingReservationConsumer sut, MakeReservationCommand cmd)
        {
            var expectedEvent = cmd.Reject().AsSource().OfLikeness<ReservationRejectedEvent>();
            sut.Consume(cmd);
            channelMock.Verify(c => c.Send(expectedEvent));
        }
    }
}
