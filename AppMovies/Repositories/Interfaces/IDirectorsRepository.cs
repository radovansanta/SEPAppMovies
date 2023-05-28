using System.Collections.Generic;
using System.Threading.Tasks;
using AppMovies.Models;

namespace AppMovies.Repositories.Interfaces
{
    public interface IDirectorsRepository
    {
        Task<List<Director>> GetDirectorsAsync();
        Task<List<Director>> GetDirectorsBySearchAsync(string searchTerm);
    }
}