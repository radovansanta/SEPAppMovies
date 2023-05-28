using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using AppMovies.Models;
using AppMovies.Repositories.Interfaces;

namespace AppMovies.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly string _connectionString;

        public MovieRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // To get all movies
        public async Task<List<Movie>> GetMoviesAsync()
        {
            var movies = new List<Movie>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "SELECT MovieID, Title, Picture, Description, OverallRating, RatingsCount, Year FROM [MoviesApp].[MoviesWithRatings]";
                using (var command = new SqlCommand(query, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var imageData = (byte[])reader.GetValue(2);
                        var base64Image = Convert.ToBase64String(imageData);
                        var imageUrl = $"data:image/png;base64,{base64Image}";

                        var item = new Movie
                        {
                            MovieId = reader.GetInt32(0),
                            Title = reader.GetString(1),
                            Picture = imageUrl,
                            Description = reader.GetString(3),
                            OverallRating = reader.GetInt32(4),
                            RatingsCount = reader.GetInt32(5),
                            Year = reader.GetInt32(6)
                        };

                        movies.Add(item);
                    }
                }
            }

            return movies;
        }
        
        // To filter movies
        public async Task<List<Movie>> GetMoviesBySearchAsync(string searchTerm)
        {
            var movies = new List<Movie>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "SELECT MovieID, Title, Picture, Description, OverallRating, RatingsCount, Year FROM [MoviesApp].[MoviesWithRatings] WHERE Title LIKE @searchTerm";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@searchTerm", $"%{searchTerm}%");
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var imageData = (byte[])reader.GetValue(2);
                            var base64Image = Convert.ToBase64String(imageData);
                            var imageUrl = $"data:image/png;base64,{base64Image}";

                            var item = new Movie
                            {
                                MovieId = reader.GetInt32(0),
                                Title = reader.GetString(1),
                                Picture = imageUrl,
                                Description = reader.GetString(3),
                                OverallRating = reader.GetInt32(4),
                                RatingsCount = reader.GetInt32(5),
                                Year = reader.GetInt32(6)
                            };

                            movies.Add(item);
                        }
                    }
                }
            }

            return movies;
        }
    }
}