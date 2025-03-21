using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CinemaStore.Models
{
    public class ShowTimeVM
    {

        public ShowTimeVM()
        {
            ShowTimeDate = DateTime.Now;
        }

        [Required(ErrorMessage = "Morate odabrati film!")]
        public int SelectedMovieId { get; set; } 
        public List<SelectListItem> Movies { get; set; } = new List<SelectListItem>();

        public IEnumerable<SelectListItem> Formats { get; set; } = new List<SelectListItem>();

        [Required(ErrorMessage = "Format prikaza je obavezan!")]
        public int SelectedFormatId { get; set; }

        public List<int> SelectedFormats { get; set; } = new List<int>();

        [Required]
        [Display(Name = "Odaberite datum i vrijeme prikazivanja:")]
        public DateTime ShowTimeDate { get; set; }


        [Required]
        [Display(Name = "Odaberite broj raspoloživih karata:")]
        [Range(1, 40, ErrorMessage = "Broj mesta mora biti između 1 i 40!")]
        public int AvailableSeats { get; set; }

    }
}
