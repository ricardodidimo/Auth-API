using System.Collections.Generic;
using System.Threading.Tasks;
using api.Models.Inputs;
using api.Models.Views;

namespace api.Services
{
    public interface IUserService
    {
        Task<List<UserViewModel>> GetUsersAsync();
        Task<UserViewModel> GetActualUserAsync();
        Task<UserViewModel> AddUserAsync(UserInputModel userInput);
        Task<string> AuthenticateUserAsync(UserInputModel userInput);

        #nullable enable
        Task<UserViewModel> UpdateUserAsync(string? username, string? password);
        #nullable disable

        Task<UserViewModel> RemoveUserAsync();
       
    }
}