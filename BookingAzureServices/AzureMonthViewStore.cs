using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.Samples.Booking.WebModel;
using Microsoft.WindowsAzure.StorageClient;

namespace Ploeh.Samples.Booking.Azure
{
    public class AzureMonthViewStore : IMonthViewWriter, IMonthViewReader
    {
        private readonly CloudBlobContainer viewContainer;

        public AzureMonthViewStore(CloudBlobContainer viewContainer)
        {
            if (viewContainer == null)
            {
                throw new ArgumentNullException("viewContainer");
            }

            this.viewContainer = viewContainer;
        }

        #region IMonthViewWriter Members

        public void Disable(DateTime date)
        {
            var viewBlob = this.GetViewBlob(date);
            DateTime[] disabledDates = viewBlob.DownloadItem();
            viewBlob.Upload(disabledDates
                .Union(new[] { date }).ToArray());
        }

        #endregion

        #region IMonthViewReader Members

        public IEnumerable<string> Read(int year, int month)
        {
            DateTime[] disabledDates = 
                this.GetViewBlob(year, month).DownloadItem();
            return (from d in disabledDates
                    select d.ToString("yyyy.MM.dd"));
        }

        #endregion

        private SerializerBlob<DateTime[]> GetViewBlob(DateTime date)
        {
            var viewName = string.Format("view.month.{0}", date.ToString("yyyyMM"));
            return this.GetViewBlob(viewName);
        }

        private SerializerBlob<DateTime[]> GetViewBlob(int year, int month)
        {
            var viewName = string.Format("view.month.{0:D2}{1:D2}", year, month);
            return this.GetViewBlob(viewName);
        }

        private SerializerBlob<DateTime[]> GetViewBlob(string viewName)
        {
            return new SerializerBlob<DateTime[]>(this.viewContainer.GetBlobReference(viewName), () => new DateTime[0]);
        }
    }
}
