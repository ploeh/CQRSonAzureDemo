using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;
using Ploeh.Samples.Booking.DomainModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Ploeh.Samples.Booking.Azure
{
    public class SerializerBlob<T> : CloudBlob
    {
        private readonly Func<T> createNewItem;
        private readonly BinaryFormatter formatter;

        public SerializerBlob(CloudBlob cloudBlob, Func<T> initialValueFactory)
            : base(cloudBlob)
        {
            if (initialValueFactory == null)
            {
                throw new ArgumentNullException("initialValueFactory");
            }

            this.createNewItem = initialValueFactory;
            this.formatter = new BinaryFormatter();
        }

        public T DownloadItem()
        {
            var s = new MemoryStream();
            try
            {
                this.DownloadToStream(s);
                s.Position = 0;

                return (T)this.formatter.Deserialize(s);
            }
            catch (StorageClientException e)
            {
                if (e.ErrorCode != StorageErrorCode.BlobNotFound)
                {
                    throw;
                }

                return this.createNewItem();
            }
            finally
            {
                s.Dispose();
            }
        }

        public void Upload(T item)
        {
            using (var s = new MemoryStream())
            {
                this.formatter.Serialize(s, item);
                s.Position = 0;

                var options = this.CreateBlobRequestOptions();
                this.UploadFromStream(s, options);
            }
        }

        private BlobRequestOptions CreateBlobRequestOptions()
        {
            var etag = this.Properties.ETag;
            var options = new BlobRequestOptions();
            options.AccessCondition = etag == null ?
                AccessCondition.IfNoneMatch("*") :
                AccessCondition.IfMatch(etag);
            return options;
        }
    }
}
