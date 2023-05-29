using System;
using System.Net;
using System.Runtime.Caching;
using System.Security.Claims;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Services.Description;

namespace AppMovies.Controllers
{
    public class AuthenticationController : Controller
    {


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        /*public ActionResult Login()
        {
            FormsAuthentication.SetAuthCookie("username", false);
            Console.WriteLine("Authenticated");
            return RedirectToAction("Index", "Home");
            }
        */
        

        public ActionResult Login(string username, string password)
        {
            // Perform authentication logic here using the provided username and password
            // Example:
            if (username == "admin" && password == "password")
            {
                Console.WriteLine("Authenticated");
                FormsAuthentication.SetAuthCookie(username, false);
                
                var cacheKey = "UserId";
                var yourData = username;
                HttpRuntime.Cache.Insert("UserId", yourData);
                
                return RedirectToAction("Index", "Home");
            }
            
            // Authentication failed, redirect back to the login page or display an error message
            return RedirectToAction("Login");
        }
        
        public ActionResult LogOut()
        {
            Console.WriteLine("Logged out");
            HttpRuntime.Cache.Remove("UserId");
            return RedirectToAction("Index", "Home");
        }
        
    }

}