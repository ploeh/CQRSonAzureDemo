using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.Booking.DomainModel
{
    public class ConditionalMessageConsumer<T> : IMessageConsumer<T>
    {
        private Func<T, bool> condition;
        private IMessageConsumer<T> first;
        private IMessageConsumer<T> second;

        public ConditionalMessageConsumer(Func<T, bool> condition, IMessageConsumer<T> first, IMessageConsumer<T> second)
        {
            if (condition == null)
            {
                throw new ArgumentNullException("condition");
            }
            if (first == null)
            {
                throw new ArgumentNullException("first");
            }
            if (second == null)
            {
                throw new ArgumentNullException("second");
            }

            this.condition = condition;
            this.first = first;
            this.second = second;
        }

        public Func<T, bool> Condition
        {
            get { return this.condition; }
        }

        public IMessageConsumer<T> First
        {
            get { return this.first; }
        }

        public IMessageConsumer<T> Second
        {
            get { return this.second; }
        }

        #region IMessageConsumer<T> Members

        public void Consume(T message)
        {
            (this.condition(message) ? this.first : this.second).Consume(message);
        }

        #endregion
    }
}
