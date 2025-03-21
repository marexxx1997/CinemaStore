using Microsoft.EntityFrameworkCore;
using CinemaStore.Data.Base;
using CinemaStore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;

namespace CinemaStore.Data.Services
{
    public class MoviesService:EntityBaseRepository<Ticket>, IMoviesService
    {
        private readonly AppDbContext _context;

        public MoviesService (AppDbContext context):base(context)
        {
            _context = context;
        }

        public async Task<Ticket> GetMovieByIdAsync(int id)
        {
            var movieDetails = _context.Tickets.FirstOrDefaultAsync(n => n.Id == id);
            return await movieDetails;
        }

        public async Task AddNewTicketAsync(NewTicketVM data)
        {
            var newTicket = new Ticket()
            {
                Name = data.Name,
                Description = data.Description,
                Producer = data.Producer,
                Actors = data.Actors,
                TicketGenres = data.SelectedGenres.Select(g => new TicketGenre { GenreId = g }).ToList(),
                TicketFormats = data.SelectedFormats.Select(f => new TicketFormat { FormatId = f }).ToList(),
                Duration = data.Duration,
                TicketPictureUrl = data.TicketPictureUrl,
                Price = data.Price,
                TicketType = data.TicketType
            };
            await _context.Tickets.AddAsync(newTicket);
            await _context.SaveChangesAsync();

        }

        public async Task UpdateTicketAsync(NewTicketVM data)
        {
            var dbTicket = await _context.Tickets.FirstOrDefaultAsync(n => n.Id == data.Id);
            if (dbTicket != null)
            {

                dbTicket.Name = data.Name;
                dbTicket.Description = data.Description;
                dbTicket.Producer = data.Producer;
                dbTicket.Actors = data.Actors;
                //dbTicket.TicketGenres = data.SelectedGenres.Select(g => new TicketGenre { GenreId = g }).ToList();
                //dbTicket.TicketFormats = data.SelectedFormats.Select(f => new TicketFormat { FormatId = f }).ToList();
                dbTicket.Duration = data.Duration;
                dbTicket.TicketPictureUrl = data.TicketPictureUrl;
                dbTicket.Price = data.Price;
                dbTicket.TicketType = data.TicketType;

                var ticketGenres = new List<TicketGenre>();
                foreach (var genreId in data.SelectedGenres)
                {
                    var existingGenre = await _context.TicketGenres
                        .FirstOrDefaultAsync(g => g.TicketId == dbTicket.Id && g.GenreId == genreId);
                    if (existingGenre == null)
                    {
                        ticketGenres.Add(new TicketGenre { GenreId = genreId });
                    }
                }

                var ticketFormats = new List<TicketFormat>();
                foreach (var formatId in data.SelectedFormats)
                {
                    var existingFormat = await _context.TicketFormats
                        .FirstOrDefaultAsync(f => f.TicketId == dbTicket.Id && f.FormatId == formatId);
                    if (existingFormat == null)
                    {
                        ticketFormats.Add(new TicketFormat { FormatId = formatId });
                    }
                }

                //dbTicket.TicketGenres = ticketGenres;
                //dbTicket.TicketFormats = ticketFormats;

                await _context.SaveChangesAsync();
            }

        }

        public async Task<NewTicketVM> GetTicketGenresAndFormatsAsync()
        {
            var genres = await _context.Genres.Select(g => new SelectListItem
            {
                Value = g.Id.ToString(),
                Text = g.Name
            }).ToListAsync();

            var formats = await _context.Formats.Select(f => new SelectListItem
            {
                Value = f.Id.ToString(),
                Text = f.Name
            }).ToListAsync();

            return new NewTicketVM
            {
                Genres = genres,
                Formats = formats
            };
        }

        public async Task<ShowTimeVM> GetShowTimeDetailsAsync()

        {
            var movies = await _context.Tickets
        .Select(t => new SelectListItem { Value = t.Id.ToString(), Text = t.Name })
        .ToListAsync();

            var formats = await _context.Formats.Select(f => new SelectListItem
            {
                Value = f.Id.ToString(),
                Text = f.Name
            }).ToListAsync();

            return new ShowTimeVM
            {
                Movies = movies,
                Formats = formats
            };
        }

