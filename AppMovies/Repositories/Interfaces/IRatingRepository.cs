using System.Threading.Tasks;
using AppMovies.Models;

namespace AppMovies.Repositories.Interfaces
{
    public interface IRatingRepository
    {
        Task InsertRatingAsync(int movieId, int userId, int ratingValue, string comment);
    }
}