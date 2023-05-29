using System;
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
    }
}