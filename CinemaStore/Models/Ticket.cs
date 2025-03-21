using CinemaStore.Data.Base;
using CinemaStore.Data.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CinemaStore.Models
{
    public class Ticket:IEntityBase
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }
        public string TicketPictureUrl { get; set; }

        public int Duration { get; set; }

        //public Genre Genre { get; set; }

        public string Producer { get; set; }
        public string Actors { get; set; }

        //public  MovieFormat MovieFormat { get; set; }
        public int TicketType { get; set; }

        public ICollection<TicketGenre> TicketGenres { get; set; }

        public ICollection<TicketFormat> TicketFormats { get; set; }

        public List<TicketFormatShowTime> ShowTimes { get; set; } = new List<TicketFormatShowTime>();

    }
}
