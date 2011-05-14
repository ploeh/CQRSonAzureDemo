using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Xunit;
using Ploeh.AutoFixture;

namespace Ploeh.Samples.Booking.DomainModel.UnitTest
{
    public class AutoDomainDataAttribute : AutoDataAttribute
    {
        public AutoDomainDataAttribute()
            : base(new Fixture().Customize(new DomainCustomization()))
        {
        }
    }
}