        public async Task<List<SelectListItem>> GetFormatsByMovieAsync(int movieId)
        {
            Console.WriteLine($">>> Proveravam formate za movieId: {movieId}");

            var formats = await _context.TicketFormats
                .Where(tf => tf.TicketId == movieId)
                .Select(tf => new SelectListItem
                {
                    Value = tf.FormatId.ToString(),
                    Text = tf.Format.Name
                })
                .ToListAsync();

            Console.WriteLine($">>> Broj pronađenih formata: {formats.Count}");

            foreach (var format in formats)
            {
                Console.WriteLine($">>> Format ID: {format.Value}, Naziv: {format.Text}");
            }

            return formats;
        }

        public async Task<List<SelectListItem>> GetGenresByMovieAsync(int movieId)
        {
            //Console.WriteLine($">>> Proveravam formate za movieId: {movieId}");

            var genres = await _context.TicketGenres
                .Where(tf => tf.TicketId == movieId)
                .Select(tf => new SelectListItem
                {
                    Value = tf.GenreId.ToString(),
                    Text = tf.Genre.Name
                })
                .ToListAsync();

            //Console.WriteLine($">>> Broj pronađenih formata: {formats.Count}");

            //foreach (var format in formats)
            //{
            //    Console.WriteLine($">>> Format ID: {format.Value}, Naziv: {format.Text}");
            //}

            return genres;
        }

        public async Task AddShowTimeAsync(ShowTimeVM showTime)
        {
            var newShowTime = new TicketFormatShowTime
            {
                TicketId = showTime.SelectedMovieId,
                FormatId = showTime.SelectedFormatId,
                ShowTime = showTime.ShowTimeDate,
                AvailableSeats = showTime.AvailableSeats
            };

            await _context.TicketFormatShowTimes.AddAsync(newShowTime);
            await _context.SaveChangesAsync();
        }

        public async Task<List<TicketFormatShowTime>> GetShowTimesByMovieIdAsync(int movieId)
        {
            return await _context.TicketFormatShowTimes
                .Where(t => t.TicketId == movieId)
                .ToListAsync();
        }

        //public async Task<TicketFormatShowTime> GetShowTimeByMovieAndFormatAsync(int movieId, int formatId)
        //{
        //    return await _context.TicketFormatShowTimes
        //        .Include(t => t.Format)
        //        .Where(t => t.TicketId == movieId && t.FormatId == formatId)
        //        .FirstOrDefaultAsync();
        //}
        
        public async Task<bool> DecreaseAvailableSeatsAsync(int showTimeId, int quantity)
        {
            var showTime = await _context.TicketFormatShowTimes.FirstOrDefaultAsync(st => st.Id == showTimeId);

            if (showTime == null)
            {
                Console.WriteLine($">>> Greška: ShowTime sa ID {showTimeId} ne postoji u bazi!");
                return false;
            }

            Console.WriteLine($">>> Pronadjen ShowTime ID: {showTime.Id}, Trenutno sedišta: {showTime.AvailableSeats}");

            if (showTime.AvailableSeats < quantity)
            {
                Console.WriteLine($">>> Nema dovoljno sedišta! Potrebno: {quantity}, Dostupno: {showTime.AvailableSeats}");
                return false;
            }

            if (showTime != null && showTime.AvailableSeats >= quantity)
            {
                Console.WriteLine($">>> Pre umanjenja: {showTime.AvailableSeats} sedišta dostupno");

                showTime.AvailableSeats -= quantity;
                await _context.SaveChangesAsync();

                Console.WriteLine($">>> Posle umanjenja: {showTime.AvailableSeats} sedišta dostupno");

                return true;
            }

            Console.WriteLine($">>> Nema dovoljno sedišta za umanjenje!");

            return false;

        }

        public async Task<List<Ticket>> GetAllWithGenresAsync()
        {
            return await _context.Tickets
                .Include(t => t.TicketGenres)
                .Include(st => st.ShowTimes)
                .ToListAsync();
        }
    }
}
