using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using AppMovies.Models;
using AppMovies.Repositories.Interfaces;

namespace AppMovies.Repositories
{
    public class DirectorRepository : IDirectorsRepository
    {
        private readonly string _connectionString;
        
        public DirectorRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // To get all directors
        public async Task<List<Director>> GetDirectorsAsync()
        {
            var movies = new List<Director>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "SELECT DirectorID, FisrtName, LastName, BornDate, DiedDate, Age, NumberOFMovies, Description, Photo FROM [MoviesApp].[DirectorsView]";
                using (var command = new SqlCommand(query, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var imageData = (byte[])reader.GetValue(8);
                        var base64Image = Convert.ToBase64String(imageData);
                        var imageUrl = $"data:image/png;base64,{base64Image}";

                        var item = new Director
                        {
                            DirectorId = reader.GetInt32(0),
                            FirstName = reader.GetString(1),
                            LastName = reader.GetString(2),
                            //BornDate = reader.GetDateTime(3),
                            //DiedDate = reader.GetDateTime(4),
                            Age = reader.GetInt32(5),
                            NumberOfMovies = reader.GetInt32(6),
                            Description = reader.GetString(7),
                            Photo = imageUrl
                        };

                        movies.Add(item);
                    }
                }
            }

            return movies;
        }

        public async Task<List<Director>> GetDirectorsBySearchAsync(string searchTerm)
        {
            var movies = new List<Director>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "SELECT DirectorID, FisrtName, LastName, BornDate, DiedDate, Age, NumberOFMovies, Description, Photo, FullName FROM [MoviesApp].[DirectorsView] WHERE FullName LIKE @searchTerm";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@searchTerm", $"%{searchTerm}%");
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var imageData = (byte[])reader.GetValue(8);
                            var base64Image = Convert.ToBase64String(imageData);
                            var imageUrl = $"data:image/png;base64,{base64Image}";

                            var item = new Director
                            {
                                DirectorId = reader.GetInt32(0),
                                FirstName = reader.GetString(1),
                                LastName = reader.GetString(2),
                                //BornDate = reader.GetDateTime(3),
                                //DiedDate = reader.GetDateTime(4),
                                Age = reader.GetInt32(5),
                                NumberOfMovies = reader.GetInt32(6),
                                Description = reader.GetString(7),
                                Photo = imageUrl
                            };

                            movies.Add(item);
                        }
                    }
                }
            }

            return movies;
        }

        public async Task<List<Movie>> GetMoviesByDirectorIdAsync(int directorId)
        {
            var movies = new List<Movie>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "SELECT MovieID, Title, Picture, Year, Description, OverallRating FROM [MoviesApp].[MoviesWithDireectors] WHERE DirectorID = @directorId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@directorId", directorId);
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
                                Year = reader.GetInt32(3),
                                Description = reader.GetString(4),
                                OverallRating = reader.GetInt32(5)
                            };

                            movies.Add(item);
                        }
                    }
                }
            }

            return movies;
        }

        public async Task<Director> GetDirectorByIdAsync(int directorId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "SELECT DirectorID, FisrtName, LastName, Age, NumberOFMovies, Description, Photo FROM [MoviesApp].[DirectorsView] WHERE DirectorID = @directorId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@directorId", directorId);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            var imageData = (byte[])reader.GetValue(6);
                            var base64Image = Convert.ToBase64String(imageData);
                            var imageUrl = $"data:image/png;base64,{base64Image}";

                            var item = new Director
                            {
                                DirectorId = reader.GetInt32(0),
                                FirstName = reader.GetString(1),
                                LastName = reader.GetString(2),
                                Age = reader.GetInt32(3),
                                NumberOfMovies = reader.GetInt32(4),
                                Description = reader.GetString(5),
                                Photo = imageUrl,
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