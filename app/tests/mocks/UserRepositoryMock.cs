using System.Collections.Generic;
using System.Linq;
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
            new User(){ UserId = 3, username = "shawn", normalized_username = "SHAWN"},
            new User(){ UserId = 4, username = "louis", normalized_username = "LOUIS"}
        };
        public User DeleteUser(User toDelete)
        {
            userDb.Remove(toDelete);
            return toDelete;
        }

        public User InsertUser(User userInput)
        {
            userDb.Add(userInput);
            return userInput;
        }

        public User SelectUserById(int id)
        {
            return userDb.Where(u => u.UserId == id).FirstOrDefault();
        }

        public User SelectUserByName(string username)
        {
            return userDb.Where(u => u.normalized_username.Equals(username.ToUpper())).FirstOrDefault();
        }

        public List<User> SelectUsers()
        {
            return userDb;
        }

        public User UpdateUser(User userInput)
        {
            User toUpdate = userDb.Find(u => u.UserId == userInput.UserId);
            toUpdate.username = userInput.username;
            toUpdate.normalized_username = userInput.normalized_username;
            toUpdate.password = userInput.password;
            return userInput;
        }
    }
}