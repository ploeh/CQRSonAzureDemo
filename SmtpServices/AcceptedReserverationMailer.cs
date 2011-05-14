using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.Samples.Booking.DomainModel;
using System.Net.Mail;

namespace Ploeh.Samples.Booking.Smtp
{
    public class AcceptedReserverationMailer : IMessageConsumer<ReservationAcceptedEvent>
    {
        private readonly Action<MailMessage> send;
        private readonly MailAddress sender;

        public AcceptedReserverationMailer(Action<MailMessage> mailClient, MailAddress sender)
        {
            this.send = mailClient;
            this.sender = sender;
        }

        #region IMessageConsumer<ReservationAcceptedEvent> Members

        public void Consume(ReservationAcceptedEvent message)
        {
            var msg = new MailMessage();
            msg.From = this.sender;
            msg.To.Add(new MailAddress(message.Email));
            msg.Subject = "Your Reservation Was Accepted";
            msg.Body = string.Format("Dear {1}{0}{0}Thank you for your inquiry. We have the pleasure to inform you that we can accomodate {2} guest(s) at {3}. We look forward to seeing you.{0}{0}Regards", Environment.NewLine, message.Name, message.Quantity, message.Date.ToShortDateString());

            this.send(msg);
        }

        #endregion
    }
}
