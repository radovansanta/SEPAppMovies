using System.Collections.Generic;

namespace AppMovies.Models
{
    public class MovieWithDetails
    {
        public Movie Movie { get; set; }
        public List<Director> Directors { get; set; }
        public List<Actor> Actors { get; set; }
        public List<Rating> Ratings { get; set; }
        public List<Vote> Votes { get; set; }
    }
}