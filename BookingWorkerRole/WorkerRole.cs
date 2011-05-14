using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;
using Castle.Windsor;

namespace Ploeh.Samples.Booking.WorkerRole
{
    public class WorkerRole : RoleEntryPoint
    {
        private IWindsorContainer container;
        private readonly CancellationTokenSource cancellationTokenSource;

        public WorkerRole()
        {
            this.cancellationTokenSource = new CancellationTokenSource();
        }

        public override void Run()
        {
            Trace.WriteLine("BookingWorkerRole entry point called", "Verbose");

            var messageHandler = new AzureQueueMessageProcessor(this.container);
            messageHandler.Run(this.cancellationTokenSource.Token);
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            //DiagnosticMonitor.Start("DiagnosticsConnectionString");

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.
            //RoleEnvironment.Changing += RoleEnvironmentChanging;

            CloudStorageAccount.SetConfigurationSettingPublisher((configName, configSetter) =>
                configSetter(RoleEnvironment.GetConfigurationSettingValue(configName)));

            this.container = new WindsorContainer().Install(new BookingWorkerRoleWindsorInstaller());

            return base.OnStart();
        }

        public override void OnStop()
        {
            this.cancellationTokenSource.Cancel();
            this.container.Dispose();
            base.OnStop();
        }

        //private void RoleEnvironmentChanging(object sender, RoleEnvironmentChangingEventArgs e)
        //{
        //    // If a configuration setting is changing
        //    if (e.Changes.Any(change => change is RoleEnvironmentConfigurationSettingChange))
        //    {
        //        // Set e.Cancel to true to restart this role instance
        //        e.Cancel = true;
        //    }
        //}
    }
}
