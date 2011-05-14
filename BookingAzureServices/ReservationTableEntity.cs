using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;

namespace Ploeh.Samples.Booking.Azure
{
    public class ReservationTableEntity : TableServiceEntity
    {
        public DateTime Date { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public int Quantity { get; set; }
    }
}
