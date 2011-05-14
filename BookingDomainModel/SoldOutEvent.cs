using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.Booking.DomainModel
{
    [Serializable]
    public class SoldOutEvent
    {
        private readonly DateTime date;

        public SoldOutEvent(DateTime date)
        {
            this.date = date;
        }

        public DateTime Date
        {
            get { return this.date; }
        }
    }
}
