using System.Collections.Generic;
using System.Threading.Tasks;
using AppMovies.Models;

namespace AppMovies.Repositories.Interfaces
{
    public interface IMovieRepository
    {
        Task<List<Movie>> GetMoviesAsync();
        Task<List<Movie>> GetMoviesBySearchAsync(string searchTerm);
        Task<List<Director>> GetDirectorsByMovieIdAsync(int movieId);
        Task<List<Actor>> GetActorsByMovieIdAsync(int movieId);
        Task<List<Rating>> GetRatingsByMovieIdAsync(int movieId);
        Task<Movie> GetMovieByIdAsync(int movieId);
    }
}