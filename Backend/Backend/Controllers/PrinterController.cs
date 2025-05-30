using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrinterController : ControllerBase 
    {
        private readonly PrinterService _service;

        public PrinterController(PrinterService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetPrinters()
        {
            var printers = _service.GetAllPrinters();
            return Ok(printers);
        }

        [HttpPost("print")]
        public async Task<IActionResult> Print([FromBody] PrintJob job)
        {
            var result = await _service.SubmitJobAsync(job);
            return Accepted(result);
        }
    }
}
