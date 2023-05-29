using System;
using System.Data.SqlClient;
using System.IO;
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
            string connectionString =
                "Server=tcp:sepproject.database.windows.net,1433;Initial Catalog=SepProject;Persist Security Info=False;User ID=rasapebl;Password=BCb7wcOH;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            _userRepository = new UserRepository(connectionString);
        }

        public async Task<ActionResult> Login(string username, string password)
        {
            // Perform authentication logic here using the provided username and password
            var userId = await _userRepository.AuthenticateUser(username, password);

            if (userId != 0)
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


        public async Task<ActionResult> SignUp(string firstName, string lastName, string email, string password, HttpPostedFileBase profilePhoto)
        {
            Console.WriteLine("Picture is");
            Console.WriteLine(profilePhoto);

            if (profilePhoto != null && profilePhoto.ContentLength > 0)
            {
                byte[] photoBytes;
                using (BinaryReader reader = new BinaryReader(profilePhoto.InputStream))
                {
                    photoBytes = reader.ReadBytes(profilePhoto.ContentLength);
                }

                // Call a method to store the photoBytes in the database
                Console.WriteLine("Encoded");
                Console.WriteLine(photoBytes);
                await _userRepository.InsertUserAsync(firstName,lastName,email,password,photoBytes);
            }

            return RedirectToAction("Index", "Home");
        }

    }
}