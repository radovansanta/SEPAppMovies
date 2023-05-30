using System.Collections.Generic;
using System.Threading.Tasks;
using AppMovies.Models;

namespace AppMovies.Repositories.Interfaces
{
    public interface IVotesRepository
    {
        Task<int> HasUserLikedRatingAsync(int userId, int ratingId);
        Task UpdateVoteAsync(int userId, int ratingId, int ratingValue);
        Task DeleteVoteAsync(int userId, int ratingId);
        
        Task InsertVoteAsync(int userId, int ratingId, int ratingValue);
        Task<List<Vote>> GetVotesByMovieIdAndUserIdAsync(int userId, int movieId);

    }
}