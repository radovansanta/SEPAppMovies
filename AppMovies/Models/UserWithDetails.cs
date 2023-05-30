using System.Collections.Generic;

namespace AppMovies.Models
{
    public class UserWithDetails
    {
        public User User { get; set; }
        public List<FavoriteWithMovie> Favorites { get; set; }
        public List<RatingWithMovie> Ratings { get; set; }
    }
}