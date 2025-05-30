using Backend.Models;
using Backend.Repositories;

namespace Backend.Services
{
    public class PrinterService
    {
        private readonly PrinterRepository _repository;
        private readonly Backend.WebSocketManager _wsManager;

        public PrinterService(PrinterRepository repository, Backend.WebSocketManager wsManager)
        {
            _repository = repository;
            _wsManager = wsManager;
        }

        public List<Printer> GetAllPrinters() => _repository.GetAllPrinters();

        public async Task<PrintJob> SubmitJobAsync(PrintJob job)
        {
            job.Status = PrintJobStatus.Started;

            await _wsManager.BroadcastAsync($"Job started on printer {job.PrinterId}");

            _ = Task.Run(async () =>
            {
                await Task.Delay(1500);
                job.Status = PrintJobStatus.Processing;
                await _wsManager.BroadcastAsync($"Job being processed on printer {job.PrinterId}");
            });

            // Simulate delay
            _ = Task.Run(async () =>
            {
                await Task.Delay(3000);
                job.Status = PrintJobStatus.Finished;
                await _wsManager.BroadcastAsync($"Job completed on printer {job.PrinterId}");
            });

            return job;
        }
    }
}
