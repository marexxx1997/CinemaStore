using CinemaStore.Data.Base;
using CinemaStore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CinemaStore.Data.Services
{
    public interface IMoviesService:IEntityBaseRepository<Ticket>
    {
        Task<Ticket> GetMovieByIdAsync(int id);

        Task AddNewTicketAsync(NewTicketVM data);
        Task UpdateTicketAsync(NewTicketVM data);
        Task<NewTicketVM> GetTicketGenresAndFormatsAsync();

        Task<ShowTimeVM> GetShowTimeDetailsAsync();

        Task<List<SelectListItem>> GetFormatsByMovieAsync(int movieId);

        Task<List<SelectListItem>> GetGenresByMovieAsync(int movieId);
        Task AddShowTimeAsync(ShowTimeVM showTime);

        Task<List<TicketFormatShowTime>> GetShowTimesByMovieIdAsync(int movieId);

        Task<List<Ticket>> GetAllWithGenresAsync();

        Task<bool> DecreaseAvailableSeatsAsync(int showTimeId, int quantity);
    }
}
