using Android.App;
using Android.PrintServices;
using Android.Util;
using AndroidX.Annotations;


namespace MauiFrontend.Platforms.Android
{
    [Service(
        Exported = true,
        Permission = "android.permission.BIND_PRINT_SERVICE"
    )]
    [IntentFilter(new[] { "android.printservice.PrintService" })]
    public class MyPrintService : PrintService
    {
        const string TAG = "MyPrintService";

        // This static array is populated from your shared/.NET code.
        public static BackendPrinter[] SharedPrinters = new BackendPrinter[0];

        public MyPrintService()
        {
            // Required parameterless constructor for Android to instantiate the service.
        }

        // Called when Android wants you to create a new discovery session:
        protected override PrinterDiscoverySession OnCreatePrinterDiscoverySession()
        {
            Log.Debug(TAG, "Creating new printer discovery session (C#)");
            return new MyPrinterDiscoverySession(this, SharedPrinters);
        }

        protected override void OnRequestCancelPrintJob(PrintJob printJob)
        {
            Log.Debug(TAG, $"Cancelling print job: {printJob.Id?.ToString()}");
            printJob.Cancel();
        }

        protected override void OnPrintJobQueued(PrintJob printJob)
        {
            Log.Debug(TAG, $"Print job queued: {printJob.Id?.ToString()}");
            printJob.Start();
            printJob.Complete();
        }

        // C# helper to populate SharedPrinters from anywhere in your MAUI code:
        public void PrintersToDiscover(BackendPrinter[] printers)
        {
            SharedPrinters = printers;
            //OnCreatePrinterDiscoverySession();
        }
    }
}
