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

namespace MauiFrontend.Platforms.Android
{
    public class MyPrinterDiscoverySession : PrinterDiscoverySession
    {
        const string TAG = "MyPrintService";
        readonly MyPrintService _service;
        readonly BackendPrinter[] _printers;

        public MyPrinterDiscoverySession(MyPrintService service, BackendPrinter[] printers)
        {
            _service = service;
            _printers = printers;
        }

        public override void OnStartPrinterDiscovery(IList<PrinterId> priorityList)
        {
            var printerInfos = new List<PrinterInfo>();

            foreach (var backend in _printers)
            {
                string localId = backend.Id;
                string name = backend.Name;

                // Use the built-in C# helper to get a PrinterId:
                PrinterId printId = _service.GeneratePrinterId(localId);

                // Build capabilities:
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

                // Build PrinterInfo with PrinterStatus.Idle (instead of obsolete StatusIdle):
                var info = new PrinterInfo.Builder(printId, name, PrinterStatus.Idle)
                    .SetCapabilities(caps)
                    .Build();

                printerInfos.Add(info);
                Log.Debug(TAG, $"Added printer to list: {info.Name}");
            }

            AddPrinters(printerInfos);
            Log.Info(TAG, $"Successfully published {_printers.Length} printers");
        }

        public override void OnStopPrinterDiscovery()
        {
            Log.Debug(TAG, "Stopping printer discovery");
            // No call to base.OnStopPrinterDiscovery() because the base is abstract.
        }

        public override void OnValidatePrinters(IList<PrinterId> printerIds)
        {
            Log.Debug(TAG, $"Validating printers: {printerIds.Count}");
            // If any printer is no longer valid, call ReportPrinterStatus(printerId, status).
        }

        public override void OnStartPrinterStateTracking(PrinterId printerId)
        {
            Log.Debug(TAG, $"Starting state tracking for printer: {printerId.LocalId}");
            // You could, e.g., query toner/ink levels here.
        }

        public override void OnStopPrinterStateTracking(PrinterId printerId)
        {
            Log.Debug(TAG, $"Stopping state tracking for printer: {printerId.LocalId}");
            // Stop any ongoing tracking.
        }

        public override void OnDestroy()
        {
            Log.Debug(TAG, "Destroying printer discovery session");
            // Do NOT call base.OnDestroy(); the base is abstract.
        }
    }
}
