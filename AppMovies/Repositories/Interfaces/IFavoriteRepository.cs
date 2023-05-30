using System.Threading.Tasks;

namespace AppMovies.Repositories.Interfaces
{
    public interface IFavoriteRepository
    {
        Task<int> GetIsFavoriteByUserIdAndMovieAsync(int userId, int movieId);
        Task InsertFavoriteAsync(int userId, int movieId);
        Task DeleteFavoriteAsync(int userId, int movieId);
    }
}