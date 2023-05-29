using System.Threading.Tasks;

namespace AppMovies.Repositories.Interfaces
{
    public interface IFavoriteRepository
    {
        Task<int> GetIsFavoriteByUserIdAndMovieAsync(int userId, int movieId);
    }
}