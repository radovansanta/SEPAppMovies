using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AppMovies.Models;
using AppMovies.Repositories;
using AppMovies.Repositories.Interfaces;

namespace AppMovies.Controllers
{
    public class DirectorsController : Controller
    {
        
        private readonly IDirectorsRepository _directorRepository;
        private readonly IUserRepository _userRepository;
        public DirectorsController()
        {
            string connectionString = "Server=tcp:sepproject.database.windows.net,1433;Initial Catalog=SepProject;Persist Security Info=False;User ID=rasapebl;Password=BCb7wcOH;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            _directorRepository = new DirectorRepository(connectionString);
            _userRepository = new UserRepository(connectionString);
        }
        public async Task<ViewResult> Directors(string searchTerm)
        {
            List<Director> data;
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                data = await _directorRepository.GetDirectorsAsync();
            }
            else
            {
                data = await _directorRepository.GetDirectorsBySearchAsync(searchTerm);
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
            Director director = await _directorRepository.GetDirectorByIdAsync(id);

            if (director == null)
            {
                return HttpNotFound();
            }

            // Retrieve the extra items related to the director
            List<Movie> moviesForDirector = await _directorRepository.GetMoviesByDirectorIdAsync(id);

            // Create a view model to hold both the director and extra items data
            DirectorWithMovies directorWithMovies = new DirectorWithMovies
            {
                Director = director,
                ExtraItems = moviesForDirector
            };

            return View(directorWithMovies);
        }
    }
}