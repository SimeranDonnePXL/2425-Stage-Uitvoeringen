using Android.Content;
using MFrontend.Platforms.Android;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFrontend.Helpers
{
    public static class PrinterStorageHelper
    {
        private const string PREFS_NAME = "print_data";
        private const string KEY_PRINTERS = "printers";

        public static void SavePrinters(Context context, BackendPrinter[] printers)
        {
            var prefs = context.GetSharedPreferences(PREFS_NAME, FileCreationMode.Private);
            var editor = prefs.Edit();

            var json = JsonConvert.SerializeObject(printers);
            editor.PutString(KEY_PRINTERS, json);
            editor.Commit();
        }

        public static BackendPrinter[] LoadPrinters(Context context)
        {
            var prefs = context.GetSharedPreferences(PREFS_NAME, FileCreationMode.Private);
            var json = prefs.GetString(KEY_PRINTERS, "[]");

            return JsonConvert.DeserializeObject<BackendPrinter[]>(json)
                   ?? Array.Empty<BackendPrinter>();
        }
    }
}
