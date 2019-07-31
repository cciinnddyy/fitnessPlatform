using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using System.Net.Sockets;

namespace tcplisten
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);

        public override void Run()
        {
            Trace.TraceInformation("tcplisten is running");

            try
            {
                this.RunAsync(this.cancellationTokenSource.Token).Wait();
            }
            finally
            {
                this.runCompleteEvent.Set();
            }



        }

        public override bool OnStart()
        {
            // 設定同時連線的數目上限
            ServicePointManager.DefaultConnectionLimit = 12;

            // 如需有關處理組態變更的資訊，
            // 請參閱 MSDN 主題，網址為 https://go.microsoft.com/fwlink/?LinkId=166357。

            //var config = DiagnosticMonitor.GetDefaultInitialConfiguration();

            //config.DiagnosticInfrastructureLogs.ScheduledTransferLogLevelFilter = Microsoft.WindowsAzure.Diagnostics.LogLevel.Error;
            //config.DiagnosticInfrastructureLogs.ScheduledTransferPeriod = TimeSpan.FromMinutes(5);

            //DiagnosticMonitor.Start("DiagnosticsConnectionString", config);
            bool result = base.OnStart();

            Trace.TraceInformation("has been started");

            return result;

            
        }

        public override void OnStop()
        {
            Trace.TraceInformation("tcplisten is stopping");

            //this.cancellationTokenSource.Cancel();
            //this.runCompleteEvent.WaitOne();

            base.OnStop();

            Trace.TraceInformation("tcplisten has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: 依您自己的邏輯取代下列項目。

            //TcpListener server = null;
            try
            {
                server server1 = new server();
                server1.ListenToConnection();

            }
            catch { }
            

        }
    }
}
