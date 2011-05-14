using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture;
using System.Web.Mvc;

namespace Ploeh.Samples.Booking.WebModel.UnitTest
{
    public class MvcCustomization : ICustomization
    {
        #region ICustomization Members

        public void Customize(IFixture fixture)
        {
            fixture.Customize<ViewDataDictionary>(c =>
                c.Without(vdd => vdd.ModelMetadata));
        }

        #endregion
    }
}
