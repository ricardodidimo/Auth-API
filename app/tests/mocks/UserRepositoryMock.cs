using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models.Entities;
using api.Repositories;

namespace tests.Mocks
{
    public class UserRepositoryMock : IUserRepository
    {

        private List<User> userDb = new()
        {
            new User(){ UserId = 1, username = "Soraya", normalized_username = "SORAYA"},
            new User(){ UserId = 2, username = "Allex", normalized_username = "ALLEX"},
            new User(){ UserId = 3, username = "shawn", normalized_username = "SHAWN", password = "123shawn@"},
            new User(){ UserId = 4, username = "louis", normalized_username = "LOUIS"}
        };
        public Task<User> DeleteUserAsync(User toDelete)
        {
            userDb.Remove(toDelete);
            return Task.FromResult(toDelete);
        }

        public Task<User> InsertUserAsync(User userInput)
        {
            userDb.Add(userInput);
            return Task.FromResult(userInput);
        }

        public Task<User> SelectUserByIdAsync(int id)
        {
            return Task.FromResult(userDb.Where(u => u.UserId == id).FirstOrDefault());
        }

        public Task<User> SelectUserByNameAsync(string username)
        {
            return Task.FromResult(userDb.Where(u => u.normalized_username.Equals(username.ToUpper())).FirstOrDefault());
        }

        public Task<List<User>> SelectUsersAsync()
        {
            return Task.FromResult(userDb);
        }

        public Task<User> UpdateUserAsync(User userInput)
        {
            User toUpdate = userDb.Find(u => u.UserId == userInput.UserId);
            toUpdate.username = userInput.username;
            toUpdate.normalized_username = userInput.normalized_username;
            toUpdate.password = userInput.password;
            return Task.FromResult(userInput);
        }
    }
}