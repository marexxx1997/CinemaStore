namespace CinemaStore.Models
{
    public class TicketFormat
    {
        public int TicketId { get; set; }
        public Ticket Ticket { get; set; }

        public int FormatId { get; set; }
        public Format Format { get; set; }

        public ICollection<TicketFormatShowTime> ShowTimes { get; set; } = new List<TicketFormatShowTime>();

    }
}