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

        // To get directors by movieID
        public async Task<List<Director>> GetDirectorsByMovieIdAsync(int movieId)
        {
            var directors = new List<Director>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "SELECT DirectorID, MovieID, FisrtName, LastName, Age, NumberOFMovies, Description, Photo FROM [MoviesApp].[DirectorsWithMovies] WHERE MovieID = @movieId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@movieId", movieId);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var imageData = (byte[])reader.GetValue(7);
                            var base64Image = Convert.ToBase64String(imageData);
                            var imageUrl = $"data:image/png;base64,{base64Image}";

                            var item = new Director
                            {
                                DirectorId = reader.GetInt32(0),
                                FirstName = reader.GetString(2),
                                LastName = reader.GetString(3),
                                Age = reader.GetInt32(4),
                                NumberOfMovies = reader.GetInt32(5),
                                Description = reader.GetString(6),
                                Photo = imageUrl
                            };

                            directors.Add(item);
                        }
                    }
                }
            }

            return directors;
        }

        // To get actors by movieID
        public async Task<List<Actor>> GetActorsByMovieIdAsync(int movieId)
        {
            var actors = new List<Actor>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "SELECT ActorID, MovieID, FisrtName, LastName, FullName, Description, NumberOfMovies, Photo FROM [MoviesApp].[ActorsWithMovies] WHERE MovieID = @movieId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@movieId", movieId);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var imageData = (byte[])reader.GetValue(7);
                            var base64Image = Convert.ToBase64String(imageData);
                            var imageUrl = $"data:image/png;base64,{base64Image}";

                            var item = new Actor
                            {
                                ActorId = reader.GetInt32(0),
                                FirstName = reader.GetString(2),
                                LastName = reader.GetString(3),
                                FullName = reader.GetString(4),
                                Description = reader.GetString(5),
                                NumberOfMovies = reader.GetInt32(6),
                                Photo = imageUrl
                            };

                            actors.Add(item);
                        }
                    }
                }
            }

            return actors;
        }

        // To get ratings by movieID
        public async Task<List<Rating>> GetRatingsByMovieIdAsync(int movieId)
        {
            var ratings = new List<Rating>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "SELECT MovieID, UserID, Rating, Comment, DateTimeStamp, Likes, DisLikes, FullName, Photo, RatingID FROM [MoviesApp].[RatingsView] WHERE MovieID = @movieId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@movieId", movieId);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var imageData = (byte[])reader.GetValue(8);
                            var base64Image = Convert.ToBase64String(imageData);
                            var imageUrl = $"data:image/png;base64,{base64Image}";

                            var item = new Rating
                            {
                                MovieId = reader.GetInt32(0),
                                UserId = reader.GetInt32(1),
                                RatingValue = reader.GetInt32(2),
                                Comment = reader.GetString(3),
                                DateTimeStamp = reader.GetSqlDateTime(4),
                                Likes = reader.GetInt32(5),
                                DisLikes = reader.GetInt32(6),
                                FullName = reader.GetString(7),
                                Photo = imageUrl,
                                RatingId = reader.GetInt32(9)
                            };

                            ratings.Add(item);
                        }
                    }
                }
            }

            return ratings;
        }

        // To get movie by movieID
        public async Task<Movie> GetMovieByIdAsync(int movieId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "SELECT MovieID, Title, Picture, Description, OverallRating, RatingsCount, Year FROM [MoviesApp].[MoviesWithRatings] WHERE MovieID = @movieId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@movieId", movieId);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
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

                            return item;
                        }
                    }
                }
            }

            return null;
        }
    }
}