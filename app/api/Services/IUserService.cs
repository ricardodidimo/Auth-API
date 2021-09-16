using System.Collections.Generic;
using api.Models.Inputs;
using api.Models.Views;

namespace api.Services
{
    public interface IUserService
    {
        List<UserViewModel> GetUsers();
        UserViewModel GetActualUser();
        UserViewModel AddUser(UserInputModel userInput);
        string AuthenticateUser(UserInputModel userInput);

        #nullable enable
        UserViewModel UpdateUser(string? username, string? password);
        #nullable disable

        UserViewModel RemoveUser();
       
    }
}