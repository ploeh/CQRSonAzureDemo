using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit.Extensions;
using Xunit;
using Ploeh.Samples.Booking.DomainModel;
using Ploeh.AutoFixture.Xunit;
using Ploeh.AutoFixture;

namespace Ploeh.Samples.Booking.DomainModel.UnitTest
{
    public class SoldOutEventFacts
    {
        [Theory, AutoDomainData]
        public void DateIsCorrect([Frozen]DateTime date, SoldOutEvent sut)
        {
            Assert.Equal<DateTime>(date, sut.Date);
        }
    }
}
