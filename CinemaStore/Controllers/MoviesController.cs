using Microsoft.AspNetCore.Mvc;
using CinemaStore.Data;
using CinemaStore.Data.Services;
using CinemaStore.Models;
using System.Linq;
using CinemaStore.Data.Enums;
using PagedList;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CinemaStore.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IMoviesService _service;

        public MoviesController(IMoviesService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {

            var ticketGenresAndFormatsData = await _service.GetTicketGenresAndFormatsAsync();
            var allGenres = ticketGenresAndFormatsData.Genres.Select(g => new SelectListItem
            {
                Value = g.Value,
                Text = g.Text
            }).ToList();

            ViewData["Genres"] = allGenres;

            var data = await _service.GetAllAsync();
            var moviedata = data.Where(x => x.TicketType == 1);

            return View(moviedata);
        }

        public async Task<IActionResult> Shows()
        {

            var ticketGenresAndFormatsData = await _service.GetTicketGenresAndFormatsAsync();
            var allGenres = ticketGenresAndFormatsData.Genres.Select(g => new SelectListItem
            {
                Value = g.Value,
                Text = g.Text
            }).ToList();

            ViewData["Genres"] = allGenres;

            var data = await _service.GetAllAsync();
            var moviedata = data.Where(x => x.TicketType == 2);

            return View("Index", moviedata);
        }

        public async Task<IActionResult> Create()
        {

            var ticketGenresAndFormatsData = await _service.GetTicketGenresAndFormatsAsync();
            return View(ticketGenresAndFormatsData);
        }

        public async Task<IActionResult> AddShowTimeDate()
        {
            var showTimeDetailsData = await _service.GetShowTimeDetailsAsync();
            return View(showTimeDetailsData);

        }

        //[HttpPost]
        //public async  Task<IActionResult> AddShowTimeDate(ShowTimeVM showTime)
        //{
        //    if(!ModelState.IsValid)
        //    {
        //        var ticketGenresAndFormatsData = await _service.GetTicketGenresAndFormatsAsync();

        //        showTime.Formats = ticketGenresAndFormatsData.Formats.Select(f => new SelectListItem
        //        {
        //            Value = f.Value,
        //            Text = f.Text
        //        }).ToList();

        //        return View(showTime);
        //    }
        //    return View();
        //}

        //public async Task<IActionResult> AddShowTimeDate() 
        //{
        //    var ticketGenresAndFormatsData = await _service.GetTicketGenresAndFormatsAsync();

        //    var model = new NewTicketVM
        //    {
        //        Genres = ticketGenresAndFormatsData.Genres.Select(g => new SelectListItem
        //        {
        //            Value = g.Value,
        //            Text = g.Text
        //        }).ToList()
        //    };

        //    return View(model);
        //}


        [HttpPost]
        public async Task<IActionResult> Create(NewTicketVM ticket)
        {
            if (!ModelState.IsValid)
            {
                var ticketGenresAndFormatsData = await _service.GetTicketGenresAndFormatsAsync();
                ticket.Genres = ticketGenresAndFormatsData.Genres.Select(g => new SelectListItem
                {
                    Value = g.Value,
                    Text = g.Text
                }).ToList();

                ticket.Formats = ticketGenresAndFormatsData.Formats.Select(f => new SelectListItem
                {
                    Value = f.Value,
                    Text = f.Text
                }).ToList();

                return View(ticket);
            }
            await _service.AddNewTicketAsync(ticket);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<JsonResult> GetFormatsByMovie(int movieId)
        {
            var formats = await _service.GetFormatsByMovieAsync(movieId);
            return Json(formats);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewShowTimeDate(ShowTimeVM showTime)
        {
            if (!ModelState.IsValid)
            {
                var showTimeDetailsData = await _service.GetShowTimeDetailsAsync();
                showTime.Movies = showTimeDetailsData.Movies;
                showTime.Formats = showTimeDetailsData.Formats;
                return View("AddShowTimeDate", showTime);
            }

            Console.WriteLine($">>> Movie ID: {showTime.SelectedMovieId}");
            Console.WriteLine($">>> Format ID: {showTime.SelectedFormatId}");
            Console.WriteLine($">>> Show Time Date: {showTime.ShowTimeDate}");
            Console.WriteLine($">>> Available Seats: {showTime.AvailableSeats}");

            await _service.AddShowTimeAsync(showTime);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int id)
        {
            var movieDetails = await _service.GetMovieByIdAsync(id);
            var formats = await _service.GetFormatsByMovieAsync(id);
            var genres = await _service.GetGenresByMovieAsync(id);
            var ticketGenresAndFormatsData = await _service.GetTicketGenresAndFormatsAsync();
            var allGenres = ticketGenresAndFormatsData.Genres.Select(g => new SelectListItem
            {
                Value = g.Value,
                Text = g.Text
            }).ToList();

            var allFormats = ticketGenresAndFormatsData.Formats.Select(f => new SelectListItem
            {
                Value = f.Value,
                Text = f.Text
            }).ToList();
            if (movieDetails == null) return View("NotFound");
            var selectedGenreIds = genres.Select(g => int.Parse(g.Value)).ToList();
            var selectedFormatsIds = formats.Select(f => int.Parse(f.Value)).ToList();

            var response = new NewTicketVM()
            {
                Name = movieDetails.Name,
                Description = movieDetails.Description,
                Producer = movieDetails.Producer,
                Actors = movieDetails.Actors,
                Genres = allGenres,
                Formats = allFormats,
                SelectedGenres = selectedGenreIds,
                SelectedFormats = selectedFormatsIds,
                Duration = movieDetails.Duration,
                TicketPictureUrl = movieDetails.TicketPictureUrl,
                Price = movieDetails.Price,
                TicketType = movieDetails.TicketType,

            };

            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, NewTicketVM ticket)
        {
            if (id != ticket.Id) return View("NotFound");
            if (!ModelState.IsValid)
            {
                var ticketGenresAndFormatsData = await _service.GetTicketGenresAndFormatsAsync();
                ticket.Genres = ticketGenresAndFormatsData.Genres.Select(g => new SelectListItem
                {
                    Value = g.Value,
                    Text = g.Text
                }).ToList();

                ticket.Formats = ticketGenresAndFormatsData.Formats.Select(f => new SelectListItem
                {
                    Value = f.Value,
                    Text = f.Text
                }).ToList();

                return View(ticket);
            }
            await _service.UpdateTicketAsync(ticket);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var ticketDetail = await _service.GetMovieByIdAsync(id);
            var genres = await _service.GetGenresByMovieAsync(id);
            var selectedGenreIds = genres.Select(g => int.Parse(g.Value)).ToList();
            var formats = await _service.GetFormatsByMovieAsync(id);
            var showTimes = await _service.GetShowTimesByMovieIdAsync(id);

            ViewData["SelectedGenreIds"] = selectedGenreIds;
            ViewData["MovieGenres"] = genres;
            ViewData["MovieFormats"] = formats;
            ViewData["ShowTimes"] = showTimes;

            return View(ticketDetail);
        }

        public async Task<IActionResult> FilterByGenre(int? genreId)
        {

            var moviedata = await _service.GetAllWithGenresAsync();
            if (moviedata == null) return PartialView("_MoviesList", new List<Ticket>());

            //var moviedata = data.Where(x => x.TicketType == 1).ToList();
            if (genreId.HasValue)
            {
                moviedata = moviedata.Where(m => m.TicketGenres.Any(g => g.GenreId == genreId.Value)).ToList();
            }

            return PartialView("_MoviesList", moviedata);

        }


        public async Task<IActionResult> FilterByDate(DateTime? selectedDate)
        {
            var moviedata = await _service.GetAllWithGenresAsync();
            if (moviedata == null) return PartialView("_MoviesList", new List<Ticket>());

            if (selectedDate.HasValue)
            {
                moviedata = moviedata
                    .Where(m => m.ShowTimes.Any(st => st.ShowTime.Date == selectedDate.Value.Date))
                    .ToList();
            }

            return PartialView("_MoviesList", moviedata);
        }

        public async Task<IActionResult> Search(string searchString)
        {
            var data = await _service.GetAllAsync();
            if (!string.IsNullOrEmpty(searchString))
            {
                var filteredResult = data.Where(n => n.Name.ToUpper().Contains(searchString.ToUpper())).ToList();
                return View("Index", filteredResult);
            }
            else
            {
                return View("Index", data);
            }
        }

    }
}
