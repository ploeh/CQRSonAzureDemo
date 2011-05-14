using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.Samples.Booking.DomainModel;
using Microsoft.WindowsAzure.StorageClient;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Ploeh.Samples.Booking.WebModel;

namespace Ploeh.Samples.Booking.Azure
{
    public class CapacityGuard : IMessageConsumer<MakeReservationCommand>, IDayViewReader
    {
        private readonly CloudBlobContainer capacityContainer;
        private readonly IChannel channel;

        public CapacityGuard(CloudBlobContainer capacityContainer, IChannel channel)
        {
            if (capacityContainer == null)
            {
                throw new ArgumentNullException("capacityContainer");
            }
            if (channel == null)
            {
                throw new ArgumentNullException("channel");
            }

            this.capacityContainer = capacityContainer;
            this.channel = channel;
        }

        public bool HasCapacity(MakeReservationCommand reservation)
        {
            return this.GetCapacityBlob(reservation)
                .DownloadItem()
                .CanReserve(reservation.Quantity, reservation.Id);
        }

        #region IMessageConsumer<MakeReservationCommand> Members

        public void Consume(MakeReservationCommand message)
        {
            var blob = this.GetCapacityBlob(message);
            var originalCapacity = blob.DownloadItem();

            var newCapacity = originalCapacity.Reserve(
                message.Quantity, message.Id);

            if (!newCapacity.Equals(originalCapacity))
            {
                blob.Upload(newCapacity);
                if (newCapacity.Remaining <= 0)
                {
                    var e = new SoldOutEvent(message.Date);
                    this.channel.Send(e);
                }
            }
        }

        #endregion

        #region IDayViewReader Members

        public int GetRemainingCapacity(DateTime date)
        {
            return this.GetCapacityBlob(date).DownloadItem().Remaining;
        }

        #endregion

        private SerializerBlob<Capacity> GetCapacityBlob(DateTime date)
        {
            var capacityName = CapacityGuard.GetCapacityName(date);
            return new SerializerBlob<Capacity>(this.capacityContainer.GetBlobReference(capacityName), () => new Capacity(10));
        }

        private SerializerBlob<Capacity> GetCapacityBlob(MakeReservationCommand reservation)
        {
            return this.GetCapacityBlob(reservation.Date);
        }

        private static string GetCapacityName(DateTime date)
        {
            return string.Format("date.{0}", date.ToString("yyyyMMdd"));
        }
    }
}
