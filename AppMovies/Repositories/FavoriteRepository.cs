using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using AppMovies.Models;
using AppMovies.Repositories.Interfaces;

namespace AppMovies.Repositories
{
    public class FavoriteRepository : IFavoriteRepository
    {
        private readonly string _connectionString;

        public FavoriteRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        
        public async Task<int> GetIsFavoriteByUserIdAndMovieAsync(int userId, int movieId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                Console.WriteLine("userId");
                Console.WriteLine(userId);
                Console.WriteLine("movieId");
                Console.WriteLine(movieId);
                await connection.OpenAsync();

                var query = "SELECT CASE WHEN EXISTS (SELECT 1 FROM [MoviesApp].[Favorites] WHERE UserID = @userId AND MovieID = @movieId) THEN 1 ELSE 0 END AS IsFavorite;";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@userId", userId);
                    command.Parameters.AddWithValue("@movieId", movieId);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return reader.GetInt32(0);
                        }
                    }
                }
            }

            return 0;
        }

        public async Task InsertFavoriteAsync(int userId, int movieId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "INSERT INTO [MoviesApp].[Favorites] (UserID, MovieID) VALUES (@userId, @movieId);";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@userId", userId);
                    command.Parameters.AddWithValue("@movieId", movieId);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task DeleteFavoriteAsync(int userId, int movieId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "DELETE FROM [MoviesApp].[Favorites] WHERE UserID = @userId AND MovieID = @movieId;";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@userId", userId);
                    command.Parameters.AddWithValue("@movieId", movieId);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<List<FavoriteWithMovie>> GetFavoritesByUserIdAsync(int userId)
        {
            var favorites = new List<FavoriteWithMovie>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "SELECT FavoritesID ,UserID ,MovieID ,Title ,Description ,Year ,Picture FROM [MoviesApp].[FavoritesView] WHERE UserID = @userId;";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@userId", userId);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var imageData = (byte[])reader.GetValue(6);
                            var base64Image = Convert.ToBase64String(imageData);
                            var imageUrl = $"data:image/png;base64,{base64Image}";

                            var item = new FavoriteWithMovie()
                            {
                                FavoritesId = reader.GetInt32(0),
                                UserId = reader.GetInt32(1),
                                MovieId = reader.GetInt32(2),
                                Title = reader.GetString(3),
                                Description = reader.GetString(4),
                                Year = reader.GetInt32(5),
                                Picture = imageUrl
                            };

                            favorites.Add(item);
                        }
                    }
                }
            }

            return favorites;
        }
        
        


    }
}