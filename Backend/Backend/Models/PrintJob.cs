namespace Backend.Models
{
    public class PrintJob
    {
        public int PrinterId { get; set; }
        public int DocumentId { get; set; }
        public PrintJobStatus Status { get; set; } = PrintJobStatus.Pending;
        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
    }
}
