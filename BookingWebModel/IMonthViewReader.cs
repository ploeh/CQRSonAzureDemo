using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.Booking.WebModel
{
    public interface IMonthViewReader
    {
        IEnumerable<string> Read(int year, int month);
    }
}
