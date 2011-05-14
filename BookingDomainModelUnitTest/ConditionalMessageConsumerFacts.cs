using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit.Extensions;
using Xunit;
using Ploeh.Samples.Booking.DomainModel;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit;
using Moq;

namespace Ploeh.Samples.Booking.DomainModel.UnitTest
{
    public class ConditionalMessageConsumerFacts
    {
        [Theory, AutoDomainData]
        public void SutIsMessageConsumer(ConditionalMessageConsumer<object> sut)
        {
            Assert.IsAssignableFrom<IMessageConsumer<object>>(sut);
        }

        [Theory, AutoDomainData]
        public void FirstIsCorrect(IMessageConsumer<object> first, IMessageConsumer<object> second, Func<object, bool> condition)
        {
            var sut = ConditionalMessageConsumerFacts.CreateSut(first, second, condition);
            IMessageConsumer<object> result = sut.First;
            Assert.Equal(first, result);
        }

        [Theory, AutoDomainData]
        public void SecondIsCorrect(IMessageConsumer<object> first, IMessageConsumer<object> second, Func<object, bool> condition)
        {
            var sut = ConditionalMessageConsumerFacts.CreateSut(first, second, condition);
            IMessageConsumer<object> result = sut.Second;
            Assert.Equal(second, result);
        }

        [Theory, AutoDomainData]
        public void ConditionIsCorrect([Frozen]Func<object, bool> condition, ConditionalMessageConsumer<object> sut)
        {
            Func<object, bool> result = sut.Condition;
            Assert.Equal(condition, result);
        }

        [Theory, AutoDomainData]
        public void FirstIsInvokedWhenConditionIsTrue(Mock<IMessageConsumer<object>> firstMock, Mock<IMessageConsumer<object>> secondStub, object message)
        {
            var sut = ConditionalMessageConsumerFacts.CreateSut(firstMock.Object, secondStub.Object, m => true);
            sut.Consume(message);
            firstMock.Verify(c => c.Consume(message));
        }

        [Theory, AutoDomainData]
        public void SecondIsNotInvokedWhenConditionIsTrue(Mock<IMessageConsumer<object>> firstStub, Mock<IMessageConsumer<object>> secondMock, object message)
        {
            var sut = ConditionalMessageConsumerFacts.CreateSut(firstStub.Object, secondMock.Object, m => true);
            sut.Consume(message);
            secondMock.Verify(c => c.Consume(It.IsAny<object>()), Times.Never());
        }

        [Theory, AutoDomainData]
        public void SecondIsInvokedWhenConditionIsFalse(Mock<IMessageConsumer<object>> firstStub, Mock<IMessageConsumer<object>> secondMock, object message)
        {
            var sut = ConditionalMessageConsumerFacts.CreateSut(firstStub.Object, secondMock.Object, m => false);
            sut.Consume(message);
            secondMock.Verify(c => c.Consume(message));
        }

        [Theory, AutoDomainData]
        public void FirstIsNotInvokedWhenConditionIsFalse(Mock<IMessageConsumer<object>> firstMock, Mock<IMessageConsumer<object>> secondStub, object message)
        {
            var sut = ConditionalMessageConsumerFacts.CreateSut(firstMock.Object, secondStub.Object, m => false);
            sut.Consume(message);
            firstMock.Verify(c => c.Consume(It.IsAny<object>()), Times.Never());
        }

        private static ConditionalMessageConsumer<T> CreateSut<T>(IMessageConsumer<T> first, IMessageConsumer<T> second, Func<T, bool> condition)
        {
            return new ConditionalMessageConsumer<T>(condition, first, second);
        }        
    }
}
