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

        // This static array is populated from your shared/.NET code.
        public BackendPrinter[] SharedPrinters = new BackendPrinter[0];
        private MyPrinterDiscoverySession? _discoverySession;

        public MyPrintService()
        {
            // Required parameterless constructor for Android to instantiate the service.
        }

        public BackendPrinter[] GetCurrentPrinters() => SharedPrinters ?? Array.Empty<BackendPrinter>();


        // Called when Android wants you to create a new discovery session:
        protected override PrinterDiscoverySession OnCreatePrinterDiscoverySession()
        {
            var session = new MyPrinterDiscoverySession(this, SharedPrinters);
            _discoverySession = session;
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

        // C# helper to populate SharedPrinters from anywhere in your MAUI code:
        public void PrintersToDiscover(BackendPrinter[] printers)
        {
            SharedPrinters = printers;
            Log.Debug(TAG, $"PrintersToDiscover called with {printers.Length} printers");

            _discoverySession?.UpdatePrinters(printers); // This will now work.
        }

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
