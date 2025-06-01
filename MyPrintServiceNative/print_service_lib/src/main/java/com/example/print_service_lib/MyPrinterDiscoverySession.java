package com.example.print_service_lib;

import android.print.PrintAttributes;
import android.print.PrinterCapabilitiesInfo;
import android.print.PrinterId;
import android.print.PrinterInfo;
import android.printservice.PrinterDiscoverySession;
import android.util.Log;

import androidx.annotation.NonNull;

import java.util.ArrayList;
import java.util.List;

public class MyPrinterDiscoverySession extends PrinterDiscoverySession {
    private static final String TAG = "MyPrintService";
    private MyPrintService service;
    private BackendPrinter[] printers;

    public  MyPrinterDiscoverySession(MyPrintService myPrintService, BackendPrinter[] printers){
        super();
        this.service = myPrintService;
        this.printers = printers;
    }

    @Override
    public void onStartPrinterDiscovery(@NonNull List<PrinterId> list) {
        List<PrinterInfo> printerInfos = new ArrayList<>();

        for (var printer: this.printers) {
            String id = printer.getId();
            String name = printer.getName();

            // Use service to generate PrinterId
            PrinterId pid = service.generatePrinterId(id);

            PrinterCapabilitiesInfo caps = new PrinterCapabilitiesInfo.Builder(pid)
                    .addMediaSize(PrintAttributes.MediaSize.ISO_A4, true)
                    .addResolution(
                            new PrintAttributes.Resolution("R1", "300dpi", 300, 300),
                            true
                    )
                    .setColorModes(
                            PrintAttributes.COLOR_MODE_COLOR,
                            PrintAttributes.COLOR_MODE_COLOR
                    )
                    .build();

            PrinterInfo info = new PrinterInfo.Builder(
                    pid,
                    name,
                    PrinterInfo.STATUS_IDLE
            )
                    .setCapabilities(caps)
                    .build();
            printerInfos.add(info);
            Log.d(TAG, "Added printer to list: " + info.getName());
        }

        addPrinters(printerInfos);
        Log.i(TAG, "Successfully published " + printers.length + " printers");
    }

    @Override
    public void onStopPrinterDiscovery() {
        Log.d(TAG, "Stopping printer discovery");
    }

    @Override
    public void onValidatePrinters(@NonNull List<PrinterId> list) {
        Log.d(TAG, "Validating printers: " + list.size());
    }

    @Override
    public void onStartPrinterStateTracking(@NonNull PrinterId printerId) {
        Log.d(TAG, "Starting state tracking for printer: " + printerId);
    }

    @Override
    public void onStopPrinterStateTracking(@NonNull PrinterId printerId) {
        Log.d(TAG, "Stopping state tracking for printer: " + printerId);
    }

    @Override
    public void onDestroy() {
        Log.d(TAG, "Destroying printer discovery session");
    }
}
