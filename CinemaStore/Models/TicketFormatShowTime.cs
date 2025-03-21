using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaStore.Models
{
    public class TicketFormatShowTime
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int TicketId { get; set; }
        //[ForeignKey("TicketId")]
        public Ticket Ticket { get; set; }

        [Required]
        public int FormatId { get; set; }
        //[ForeignKey("FormatId")]
        public TicketFormat Format { get; set; }

        [Required]
        public DateTime ShowTime { get; set; }

        [Required]
        public int AvailableSeats { get; set; }
    }
}
