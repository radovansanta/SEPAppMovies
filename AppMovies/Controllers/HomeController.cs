using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppMovies.Functions;
using AppMovies.Models;

namespace AppMovies.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var data = new List<Movie>();
            var converter = new Base64ToImageConverter();

            using (var connection = new SqlConnection("Server=tcp:sepproject.database.windows.net,1433;Initial Catalog=SepProject;Persist Security Info=False;User ID=rasapebl;Password=BCb7wcOH;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"))
            {
                connection.Open();

                var query = "SELECT MovieID, Title, Picture, Description FROM [MoviesApp].[Movies]";
                using (var command = new SqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var imageData = (byte[])reader.GetValue(2);

                        var base64Image = Convert.ToBase64String(imageData);
                        var imageUrl = $"data:image/png;base64,{base64Image}";
                        
                        var item = new Movie
                        {

                            MovieId = reader.GetInt32(0),
                            Title = reader.GetString(1),
                            Picture = imageUrl,
                            Description = reader.GetString(3)
                        };

                        data.Add(item);
                    }
                }
            }

            return View(data);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
        
    }
}