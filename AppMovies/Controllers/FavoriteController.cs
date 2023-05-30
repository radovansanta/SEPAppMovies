using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AppMovies.Repositories;
using AppMovies.Repositories.Interfaces;

namespace AppMovies.Controllers
{
    public class FavoriteController : Controller
    {
        private readonly IFavoriteRepository _favoriteRepository;
        
        public FavoriteController()
        {
            string connectionString = "Server=tcp:sepproject.database.windows.net,1433;Initial Catalog=SepProject;Persist Security Info=False;User ID=rasapebl;Password=BCb7wcOH;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            _favoriteRepository = new FavoriteRepository(connectionString);
        }
        
        public async Task<ActionResult> AddToFavorites(int movieId)
        {
            // Retrieve data from cache
            string cachedData = HttpRuntime.Cache.Get("UserId") as string;
            if (cachedData != null && cachedData!="0")
            {
                int currentUserId = int.Parse(cachedData);
                await _favoriteRepository.InsertFavoriteAsync(currentUserId,movieId);
            }

            // Return a JSON response indicating the success or any other relevant data
            return RedirectToAction("Details","Home", new { id = movieId });
        }
        
        public async Task<ActionResult> RemoveFromFavorites(int movieId)
        {
            // Retrieve data from cache
            string cachedData = HttpRuntime.Cache.Get("UserId") as string;
            if (cachedData != null && cachedData!="0")
            {
                int currentUserId = int.Parse(cachedData);
                await _favoriteRepository.DeleteFavoriteAsync(currentUserId,movieId);
            }

            // Return a JSON response indicating the success or any other relevant data
            return RedirectToAction("Details","Home", new { id = movieId });
        }
        
        public async Task<ActionResult> RemoveFromFavoritesAccount(int movieId)
        {
            // Retrieve data from cache
            string cachedData = HttpRuntime.Cache.Get("UserId") as string;
            if (cachedData != null && cachedData!="0")
            {
                int currentUserId = int.Parse(cachedData);
                await _favoriteRepository.DeleteFavoriteAsync(currentUserId,movieId);
            }

            // Return a JSON response indicating the success or any other relevant data
            return RedirectToAction("Details","User");
        }

    }
}