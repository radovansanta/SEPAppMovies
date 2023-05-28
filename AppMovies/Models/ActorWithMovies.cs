using System.Collections.Generic;

namespace AppMovies.Models
{
    public class ActorWithMovies
    {
        public Actor Actor { get; set; }
        public List<Movie> Movies { get; set; }
    }
}