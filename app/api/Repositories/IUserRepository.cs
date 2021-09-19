using System.Collections.Generic;
using System.Threading.Tasks;
using api.Models.Entities;

namespace api.Repositories
{
    public interface IUserRepository
    {
        Task<List<User>> SelectUsersAsync();
        Task<User> SelectUserByNameAsync(string username);
        Task<User> SelectUserByIdAsync(int id);
        Task<User> InsertUserAsync(User userInput);
        Task<User> UpdateUserAsync(User userInput);
        Task<User> DeleteUserAsync(User toDelete);
    }
}