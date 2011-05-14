using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;

namespace Ploeh.Samples.Booking.Azure
{
    public class MailMessageTableEntity : TableServiceEntity
    {
        public string Body { get; set; }

        public string Sender { get; set; }

        public string Subject { get; set; }

        public string Recipients { get; set; }
    }
}
