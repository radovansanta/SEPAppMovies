using System;
using System.Net;
using System.Runtime.Caching;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Services.Description;
using AppMovies.Repositories;
using AppMovies.Repositories.Interfaces;

namespace AppMovies.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IUserRepository _userRepository;
        public AuthenticationController()
        {
            string connectionString = "Server=tcp:sepproject.database.windows.net,1433;Initial Catalog=SepProject;Persist Security Info=False;User ID=rasapebl;Password=BCb7wcOH;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            _userRepository = new UserRepository(connectionString);
        }
        
        public async Task<ActionResult> Login(string username, string password)
        {
            // Perform authentication logic here using the provided username and password
            var userId = await _userRepository.AuthenticateUser(username,password);

            if (userId!=0)
            {
                Console.WriteLine("Authenticated");
                var cacheKey = "UserId";
                var yourData = userId.ToString();
                HttpRuntime.Cache.Insert(cacheKey, yourData);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            
        }
        
        public ActionResult LogOut()
        {
            Console.WriteLine("Logged out");
            HttpRuntime.Cache.Remove("UserId");
            return RedirectToAction("Index", "Home");
        }
        
    }

}