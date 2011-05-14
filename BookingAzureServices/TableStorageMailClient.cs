using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Data.Services.Client;

namespace Ploeh.Samples.Booking.Azure
{
    public class TableStorageMailClient
    {
        private readonly MailContext context;

        public TableStorageMailClient(MailContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            this.context = context;
        }

        public void Send(MailMessage msg)
        {
            var time = DateTime.UtcNow;

            var partitionKey = time.Date.ToString("yyyyMMdd");
            var rowKey = Guid.NewGuid().ToString("N");
            if (this.IsReplay(partitionKey, rowKey))
            {
                return;
            }

            var m = new MailMessageTableEntity
            {
                PartitionKey = partitionKey,
                RowKey = rowKey,
                Body = msg.Body,
                Recipients = msg.To.Select(a => a.ToString()).Aggregate((x, y) => x + "; " + y),
                Sender = msg.From.ToString(),
                Subject = msg.Subject
            };

            this.context.AddMailMessage(m);
            this.context.SaveChanges();
        }

        private bool IsReplay(string partitionKey, string rowKey)
        {
            try
            {
                return this.context.MailMessages.Where(x => x.PartitionKey == partitionKey && x.RowKey == rowKey).AsEnumerable().Any();
            }
            catch (DataServiceQueryException e)
            {
                var ie = e.InnerException as DataServiceClientException;
                if ((ie != null) && (ie.StatusCode == 404))
                {
                    return false;
                }
                throw;
            }
        }
    }
}
