using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace Ploeh.Samples.Booking.WebModel.UnitTest
{
    public class BookingWebModelCustomization : CompositeCustomization
    {
        public BookingWebModelCustomization()
            : base(
                new AutoMoqCustomization(),
                new MvcCustomization())
        {
        }
    }
}
