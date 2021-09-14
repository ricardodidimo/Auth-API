using System.Collections.Generic;
using api.Models.Inputs;
using api.Models.Views;

namespace api.Services
{
    public interface IUserService
    {
        List<UserViewModel> GetUsers();
        UserViewModel AddUser(UserInputModel userInput);
        string AuthenticateUser(UserInputModel userInput);
        public UserViewModel RemoveUser();
       
    }
}