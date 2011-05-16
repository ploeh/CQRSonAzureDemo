using System;
using System.Linq;
using Moq;
using Ploeh.AutoFixture.Xunit;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.Samples.Booking.DomainModel.UnitTest
{
    public class ObjectCompositeMessageConsumerFacts : CompositeMessageConsumerFacts<object> { }
    public class Int32CompositeMessageConsumerFacts : CompositeMessageConsumerFacts<int> { }
    public class StringCompositeMessageConsumerFacts : CompositeMessageConsumerFacts<string> { }
    public class VersionCompositeMessageConsumerFacts : CompositeMessageConsumerFacts<Version> { }

    public abstract class CompositeMessageConsumerFacts<T>
    {
        [Theory, AutoDomainData]
        public void SutIsMessageConsumer(CompositeMessageConsumer<T> sut)
        {
            Assert.IsAssignableFrom<IMessageConsumer<T>>(sut);
        }

        [Theory, AutoDomainData]
        public void ConsumersIsCorrect([Frozen]IMessageConsumer<T>[] consumers, CompositeMessageConsumer<T> sut)
        {
            Assert.True(consumers.SequenceEqual(sut.Consumers));
        }

        [Theory, AutoDomainData]
        public void ConsumeConsumesAllConsumers(CompositeMessageConsumer<T> sut, T message)
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
