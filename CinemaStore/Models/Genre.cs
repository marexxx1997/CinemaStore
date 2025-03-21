namespace CinemaStore.Models
{
    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Many-to-Many relationship with Film
        public ICollection<TicketGenre> TicketGenres { get; set; }
    }
}
