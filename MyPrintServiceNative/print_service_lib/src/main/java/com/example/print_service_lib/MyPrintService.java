package com.example.print_service_lib;

import android.printservice.PrintJob;
import android.printservice.PrintService;
import android.printservice.PrinterDiscoverySession;
import android.util.Log;

import androidx.annotation.Nullable;

public class MyPrintService extends PrintService {
    public static BackendPrinter[] sharedPrinters = new BackendPrinter[0];
    private static final String TAG = "MyPrintService";

    public static void printersToDiscover(BackendPrinter[] printers){
        sharedPrinters = printers;
    }

    @Nullable
    @Override
    protected PrinterDiscoverySession onCreatePrinterDiscoverySession() {
        Log.d(TAG, "Creating new printer discovery session");
        return new MyPrinterDiscoverySession(this, sharedPrinters);
    }

    @Override
    protected void onRequestCancelPrintJob(PrintJob printJob) {
        Log.d(TAG, "Cancelling print job: " + printJob.getId());
        printJob.cancel();
    }

    @Override
    protected void onPrintJobQueued(PrintJob printJob) {
        Log.d(TAG, "Print job queued: " + printJob.getId());
        printJob.start();
        printJob.complete();
    }
}
