package com.example.stage_uitvoeringen;

import android.content.Context;
import android.content.SharedPreferences;
import android.printservice.PrintJob;
import android.printservice.PrintService;
import android.printservice.PrinterDiscoverySession;
import android.util.Log;

import com.google.gson.Gson;
import com.google.gson.JsonSyntaxException;

import androidx.annotation.Nullable;

public class MyPrintService extends PrintService {
    public static BackendPrinter[] sharedPrinters = new BackendPrinter[0];
    private static final String TAG = "MyPrintServiceFlutter";

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

    @Override
    public void onCreate() {
        super.onCreate();

        // Retrieve the JSON string from SharedPreferences
        SharedPreferences prefs = getApplicationContext()
                .getSharedPreferences("print_data_flutter", Context.MODE_PRIVATE);
        String json = prefs.getString("stored_printers", "[]");

        // Deserialize JSON into BackendPrinter[]
        try {
            Gson gson = new Gson();
            BackendPrinter[] printersFromPrefs = gson.fromJson(json, BackendPrinter[].class);
            sharedPrinters = (printersFromPrefs != null)
                    ? printersFromPrefs
                    : new BackendPrinter[0];
        } catch (JsonSyntaxException e) {
            Log.e(TAG, "Failed to parse stored_printers JSON", e);
            sharedPrinters = new BackendPrinter[0];
        }

        Log.d(TAG, "Loaded " + sharedPrinters.length + " printers from SharedPreferences");
    }
}