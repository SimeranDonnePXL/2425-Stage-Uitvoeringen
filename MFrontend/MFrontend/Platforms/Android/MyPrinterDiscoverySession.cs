using Android.Print;
using Android.PrintServices;
using Android.Runtime;
using Android.Util;
using Java.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFrontend.Platforms.Android
{
    public class MyPrinterDiscoverySession : PrinterDiscoverySession
    {
        const string TAG = "MyPrintService";
        private readonly MyPrintService _service;
        private BackendPrinter[] _printers;

        public MyPrinterDiscoverySession(MyPrintService service, BackendPrinter[] printers)
        {
            _service = service;
            _printers = printers;
            Log.Debug(TAG, $"Created discovery session with {printers?.Length ?? 0} printers");
        }

        public override void OnStartPrinterDiscovery(IList<PrinterId> priorityList)
        {
            Log.Debug(TAG, "Starting printer discovery");

            try{
                
                if (_printers == null || _printers.Length == 0)
                {
                    Log.Error(TAG, "No printers available for discovery!");
                    return;
                }

                var printerInfos = new List<PrinterInfo>();

                foreach (var backend in _printers)
                {
                        string localId = backend.Id;
                        string name = backend.Name;

                        Log.Debug(TAG, $"Processing printer: {name} (ID: {localId})");

                        PrinterId printId = _service.GeneratePrinterId(localId);

                        PrinterCapabilitiesInfo caps = new PrinterCapabilitiesInfo.Builder(printId)
                            .AddMediaSize(PrintAttributes.MediaSize.IsoA4, true)
                            .AddResolution(
                                    new PrintAttributes.Resolution("R1", "300dpi", 300, 300),
                                    true
                            )
                            .SetColorModes(
                                (int)PrintColorMode.Color,
                                (int)PrintColorMode.Color
                            )
                            .Build();

                        var info = new PrinterInfo.Builder(printId, name, PrinterStatus.Idle)
                            .SetCapabilities(caps)
                            .Build();

                        printerInfos.Add(info);
                        Log.Debug(TAG, $"Successfully added printer to list: {info.Name}");
                }

                AddPrinters(printerInfos);
                Log.Info(TAG, $"Successfully published {printerInfos.Count} printers");
            }
            catch (Exception ex)
            {
                Log.Error(TAG, $"Error publishing printers: {ex}");
            }
        }

        public override void OnStopPrinterDiscovery()
        {
            Log.Debug(TAG, "Stopping printer discovery");
        }

        public override void OnValidatePrinters(IList<PrinterId> printerIds)
        {
            Log.Debug(TAG, $"Validating printers: {printerIds.Count}");
        }

        public override void OnStartPrinterStateTracking(PrinterId printerId)
        {
            Log.Debug(TAG, $"Starting state tracking for printer: {printerId.LocalId}");
        }

        public override void OnStopPrinterStateTracking(PrinterId printerId)
        {
            Log.Debug(TAG, $"Stopping state tracking for printer: {printerId.LocalId}");
        }

        public override void OnDestroy()
        {
            Log.Debug(TAG, "Destroying printer discovery session");
        }
    }
}
