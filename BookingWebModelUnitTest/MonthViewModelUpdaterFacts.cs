using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit.Extensions;
using Xunit;
using Ploeh.Samples.Booking.WebModel;
using Ploeh.Samples.Booking.DomainModel;
using Ploeh.AutoFixture.Xunit;
using Moq;
using Ploeh.AutoFixture;

namespace Ploeh.Samples.Booking.WebModel.UnitTest
{
    public class MonthViewModelUpdaterFacts
    {
        [Theory, AutoWebData]
        public void SutIsConsumer(MonthViewModelUpdater sut)
        {
            Assert.IsAssignableFrom<IMessageConsumer<SoldOutEvent>>(sut);
        }

        [Theory, AutoWebData]
        public void ConsumeCorrectlyUpdatesViewStore([Frozen]Mock<IMonthViewWriter> storeMock, MonthViewModelUpdater sut, SoldOutEvent @event)
        {
            sut.Consume(@event);
            storeMock.Verify(s => s.Disable(@event.Date));
        }
    }
}
