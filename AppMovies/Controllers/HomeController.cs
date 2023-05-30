using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AppMovies.Functions;
using AppMovies.Models;
using AppMovies.Repositories;
using AppMovies.Repositories.Interfaces;

namespace AppMovies.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IUserRepository _userRepository;
        private readonly IFavoriteRepository _favoriteRepository;
        private readonly IVotesRepository _votesRepository;
        public HomeController()
        {
            string connectionString = "Server=tcp:sepproject.database.windows.net,1433;Initial Catalog=SepProject;Persist Security Info=False;User ID=rasapebl;Password=BCb7wcOH;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            _movieRepository = new MovieRepository(connectionString);
            _userRepository = new UserRepository(connectionString);
            _favoriteRepository = new FavoriteRepository(connectionString);
            _votesRepository = new VotesRepository(connectionString);
        }
        
        public async Task<ViewResult> Index(string searchTerm)
        {
            // Get all movies or filtered movies based on the search term
            List<Movie> data;
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                data = await _movieRepository.GetMoviesAsync();
            }
            else
            {
                data = await _movieRepository.GetMoviesBySearchAsync(searchTerm);
            }

            // Retrieve data from cache
            string cachedData = HttpRuntime.Cache.Get("UserId") as string;
            if (cachedData != null && cachedData!="0")
            {
                int userId = int.Parse(cachedData);
                User currentUser = await _userRepository.GetUserByIdAsync(userId);
                if (currentUser != null)
                {
                    ViewBag.CurrentUser = currentUser;
                }
                else
                {
                    // Handle the case when the user is not found in the repository
                }
            }

            // Pass the search term and filtered results to the view
            ViewBag.SearchTerm = searchTerm;
            return View(data);
        }
        
        public async Task<ActionResult> Details(int id)
        {
            // Retrieve the director from the database based on the provided ID
            Movie movie = await _movieRepository.GetMovieByIdAsync(id);
            MovieWithDetails movieWithDetails = null; 

            if (movie == null)
            {
                return HttpNotFound();
            }
            
            // Retrieve data from cache
            string cachedData = HttpRuntime.Cache.Get("UserId") as string;
            if (cachedData != null && cachedData!="0")
            {
                int userId = int.Parse(cachedData);
                User currentUser = await _userRepository.GetUserByIdAsync(userId);
                if (currentUser != null)
                {
                    //Add functionality to FavoriteMovie by User ID (true/false) and pass it to ViewBag
                    ViewBag.IsFavorite = await _favoriteRepository.GetIsFavoriteByUserIdAndMovieAsync(currentUser.UserId, id);
                    ViewBag.CurrentUser = currentUser;
                    
                    // Retrieve the extra items related to the director
                    List<Director> directorsForMovie = await _movieRepository.GetDirectorsByMovieIdAsync(id);
                    List<Actor> actorsForMovie = await _movieRepository.GetActorsByMovieIdAsync(id);
                    List<Rating> ratingsForMovie = await _movieRepository.GetRatingsByMovieIdAsync(id);
                    List<Vote> votesForMovieByUser = await _votesRepository.GetVotesByMovieIdAndUserIdAsync(currentUser.UserId,id);

                    // Create a view model to hold both the director and extra items data
                    movieWithDetails = new MovieWithDetails
                    {
                        Movie = movie,
                        Directors = directorsForMovie,
                        Actors = actorsForMovie,
                        Ratings = ratingsForMovie,
                        Votes = votesForMovieByUser
                    };

                    return View(movieWithDetails);
                }
                else
                {

                }
            }


            // Retrieve the extra items related to the director
            List<Director> directorsForMovieLogOut = await _movieRepository.GetDirectorsByMovieIdAsync(id);
            List<Actor> actorsForMovieLogOut = await _movieRepository.GetActorsByMovieIdAsync(id);
            List<Rating> ratingsForMovieLogOut = await _movieRepository.GetRatingsByMovieIdAsync(id);

            // Create a view model to hold both the director and extra items data
            movieWithDetails = new MovieWithDetails
            {
                Movie = movie,
                Directors = directorsForMovieLogOut,
                Actors = actorsForMovieLogOut,
                Ratings = ratingsForMovieLogOut,
                Votes = null
            };

            return View(movieWithDetails);
        }

        
    }
}