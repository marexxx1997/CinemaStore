namespace CinemaStore.Models
{
    public class TicketGenre
    {
        public int TicketId { get; set; }
        public Ticket Ticket { get; set; }

        public int GenreId { get; set; }
        public Genre Genre { get; set; }
    }
}
