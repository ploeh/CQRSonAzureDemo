using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.Samples.Booking.DomainModel;
using Microsoft.WindowsAzure.StorageClient;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Ploeh.Samples.Booking.Azure
{
    public class AzureChannel : IChannel
    {
        private readonly CloudQueue queue;

        public AzureChannel(CloudQueue queue)
        {
            if (queue == null)
            {
                throw new ArgumentNullException("queue");
            }

            this.queue = queue;
        }

        #region ICommandChannel Members

        public void Send(object command)
        {
            var formatter = new BinaryFormatter();
            using (var s = new MemoryStream())
            {
                formatter.Serialize(s, command);
                var msg = new CloudQueueMessage(s.ToArray());
                this.queue.AddMessage(msg);
            }
        }

        #endregion
    }
}
