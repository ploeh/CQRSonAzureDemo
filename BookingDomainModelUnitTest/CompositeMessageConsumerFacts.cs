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

        [Theory, AutoDomainData]
        public void ConsumeConsumesAllConsumers(CompositeMessageConsumer<object> sut, object message)
        {
            sut.Consume(message);
            var mocks = (from c in sut.Consumers
                         select Mock.Get(c)).ToList();
            mocks.ForEach(m =>
                m.Verify(c =>
                    c.Consume(message)));
        }
    }
}
