using CinemaStore.Data.Base;
using CinemaStore.Data.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
namespace CinemaStore.Models
{
    public class NewTicketVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Naziv je obavezan!")]
        [Display(Name = "Naziv")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Opis je obavezan!")]
        [Display(Name = "Opis")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Cijena je obavezna!")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Cijena mora biti veća od 0!")]
        [Display(Name = "Cijena (u KM)")]
        public double Price { get; set; }

        [Required(ErrorMessage = "URL slike je obavezan!")]

        [Display(Name = "URL slike")]
        public string TicketPictureUrl { get; set; }

        [Required(ErrorMessage = "Vrijeme trajanja je obavezno!")]
        [Range(1, int.MaxValue, ErrorMessage = "Vrijeme trajanja mora biti veće od 0!")]
        [Display(Name = "Vrijeme trajanja (u minutama)")]
        public int Duration { get; set; }

        [Required(ErrorMessage = "Žanr je obavezan!")]
        [MinLength(1, ErrorMessage = "Odaberite bar jedan žanr!")]
        [Display(Name = "Odaberite žanrove")]
        public List<int> SelectedGenres { get; set; } = new List<int>();


        [Required(ErrorMessage = "Format prikaza je obavezan!")]
        [MinLength(1, ErrorMessage = "Odaberite bar jedan format prikaza!")]
        [Display(Name = "Odaberite formate prikaza")]
        public List<int> SelectedFormats { get; set; } = new List<int>();


        [Required(ErrorMessage = "Žanr je obavezan!")]
        [Display(Name = "Odaberite žanrove")]
        public IEnumerable<SelectListItem> Genres { get; set; } = new List<SelectListItem>();

        [Required(ErrorMessage = "Format prikaza je obavezan!")]
        [Display(Name = "Odaberite formate prikaza")]

        public IEnumerable<SelectListItem> Formats { get; set; } = new List<SelectListItem>();


        [Required(ErrorMessage = "Ime režisera je obavezno!")]
        [Display(Name = "Ime režisera")]

        public string Producer { get; set; }

        [Required(ErrorMessage = "Ime glumaca je obavezno!")]
        [Display(Name = "Ime glumaca (min. 3)")]
        public string Actors { get; set; }

        [Required(ErrorMessage = "Tip događaja je obavezan!")]

        [Display(Name = "Tip događaja")]
        public int TicketType { get; set; }

    }
}
