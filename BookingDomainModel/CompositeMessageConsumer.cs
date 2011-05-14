using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.Booking.DomainModel
{
    public class CompositeMessageConsumer<T> : IMessageConsumer<T>
    {
        private readonly IEnumerable<IMessageConsumer<T>> consumers;

        public CompositeMessageConsumer(params IMessageConsumer<T>[] consumers)
        {
            if (consumers == null)
            {
                throw new ArgumentNullException("consumers");
            }

            this.consumers = consumers;
        }

        public IEnumerable<IMessageConsumer<T>> Consumers
        {
            get { return this.consumers; }
        }

        #region IMessageConsumer<T> Members

        public void Consume(T message)
        {
            foreach (var consumer in this.Consumers)
            {
                consumer.Consume(message);
            }
        }

        #endregion
    }
}
