using System;
using Moq;
using Ploeh.AutoFixture.Xunit;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.Samples.Booking.DomainModel.UnitTest
{
    public class ObjectConditionalMessageConsumerFacts : ConditionalMessageConsumerFacts<object> { }
    public class Int32ConditionalMessageConsumerFacts : ConditionalMessageConsumerFacts<int> { }
    public class StringConditionalMessageConsumerFacts : ConditionalMessageConsumerFacts<string> { }
    public class VersionConditionalMessageConsumerFacts : ConditionalMessageConsumerFacts<Version> { }

    public abstract class ConditionalMessageConsumerFacts<T>
    {
        [Theory, AutoDomainData]
        public void SutIsMessageConsumer(ConditionalMessageConsumer<object> sut)
        {
            Assert.IsAssignableFrom<IMessageConsumer<object>>(sut);
        }

        [Theory, AutoDomainData]
        public void FirstIsCorrect(IMessageConsumer<T> first, IMessageConsumer<T> second, Func<T, bool> condition)
        {
            var sut = ConditionalMessageConsumerFacts<T>.CreateSut(first, second, condition);
            IMessageConsumer<T> result = sut.First;
            Assert.Equal(first, result);
        }

        [Theory, AutoDomainData]
        public void SecondIsCorrect(IMessageConsumer<T> first, IMessageConsumer<T> second, Func<T, bool> condition)
        {
            var sut = ConditionalMessageConsumerFacts<T>.CreateSut(first, second, condition);
            IMessageConsumer<T> result = sut.Second;
            Assert.Equal(second, result);
        }

        [Theory, AutoDomainData]
        public void ConditionIsCorrect([Frozen]Func<T, bool> condition, ConditionalMessageConsumer<T> sut)
        {
            Func<T, bool> result = sut.Condition;
            Assert.Equal(condition, result);
        }

        [Theory, AutoDomainData]
        public void FirstIsInvokedWhenConditionIsTrue(Mock<IMessageConsumer<T>> firstMock, Mock<IMessageConsumer<T>> secondStub, T message)
        {
            var sut = ConditionalMessageConsumerFacts<T>.CreateSut(firstMock.Object, secondStub.Object, m => true);
            sut.Consume(message);
            firstMock.Verify(c => c.Consume(message));
        }

        [Theory, AutoDomainData]
        public void SecondIsNotInvokedWhenConditionIsTrue(Mock<IMessageConsumer<T>> firstStub, Mock<IMessageConsumer<T>> secondMock, T message)
        {
            var sut = ConditionalMessageConsumerFacts<T>.CreateSut(firstStub.Object, secondMock.Object, m => true);
            sut.Consume(message);
            secondMock.Verify(c => c.Consume(It.IsAny<T>()), Times.Never());
        }

        [Theory, AutoDomainData]
        public void SecondIsInvokedWhenConditionIsFalse(Mock<IMessageConsumer<T>> firstStub, Mock<IMessageConsumer<T>> secondMock, T message)
        {
            var sut = ConditionalMessageConsumerFacts<T>.CreateSut(firstStub.Object, secondMock.Object, m => false);
            sut.Consume(message);
            secondMock.Verify(c => c.Consume(message));
        }

        [Theory, AutoDomainData]
        public void FirstIsNotInvokedWhenConditionIsFalse(Mock<IMessageConsumer<T>> firstMock, Mock<IMessageConsumer<T>> secondStub, T message)
        {
            var sut = ConditionalMessageConsumerFacts<T>.CreateSut(firstMock.Object, secondStub.Object, m => false);
            sut.Consume(message);
            firstMock.Verify(c => c.Consume(It.IsAny<T>()), Times.Never());
        }

        private static ConditionalMessageConsumer<T> CreateSut(IMessageConsumer<T> first, IMessageConsumer<T> second, Func<T, bool> condition)
        {
            return new ConditionalMessageConsumer<T>(condition, first, second);
        }        
    }
}
