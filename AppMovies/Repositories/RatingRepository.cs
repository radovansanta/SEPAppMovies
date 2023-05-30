using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Threading.Tasks;
using AppMovies.Models;
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

        public async Task<List<RatingWithMovie>> GetRatingsByUserIdAsync(int userId)
        {
            var ratingsWithMovies = new List<RatingWithMovie>();
            
                        using (var connection = new SqlConnection(_connectionString))
                        {
                            await connection.OpenAsync();
            
                            var query = "SELECT RatingID, MovieID, UserID, Rating, Comment, DateTimeStamp, Likes, DisLikes, Title, Year, Picture FROM [MoviesApp].[RatingsWithMovies] WHERE UserID = @userId;";
                            using (var command = new SqlCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("@userId", userId);
                                using (var reader = await command.ExecuteReaderAsync())
                                {
                                    while (await reader.ReadAsync())
                                    {
                                        var imageData = (byte[])reader.GetValue(10);
                                        var base64Image = Convert.ToBase64String(imageData);
                                        var imageUrl = $"data:image/png;base64,{base64Image}";
            
                                        var item = new RatingWithMovie()
                                        {
                                            RatingId = reader.GetInt32(0),
                                            MovieId = reader.GetInt32(1),
                                            UserId = reader.GetInt32(2),
                                            Rating = reader.GetInt32(3),
                                            Comment = reader.GetString(4),
                                            DateTimeStamp = reader.GetSqlDateTime(5),
                                            Likes = reader.GetInt32(6),
                                            DisLikes = reader.GetInt32(7),
                                            Title = reader.GetString(8),
                                            Year = reader.GetInt32(9),
                                            Picture = imageUrl,
                                        };
            
                                        ratingsWithMovies.Add(item);
                                    }
                                }
                            }
                        }
            
                        return ratingsWithMovies;
        }
    }
}