using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AppMovies.Models;
using AppMovies.Repositories;
using AppMovies.Repositories.Interfaces;

namespace AppMovies.Controllers
{
    
    
    public class ActorsController : Controller
    {
        private readonly IActorsRepository _actorsRepository;
        private readonly IUserRepository _userRepository;
        
        public ActorsController()
        {
            string connectionString = "Server=tcp:sepproject.database.windows.net,1433;Initial Catalog=SepProject;Persist Security Info=False;User ID=rasapebl;Password=BCb7wcOH;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            _actorsRepository = new ActorRepository(connectionString);
            _userRepository = new UserRepository(connectionString);
        }
        
        public async Task<ViewResult> Actors(string searchTerm)
        {
            List<Actor> data;
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                data = await _actorsRepository.GetActorsAsync();
            }
            else
            {
                data = await _actorsRepository.GetActorsBySearchAsync(searchTerm);
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
            Actor actor = await _actorsRepository.GetActorByIdAsync(id);

            if (actor == null)
            {
                return HttpNotFound();
            }

            // Retrieve the extra items related to the director
            List<Movie> moviesForActor = await _actorsRepository.GetMoviesByActorIdAsync(id);

            // Create a view model to hold both the director and extra items data
            ActorWithMovies actorWithMovies = new ActorWithMovies
            {
                Actor = actor,
                Movies = moviesForActor
            };

            return View(actorWithMovies);
        }
        
    }
}