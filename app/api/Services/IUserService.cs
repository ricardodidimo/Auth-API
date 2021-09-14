using api.Models.Inputs;
using api.Models.Views;

namespace api.Services
{
    public interface IUserService
    {
        UserViewModel AddUser(UserInputModel userInput);
        string AuthenticateUser(UserInputModel userInput);
    }
}