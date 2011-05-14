using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.Samples.Booking.DomainModel;

namespace Ploeh.Samples.Booking.WebModel
{
    public class MonthViewModelUpdater : IMessageConsumer<SoldOutEvent>
    {
        private readonly IMonthViewWriter writer;

        public MonthViewModelUpdater(IMonthViewWriter writer)
        {
            if (writer == null)
            {
                throw new ArgumentNullException("writer");
            }

            this.writer = writer;
        }

        #region IMessageConsumer<DateDepletedEvent> Members

        public void Consume(SoldOutEvent message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            this.writer.Disable(message.Date);
        }

        #endregion
    }
}
