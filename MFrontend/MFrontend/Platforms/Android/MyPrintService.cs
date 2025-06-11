using Android.App;
using Android.Content;
using Android.Net.Wifi.Aware;
using Android.PrintServices;
using Android.Util;
using AndroidX.Annotations;
using Newtonsoft.Json;


namespace MFrontend.Platforms.Android
{
    [Service(
        Exported = true,
        Permission = "android.permission.BIND_PRINT_SERVICE"
    )]
    [IntentFilter(new[] { "android.printservice.PrintService" })]
    public class MyPrintService : PrintService
    {
        const string TAG = "MyPrintService";

        public BackendPrinter[] SharedPrinters = new BackendPrinter[0];
        //private MyPrinterDiscoverySession? _discoverySession;

        public MyPrintService()
        {

        }

        public BackendPrinter[] GetCurrentPrinters() => SharedPrinters ?? Array.Empty<BackendPrinter>();


        protected override PrinterDiscoverySession OnCreatePrinterDiscoverySession()
        {
            var session = new MyPrinterDiscoverySession(this, SharedPrinters);
            //_discoverySession = session;
            return session;
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

        //public void PrintersToDiscover(BackendPrinter[] printers)
        //{
        //    SharedPrinters = printers;
        //    Log.Debug(TAG, $"PrintersToDiscover called with {printers.Length} printers");
        //}

        public override void OnCreate()
        {
            base.OnCreate();
            var prefs = ApplicationContext
                .GetSharedPreferences("print_data", FileCreationMode.Private);
            var json = prefs.GetString("stored_printers", "[]");
            SharedPrinters = JsonConvert.DeserializeObject<BackendPrinter[]>(json)
                             ?? Array.Empty<BackendPrinter>();
            Log.Debug("MyPrintService", $"Loaded {SharedPrinters.Length} printers from SharedPreferences");
        }
    }
}
