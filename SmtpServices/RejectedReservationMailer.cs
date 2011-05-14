using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using Ploeh.Samples.Booking.DomainModel;

namespace Ploeh.Samples.Booking.Smtp
{
    public class RejectedReservationMailer : IMessageConsumer<ReservationRejectedEvent>
    {
        private readonly Action<MailMessage> send;
        private readonly MailAddress sender;

        public RejectedReservationMailer(Action<MailMessage> mailClient, MailAddress sender)
        {
            this.send = mailClient;
            this.sender = sender;
        }

        #region IMessageConsumer<ReservationRejectedEvent> Members

        public void Consume(ReservationRejectedEvent message)
        {
            var msg = new MailMessage();
            msg.From = this.sender;
            msg.To.Add(new MailAddress(message.Email));
            msg.Subject = "Your Reservation Could Not Be Fulfilled";
            msg.Body = string.Format("Dear {1}{0}{0}Thank you for your inquiry. Unfortunately we must inform you that we can't accomodate {2} guest(s) at {3}. Please inquire again about another date.{0}{0}Regards", Environment.NewLine, message.Name, message.Quantity, message.Date.ToShortDateString());

            this.send(msg);
        }

        #endregion
    }
}
