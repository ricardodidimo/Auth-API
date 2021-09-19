using System.Collections.Generic;
using System.Threading.Tasks;
using api.Models.Inputs;
using api.Models.Views;

namespace api.Services
{
    public interface IUserService
    {
        Task<List<UserViewModel>> GetUsersAsync();

        /// <summary>Recover the ID from the HTTP context and requires the proprietary User.</summary>
        /// <returns>The authenticated User.</returns>
        Task<UserViewModel> GetActualUserAsync();

        /// <summary>Add User entity.</summary>
        /// <remarks>Uses normalized username for case insensitive registration and the .NET PasswordHasher for safe storing users passwords</remarks>
        /// <returns>The newly added User.</returns>
        Task<UserViewModel> AddUserAsync(UserInputModel userInput);

        /// <summary>Recover User entity which matches the given username and compare hashes for password validation.</summary>
        /// <returns>The json web token containing ID and Username of the successfully authenticated user.</returns>
        Task<string> AuthenticateUserAsync(UserInputModel userInput);

        /// <summary>Updates the values passed to it in the authenticated User</summary>
        /// <remarks>Accepts null parameters as way of communicating maintainability, 
        /// changing only wanted values whitin the authenticated User, identified by the ID captured  
        /// from HTTP context. </remarks>
        /// <returns>The updated User.</returns>
        #nullable enable
        Task<UserViewModel> UpdateUserAsync(string? username, string? password);
        #nullable disable

        /// <summary>Removes the authenticated User, identified by the ID captured from HTTP context</summary>
        /// <returns>The deleted User.</returns>
        Task<UserViewModel> RemoveUserAsync();
       
    }
}