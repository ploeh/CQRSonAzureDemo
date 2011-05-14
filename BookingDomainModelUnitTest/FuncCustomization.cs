using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture;

namespace Ploeh.Samples.Booking.DomainModel.UnitTest
{
    public class FuncCustomization : ICustomization
    {
        #region ICustomization Members

        public void Customize(IFixture fixture)
        {
            fixture.Inject<Func<bool>>(() => false);
            fixture.Inject<Func<object, bool>>(x => false);
        }

        #endregion
    }
}
