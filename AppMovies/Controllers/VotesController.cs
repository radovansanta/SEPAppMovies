using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AppMovies.Repositories;
using AppMovies.Repositories.Interfaces;

namespace AppMovies.Controllers
{
    public class VotesController : Controller
    {
        private readonly IVotesRepository _votesRepository;

        public VotesController()
        {
            string connectionString = "Server=tcp:sepproject.database.windows.net,1433;Initial Catalog=SepProject;Persist Security Info=False;User ID=rasapebl;Password=BCb7wcOH;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            _votesRepository = new VotesRepository(connectionString);
        }
        
        public async Task<ActionResult> LikeDislikeRating(int ratingId, int ratingValue, int movieId)
        {
            // Retrieve the current user's ID
            string cachedData = HttpRuntime.Cache.Get("UserId") as string;
            if (cachedData != null && cachedData != "0" && int.TryParse(cachedData, out int currentUserId))
            {
                // Check if the user has already liked the movie
                int hasLiked = await _votesRepository.HasUserLikedRatingAsync(currentUserId, ratingId);
                if (hasLiked==1)
                {
                    Console.WriteLine("User HAS already liked the rating");
                    // Update the existing vote
                    await _votesRepository.UpdateVoteAsync(currentUserId, ratingId, ratingValue);
                }
                else
                {
                    Console.WriteLine("User has NOT liked the rating");
                    // Insert a new vote
                    await _votesRepository.InsertVoteAsync(currentUserId, ratingId, ratingValue);
                }
                
                return RedirectToAction("Details","Home", new { id = movieId });
            }

            // Handle the scenario where the user is not logged in or their ID is not available
            return RedirectToAction("Details","Home", new { id = movieId });
        }



        public async Task<ActionResult> RemoveLike(int ratingId, int movieId)
        {
            // Retrieve the current user's ID
            string cachedData = HttpRuntime.Cache.Get("UserId") as string;
            if (cachedData != null && cachedData != "0" && int.TryParse(cachedData, out int currentUserId))
            {
                await _votesRepository.DeleteVoteAsync(currentUserId,ratingId);
                return RedirectToAction("Details","Home", new { id = movieId });
            }

            // Handle the scenario where the user is not logged in or their ID is not available
            return RedirectToAction("Details","Home", new { id = movieId });
        }
    }
}