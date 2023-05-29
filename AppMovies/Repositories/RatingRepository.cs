using System;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Threading.Tasks;
using AppMovies.Repositories.Interfaces;

namespace AppMovies.Repositories
{
    public class RatingRepository : IRatingRepository
    {
        private readonly string _connectionString;

        public RatingRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        
        public async Task InsertRatingAsync(int movieId, int userId, int ratingValue, string comment)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "INSERT INTO [MoviesApp].[Ratings] (MovieID, UserID, Rating, Comment, DateTimeStamp) " +
                            "VALUES (@movieId, @userId, @rating, @comment, @dateTimeStamp)";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@movieId", movieId);
                    command.Parameters.AddWithValue("@userId", userId);
                    command.Parameters.AddWithValue("@rating", ratingValue);
                    command.Parameters.AddWithValue("@comment", comment);
                    command.Parameters.AddWithValue("@dateTimeStamp", DateTime.Now );

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}