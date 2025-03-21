namespace CinemaStore.Models
{
    public class Format
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Many-to-Many relationship with Film
        public ICollection<TicketFormat> TicketFormats { get; set; }
    }
}
