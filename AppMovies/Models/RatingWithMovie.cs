using System.Data.SqlTypes;

namespace AppMovies.Models
{
    public class RatingWithMovie
    {
        public int RatingId { get; set; }
        public int MovieId { get; set; }
        public int UserId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public SqlDateTime DateTimeStamp { get; set; }
        public int Likes { get; set; }
        public int DisLikes { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public string Picture { get; set; }
    }
}