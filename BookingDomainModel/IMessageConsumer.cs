using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.Booking.DomainModel
{
    public interface IMessageConsumer<T>
    {
        void Consume(T message);
    }
}
