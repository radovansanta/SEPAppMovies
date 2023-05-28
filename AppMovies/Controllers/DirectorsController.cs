using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using AppMovies.Models;
using AppMovies.Repositories;
using AppMovies.Repositories.Interfaces;

namespace AppMovies.Controllers
{
    public class DirectorsController : Controller
    {
        
        private readonly IDirectorsRepository _directorRepository;
        public DirectorsController()
        {
            string connectionString = "Server=tcp:sepproject.database.windows.net,1433;Initial Catalog=SepProject;Persist Security Info=False;User ID=rasapebl;Password=BCb7wcOH;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            _directorRepository = new DirectorRepository(connectionString);
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

            // Pass the search term and filtered results to the view
            ViewBag.SearchTerm = searchTerm;
            return View(data);
        }
    }
}