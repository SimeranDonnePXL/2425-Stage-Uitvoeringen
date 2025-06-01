using MauiFrontend.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace MauiFrontend.Services
{
    public class PrintService
    {
        private readonly HttpClient _http;
        private const string BaseUrl = "http://10.0.2.2:5128/api/printer";
        private readonly CustomPrintService _customPrintService;

        public PrintService()
        {
            _http = new HttpClient();
            _customPrintService = new CustomPrintService();
        }

        public async Task<List<Printer>> GetPrintersAsync()
        {
            try
            {
                //var printers = await _http.GetFromJsonAsync<List<Printer>>(BaseUrl);

                var printers = new List<Printer>
                {
                    new Printer
                    {
                        Id = 1,
                        Name = "Fake"
                    }
                };
                
                _customPrintService.PrintersToDiscover(printers);

                return printers;
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
                return new();
            }
        }

        public async Task SubmitPrintJobAsync(int printerId)
        {
            var job = new PrintJob
            {
                PrinterId = printerId,
                DocumentId = printerId
            };

            await _http.PostAsJsonAsync($"{BaseUrl}/print", job);
        }
    }

}
