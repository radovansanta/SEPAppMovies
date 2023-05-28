using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using AppMovies.Models;
using AppMovies.Repositories.Interfaces;

namespace AppMovies.Repositories
{
    public class ActorRepository : IActorsRepository
    {
        
        private readonly string _connectionString;
        
        public ActorRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        //To get all the actors
        public async Task<List<Actor>> GetActorsAsync()
        {
            var actors = new List<Actor>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "SELECT ActorID, FisrtName, LastName, FullName, Description, NumberOfMovies, Photo FROM [MoviesApp].[ActorsView]";
                using (var command = new SqlCommand(query, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var imageData = (byte[])reader.GetValue(6);
                        var base64Image = Convert.ToBase64String(imageData);
                        var imageUrl = $"data:image/png;base64,{base64Image}";

                        var item = new Actor
                        {
                            ActorId = reader.GetInt32(0),
                            FirstName = reader.GetString(1),
                            LastName = reader.GetString(2),
                            FullName = reader.GetString(3),
                            Description = reader.GetString(4),
                            NumberOfMovies = reader.GetInt32(5),
                            Photo = imageUrl
                        };

                        actors.Add(item);
                    }
                }
            }

            return actors;
        }

        //To get all the actors using search
        public async Task<List<Actor>> GetActorsBySearchAsync(string searchTerm)
        {
            var actors = new List<Actor>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "SELECT ActorID, FisrtName, LastName, FullName, Description, NumberOfMovies, Photo FROM [MoviesApp].[ActorsView] WHERE FullName LIKE @searchTerm";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@searchTerm", $"%{searchTerm}%");
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var imageData = (byte[])reader.GetValue(6);
                            var base64Image = Convert.ToBase64String(imageData);
                            var imageUrl = $"data:image/png;base64,{base64Image}";

                            var item = new Actor
                            {
                                ActorId = reader.GetInt32(0),
                                FirstName = reader.GetString(1),
                                LastName = reader.GetString(2),
                                FullName = reader.GetString(3),
                                Description = reader.GetString(4),
                                NumberOfMovies = reader.GetInt32(5),
                                Photo = imageUrl
                            };

                            actors.Add(item);
                        }
                    }
                }
            }

            return actors;
        }

        //To get all movies by actor id
        public async Task<List<Movie>> GetMoviesByActorIdAsync(int actorId)
        {
            var movies = new List<Movie>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "SELECT MovieID, Title, Picture, Year, Description, OverallRating FROM [MoviesApp].[MoviesWithActors] WHERE ActorID = @actorId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@actorId", actorId);
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

        //To get the actor by actor id
        public async Task<Actor> GetActorByIdAsync(int actorId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "SELECT ActorID, FisrtName, LastName, FullName, Description, NumberOfMovies, Photo FROM [MoviesApp].[ActorsView] WHERE ActorID = @actorId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@actorId", actorId);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            var imageData = (byte[])reader.GetValue(6);
                            var base64Image = Convert.ToBase64String(imageData);
                            var imageUrl = $"data:image/png;base64,{base64Image}";

                            var item = new Actor
                            {
                                ActorId = reader.GetInt32(0),
                                FirstName = reader.GetString(1),
                                LastName = reader.GetString(2),
                                FullName = reader.GetString(3),
                                Description = reader.GetString(4),
                                NumberOfMovies = reader.GetInt32(5),
                                Photo = imageUrl
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