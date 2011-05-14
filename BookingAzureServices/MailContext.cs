using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;
using Microsoft.WindowsAzure;
using System.Data.Services.Client;

namespace Ploeh.Samples.Booking.Azure
{
    public class MailContext : TableServiceContext, IRequiresInitialization
    {
        private const string tableName = "MailMessages";

        public MailContext(CloudStorageAccount account)
            : base(account.TableEndpoint.AbsoluteUri, account.Credentials)
        {
        }

        public DataServiceQuery<MailMessageTableEntity> MailMessages
        {
            get { return this.CreateQuery<MailMessageTableEntity>(MailContext.tableName); }
        }

        public void AddMailMessage(MailMessageTableEntity m)
        {
            this.AddObject(MailContext.tableName, m);
        }

        public void Initialize()
        {
            new CloudTableClient(this.BaseUri.ToString(), this.StorageCredentials).CreateTableIfNotExist(MailContext.tableName);
        }
    }
}
