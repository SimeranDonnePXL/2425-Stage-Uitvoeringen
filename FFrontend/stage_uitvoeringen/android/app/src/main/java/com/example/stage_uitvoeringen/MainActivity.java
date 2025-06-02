package com.example.stage_uitvoeringen;

import android.content.Context;
import android.content.SharedPreferences;

import org.json.JSONArray;
import org.json.JSONObject;

import java.util.List;
import java.util.Map;

import io.flutter.embedding.android.FlutterActivity;
import io.flutter.embedding.engine.FlutterEngine;
import io.flutter.plugin.common.MethodChannel;

public class MainActivity extends FlutterActivity {
    private static final String CHANNEL = "stage_uitvoering/custom_print_service";

    @Override
    public void configureFlutterEngine(FlutterEngine flutterEngine) {
        super.configureFlutterEngine(flutterEngine);

        new MethodChannel(flutterEngine.getDartExecutor().getBinaryMessenger(), CHANNEL)
                .setMethodCallHandler((call, result) -> {
                    if ("setPrinters".equals(call.method)) {
                        List<Map<String, Object>> printers = call.argument("printers");

                        if (printers != null) {
                            savePrintersToPreferences(printers);
                            result.success(null);
                        } else {
                            result.error("INVALID_DATA", "No printers received", null);
                        }
                    } else {
                        result.notImplemented();
                    }
                });
    }

    private void savePrintersToPreferences(List<Map<String, Object>> printers) {
        SharedPreferences prefs = getSharedPreferences("print_data_flutter", Context.MODE_PRIVATE);
        SharedPreferences.Editor editor = prefs.edit();

        JSONArray jsonArray = new JSONArray();
        for (Map<String, Object> printer : printers) {
            JSONObject obj = new JSONObject();
            try {
                obj.put("id", printer.get("id"));
                obj.put("name", printer.get("name"));
                jsonArray.put(obj);
            } catch (Exception e) {
                e.printStackTrace();
            }
        }

        editor.putString("stored_printers", jsonArray.toString());
        editor.apply();
    }
}
