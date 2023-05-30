using System.Data.SqlTypes;

namespace AppMovies.Models
{
    public class Vote
    {
        public int VoteId { get; set; }
        public int RatingId { get; set; }
        public int UserId { get; set; }
        public int VoteValue { get; set; }
        public SqlDateTime DateTimeStamp { get; set; }
    }
}