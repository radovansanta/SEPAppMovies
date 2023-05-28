using System.Collections.Generic;
using System.Threading.Tasks;
using AppMovies.Models;

namespace AppMovies.Repositories.Interfaces
{
    public interface IMovieRepository
    {
        Task<List<Movie>> GetMoviesAsync();
        Task<List<Movie>> GetMoviesBySearchAsync(string searchTerm);
    }
}