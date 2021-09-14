using api.Models.Entities;

namespace api.Repositories
{
    public interface IUserRepository
    {
        User InsertUser(User userInput);
    }
}