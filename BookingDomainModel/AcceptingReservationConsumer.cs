using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.Booking.DomainModel
{
    public class AcceptingReservationConsumer : IMessageConsumer<MakeReservationCommand>
    {
        private readonly IChannel channel;

        public AcceptingReservationConsumer(IChannel channel)
        {
            if (channel == null)
            {
                throw new ArgumentNullException("channel");
            }

            this.channel = channel;
        }

        #region IMessageConsumer<MakeNewReservationCommand> Members

        public void Consume(MakeReservationCommand message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            this.channel.Send(message.Accept());
        }

        #endregion
    }
}
