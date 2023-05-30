using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AppMovies.Models;
using AppMovies.Repositories;
using AppMovies.Repositories.Interfaces;

namespace AppMovies.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IFavoriteRepository _favoriteRepository;
        private readonly IRatingRepository _ratingRepository;
        
        public UserController()
        {
            string connectionString = "Server=tcp:sepproject.database.windows.net,1433;Initial Catalog=SepProject;Persist Security Info=False;User ID=rasapebl;Password=BCb7wcOH;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            _userRepository = new UserRepository(connectionString);
            _favoriteRepository = new FavoriteRepository(connectionString);
            _ratingRepository = new RatingRepository(connectionString);
        }
        
        
        
        public async Task<ActionResult> Details()
        {
            //int id = int.Parse(currentUserId);
            // Retrieve data from cache
            string cachedData = HttpRuntime.Cache.Get("UserId") as string;
            if (cachedData != null && cachedData!="0")
            {
                int userId = int.Parse(cachedData);
                User currentUser = await _userRepository.GetUserByIdAsync(userId);
                if (currentUser != null)
                {
                    ViewBag.CurrentUser = currentUser;
                    List<FavoriteWithMovie> favorites = await _favoriteRepository.GetFavoritesByUserIdAsync(currentUser.UserId);
                    List<RatingWithMovie> ratings = await _ratingRepository.GetRatingsByUserIdAsync(currentUser.UserId);
                    
                    UserWithDetails userWithDetails = new UserWithDetails
                    {
                        User = currentUser,
                        Favorites = favorites,
                        Ratings = ratings
                    };
                    
                    Console.WriteLine("Controller");
                    Console.WriteLine(userWithDetails.Favorites.Count);
                    
                    return View(userWithDetails);
                }
                else
                {
                    return HttpNotFound();
                }
            }
            
            return HttpNotFound();
            
        }
    }
}