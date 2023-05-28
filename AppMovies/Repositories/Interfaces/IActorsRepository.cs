using System.Collections.Generic;
using System.Threading.Tasks;
using AppMovies.Models;

namespace AppMovies.Repositories.Interfaces
{
    public interface IActorsRepository
    {
        Task<List<Actor>> GetActorsAsync();
        Task<List<Actor>> GetActorsBySearchAsync(string searchTerm);
        Task<List<Movie>> GetMoviesByActorIdAsync(int actorId);
        Task<Actor> GetActorByIdAsync(int actorId);
    }
}