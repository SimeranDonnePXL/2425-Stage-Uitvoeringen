using MauiFrontend.Models;
using MauiFrontend.Platforms.Android;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiFrontend.Services
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

            var service = new MyPrintService();
            service.PrintersToDiscover(backendPrinters);
        }
    }
}
