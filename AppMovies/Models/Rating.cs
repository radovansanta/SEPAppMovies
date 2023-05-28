using System;
using System.Data.SqlTypes;

namespace AppMovies.Models
{
    public class Rating
    {
        public int RatingId { get; set; }
        public int MovieId { get; set; }
        public int UserId { get; set; }
        public int RatingValue { get; set; }
        public string Comment { get; set; }
        public SqlDateTime DateTimeStamp { get; set; }
        public int Likes { get; set; }
        public int DisLikes { get; set; }
        public string FullName { get; set; }
        public string Photo { get; set; }
    }
}