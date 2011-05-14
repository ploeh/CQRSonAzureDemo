using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;
using Castle.Windsor;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Ploeh.Samples.Booking.DomainModel;
using System.Diagnostics;

namespace Ploeh.Samples.Booking.WorkerRole
{
    public class AzureQueueMessageProcessor
    {
        private IWindsorContainer container;

        public AzureQueueMessageProcessor(IWindsorContainer container)
        {
            this.container = container;
        }

        public void Run(CancellationToken token)
        {
            var queue = this.container.Resolve<CloudQueue>();

            while (!token.IsCancellationRequested)
            {
                Trace.WriteLine("Polling for message.", "Verbose");

                this.PollForMessage(queue);
            }
        }

        public void PollForMessage(CloudQueue queue)
        {
            var message = queue.GetMessage();
            if (message == null)
            {
                Thread.Sleep(500);
                return;
            }

            try
            {
                this.Handle(message);
                queue.DeleteMessage(message);
            }
            catch (Exception e)
            {
                if (e.IsUnsafeToSuppress())
                {
                    throw;
                }
                Trace.TraceError(e.ToString());
            }
        }

        public void Handle(CloudQueueMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            var formatter = new BinaryFormatter();
            using (var s = new MemoryStream(message.AsBytes))
            {
                dynamic consumable = formatter.Deserialize(s);
                Trace.TraceInformation("Received {0} message.", (object)consumable);

                var consumerType = typeof(IMessageConsumer<>).MakeGenericType(consumable.GetType());
                dynamic consumer = this.container.Resolve(consumerType);
                consumer.Consume(consumable);
            }
        }
    }
}
