using System.Collections.Generic;

namespace AppMovies.Models
{
    public class DirectorWithMovies
    {
        public Director Director { get; set; }
        public List<Movie> ExtraItems { get; set; }
    }
}