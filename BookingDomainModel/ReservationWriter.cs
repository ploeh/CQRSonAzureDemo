using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.Booking.DomainModel
{
    public class ReservationWriter : IMessageConsumer<MakeReservationCommand>
    {
        private readonly IReservationRepository repository;

        public ReservationWriter(IReservationRepository repository)
        {
            if (repository == null)
            {
                throw new ArgumentNullException("repository");
            }

            this.repository = repository;
        }

        #region ICommandConsumer<MakeReservationCommand> Members

        public void Consume(MakeReservationCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            this.repository.AddReservation(command.Accept());
        }

        #endregion
    }
}
