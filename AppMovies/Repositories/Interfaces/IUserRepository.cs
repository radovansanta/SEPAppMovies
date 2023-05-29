using System.Threading.Tasks;
using AppMovies.Models;

namespace AppMovies.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<int> AuthenticateUser(string email, string password);
        Task<User> GetUserByIdAsync(int userId);
        Task InsertUserAsync(string firstName, string lastName, string email, string password, byte[] profilePhoto);
    }
}