using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.Booking.DomainModel
{
    [Serializable]
    public class MakeReservationCommand
    {
        private readonly Guid id;
        private readonly DateTime date;
        private readonly string name;
        private readonly string email;
        private readonly int quantity;

        public MakeReservationCommand(DateTime date, string name, string email, int quantity)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (email == null)
            {
                throw new ArgumentNullException("email");
            }

            this.id = Guid.NewGuid();
            this.date = date;
            this.name = name;
            this.email = email;
            this.quantity = quantity;
        }

        public DateTime Date
        {
            get { return this.date; }
        }

        public string Email
        {
            get { return this.email; }
        }

        public Guid Id
        {
            get { return this.id; }
        }

        public string Name
        {
            get { return this.name; }
        }

        public int Quantity
        {
            get { return this.quantity; }
        }

        public ReservationAcceptedEvent Accept()
        {
            return new ReservationAcceptedEvent(this.Id, this.Date, this.Name, this.Email, this.Quantity);
        }

        public ReservationRejectedEvent Reject()
        {
            return new ReservationRejectedEvent(this.Id, this.Date, this.Name, this.Email, this.Quantity);
        }
    }
}
