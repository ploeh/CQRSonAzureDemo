using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;
using Microsoft.WindowsAzure;
using System.Data.Services.Client;

namespace Ploeh.Samples.Booking.Azure
{
    public class ReservationContext : TableServiceContext, IRequiresInitialization
    {
        private const string tableName = "Reservations";

        public ReservationContext(CloudStorageAccount account)
            : base(account.TableEndpoint.AbsoluteUri, account.Credentials)
        {
        }

        public DataServiceQuery<ReservationTableEntity> Reservations
        {
            get { return this.CreateQuery<ReservationTableEntity>(ReservationContext.tableName); }
        }

        public void AddReservation(ReservationTableEntity reservation)
        {
            this.AddObject(ReservationContext.tableName, reservation);
        }

        public void Initialize()
        {
            new CloudTableClient(this.BaseUri.ToString(), this.StorageCredentials).CreateTableIfNotExist(ReservationContext.tableName);
        }
    }
}
