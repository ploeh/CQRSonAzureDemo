using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.Samples.Booking.DomainModel;
using System.Data.Services.Client;

namespace Ploeh.Samples.Booking.Azure
{
    public class AzureReservationRepository : IReservationRepository
    {
        private readonly ReservationContext context;

        public AzureReservationRepository(ReservationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            this.context = context;
        }

        #region IReservationRoot Members

        public void AddReservation(ReservationAcceptedEvent reservation)
        {
            var partitionKey = reservation.Date.Date.ToString("yyyyMMdd");
            var rowKey = reservation.Id.ToString();

            if (this.IsReplay(partitionKey, rowKey))
            {
                return;
            }

            var r = new ReservationTableEntity
            {
                PartitionKey = reservation.Date.Date.ToString("yyyyMMdd"),
                RowKey = reservation.Id.ToString(),
                Date = reservation.Date,
                Name = reservation.Name,
                Email = reservation.Email,
                Quantity = reservation.Quantity
            };

            this.context.AddReservation(r);
            this.context.SaveChanges();
        }

        #endregion

        private bool IsReplay(string partitionKey, string rowKey)
        {
            try
            {
                return this.context.Reservations.Where(x => x.PartitionKey == partitionKey && x.RowKey == rowKey).AsEnumerable().Any();
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
