using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit.Extensions;
using Xunit;
using Ploeh.Samples.Booking.DomainModel;
using Ploeh.AutoFixture.Xunit;
using Ploeh.AutoFixture;
using Moq;

namespace Ploeh.Samples.Booking.DomainModel.UnitTest
{
    public class CompositeMessageConsumerFacts
    {
        [Theory, AutoDomainData]
        public void SutIsMessageConsumer(CompositeMessageConsumer<object> sut)
        {
            Assert.IsAssignableFrom<IMessageConsumer<object>>(sut);
        }

        [Theory, AutoDomainData]
        public void ConsumersIsCorrect([Frozen]IMessageConsumer<object>[] consumers, CompositeMessageConsumer<object> sut)
        {
            Assert.True(consumers.SequenceEqual(sut.Consumers));
        }

        [Fact]
        public void ConsumeConsumesAllConsumers()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new DomainCustomization());
            var mocks = fixture.CreateMany<Mock<IMessageConsumer<object>>>().ToList();
            fixture.Inject(mocks.Select(m => m.Object).ToArray());

            var message = fixture.CreateAnonymous<object>();

            var sut = fixture.CreateAnonymous<CompositeMessageConsumer<object>>();
            // Exercise system
            sut.Consume(message);
            // Verify outcome
            mocks.ForEach(m =>
                m.Verify(c =>
                    c.Consume(message)));
            // Teardown
        }
    }
}
