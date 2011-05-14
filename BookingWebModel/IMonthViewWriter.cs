using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.Booking.WebModel
{
    public interface IMonthViewWriter
    {
        void Disable(DateTime date);
    }
}
