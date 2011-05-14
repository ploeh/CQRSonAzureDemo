using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.Booking.WebModel
{
    public interface IDayViewReader
    {
        int GetRemainingCapacity(DateTime date);
    }
}
