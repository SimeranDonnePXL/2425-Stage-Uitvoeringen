using Android.Content;
using MFrontend.Models;
using MFrontend.Models;
using MFrontend.Platforms.Android;
using MFrontend.Platforms.Android;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFrontend.Services
{
    public partial class CustomPrintService
    {
        public partial void PrintersToDiscover(List<Printer> printers)
        {
            var backendPrinters = new BackendPrinter[printers.Count];

            for (int i = 0; i < printers.Count; i++)
            {
                var printer = printers[i];

                backendPrinters[i] = new BackendPrinter(printer.Id.ToString(), printer.Name);
            }

            var prefs = Android.App.Application.Context
                  .GetSharedPreferences("print_data", FileCreationMode.Private);

            var json = JsonConvert.SerializeObject(backendPrinters);
            prefs.Edit().PutString("stored_printers", json).Commit();


            //var service = new MyPrintService();
            //service.PrintersToDiscover(backendPrinters);
        }
    }
}
