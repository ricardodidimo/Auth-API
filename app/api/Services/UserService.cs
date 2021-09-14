using api.Models.Entities;
using api.Models.Inputs;
using api.Models.Views;
using api.Repositories;
using Microsoft.AspNetCore.Identity;

namespace api.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public UserViewModel AddUser(UserInputModel userInput)
        {
            PasswordHasher<User> ph = new();

            User userToAdd = new User()
            {
                username = userInput.username,
                normalized_username = userInput.username.ToUpper()
            };
            userToAdd.password = ph.HashPassword(userToAdd, userInput.password);

            User insertedUser = _userRepository.InsertUser(userToAdd);

            return new UserViewModel(){
                UserId = insertedUser.UserId,
                username = insertedUser.username
            };
        }
    }
}