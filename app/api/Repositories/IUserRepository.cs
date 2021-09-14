using System.Collections.Generic;
using api.Models.Entities;

namespace api.Repositories
{
    public interface IUserRepository
    {
        List<User> SelectUsers();
        User InsertUser(User userInput);
        User SelectUserByName(string username);
        User SelectUserById(int id);
        User DeleteUser(User toDelete);
    }
}