using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AppMovies.Repositories;
using AppMovies.Repositories.Interfaces;

namespace AppMovies.Controllers
{
    public class ReviewController : Controller
    {
        
        private readonly IRatingRepository _ratingRepository;

        public ReviewController()
        {
            string connectionString = "Server=tcp:sepproject.database.windows.net,1433;Initial Catalog=SepProject;Persist Security Info=False;User ID=rasapebl;Password=BCb7wcOH;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            _ratingRepository = new RatingRepository(connectionString);
        }
        public async Task<ActionResult> PostReview(int? selectedNumber, string reviewText, int selectedMovie)
        {
            if (selectedNumber == null || reviewText == null)
            {
                return RedirectToAction("Details", "Home", new { id = selectedMovie });
            }
                // Retrieve data from cache
            string cachedData = HttpRuntime.Cache.Get("UserId") as string;
            if (cachedData != null && cachedData!="0")
            {
                    int userId = int.Parse(cachedData);
                    
                    Console.WriteLine(selectedNumber);
                    Console.WriteLine(reviewText);
                    Console.WriteLine(userId);
                    
                    await _ratingRepository.InsertRatingAsync(selectedMovie,userId,selectedNumber.Value,reviewText);
            }

            // Return a JSON response indicating the success or any other relevant data
            return RedirectToAction("Details","Home", new { id = selectedMovie });
        }

    }
}