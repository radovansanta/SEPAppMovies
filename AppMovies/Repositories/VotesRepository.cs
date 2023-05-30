using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using AppMovies.Models;
using AppMovies.Repositories.Interfaces;

namespace AppMovies.Repositories
{
    public class VotesRepository : IVotesRepository
    {
        private readonly string _connectionString;

        public VotesRepository(string connectionString)
        {
            _connectionString = connectionString;
        }


        public async Task<int> HasUserLikedRatingAsync(int userId, int ratingId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "SELECT CASE WHEN COUNT(*) > 0 THEN 1 ELSE 0 END AS HasVote FROM [MoviesApp].[Votes] WHERE RatingID = @ratingId AND UserID = @userId;";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@userId", userId);
                    command.Parameters.AddWithValue("@ratingId", ratingId);
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

        public async Task UpdateVoteAsync(int userId, int ratingId, int ratingValue)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                Console.WriteLine("REPOSITORY");
                Console.WriteLine(userId);
                Console.WriteLine(ratingId);
                Console.WriteLine(ratingValue);
                await connection.OpenAsync();

                var query = "UPDATE [MoviesApp].[Votes] SET Vote = @ratingValue, DateTimeStamp = @dateTimeStamp WHERE RatingID = @ratingId AND UserID = @userId";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ratingValue", ratingValue);
                    command.Parameters.AddWithValue("@dateTimeStamp", DateTime.Now);
                    command.Parameters.AddWithValue("@ratingId", ratingId);
                    command.Parameters.AddWithValue("@userId", userId);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task DeleteVoteAsync(int userId, int ratingId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "DELETE FROM [MoviesApp].[Votes] WHERE RatingID = @ratingId AND UserID = @userId;";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@userId", userId);
                    command.Parameters.AddWithValue("@ratingId", ratingId);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task InsertVoteAsync(int userId, int ratingId, int ratingValue)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "INSERT INTO [MoviesApp].[Votes] (RatingID, UserID, Vote, DateTimeStamp) VALUES (@ratingId, @userId, @ratingValue, @dateTimeStamp);";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ratingId", ratingId);
                    command.Parameters.AddWithValue("@userId", userId);
                    command.Parameters.AddWithValue("@ratingValue", ratingValue);
                    command.Parameters.AddWithValue("@dateTimeStamp", DateTime.Now);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<List<Vote>> GetVotesByMovieIdAndUserIdAsync(int userId, int movieId)
        {
            var votes = new List<Vote>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "SELECT VoteID, RatingID, UserID, Vote FROM [MoviesApp].[VotesWithMovies] WHERE UserID = @userId AND MovieID = @movieId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@userId", userId);
                    command.Parameters.AddWithValue("@movieId", movieId);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var item = new Vote()
                            {
                                VoteId = reader.GetInt32(0),
                                RatingId = reader.GetInt32(1),
                                UserId = reader.GetInt32(2),
                                VoteValue = reader.GetInt32(3)
                            };

                            votes.Add(item);
                        }
                    }
                }
            }

            return votes;
        }
    }
}