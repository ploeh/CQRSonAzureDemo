using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.Booking.Azure
{
    public interface IRequiresInitialization
    {
        void Initialize();
    }
}
