using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.Samples.Booking.DomainModel;

namespace Ploeh.Samples.Booking.WebModel
{
    public class BookingViewModel
    {
        public DateTime Date { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public int Quantity { get; set; }

        public int Remaining { get; set; }

        public MakeReservationCommand MakeNewReservation()
        {
            return new MakeReservationCommand(this.Date,
                this.Name, this.Email, this.Quantity);
        }
    }
}
