using Backend.Models;

namespace Backend.Repositories
{
    public class PrinterRepository
    {
        private readonly List<Printer> _printers = new();

        public PrinterRepository()
        {
            InitializePrinters();
        }

        public List<Printer> GetAllPrinters() => _printers;

        private void InitializePrinters()
        {
            for (int i = 0; i < 30; i++)
            {
                var printer = new Printer
                {
                    Id = i + 1,
                    Name = $"printer {i + 1}"
                };

                _printers.Add(printer);
            }
        }
    }
}
