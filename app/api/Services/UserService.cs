using System;
using api.Models.Entities;
using api.Models.Inputs;
using api.Models.Responses;
using api.Models.Views;
using api.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
            bool alreadyAdded = CheckIfUserAlreadyExists(userInput.username);
            if(alreadyAdded)
            {
                throw new DomainException(400, new string[]{"Username already in use"});
            }

            User userToAdd = BuildFormattedUser(userInput);
            User insertedUser;
    
            insertedUser = _userRepository.InsertUser(userToAdd);

            return new UserViewModel(){
                UserId = insertedUser.UserId,
                username = insertedUser.username
            };
        }

        private User BuildFormattedUser(UserInputModel userInput)
        {
            PasswordHasher<User> ph = new();

            User userToAdd = new User()
            {
                username = userInput.username,
                normalized_username = userInput.username.ToUpper()
            };
            userToAdd.password = ph.HashPassword(userToAdd, userInput.password);
            
            return userToAdd;
        }

        private bool CheckIfUserAlreadyExists(string username)
        {
            User foundUser = _userRepository.SelectUserByName(username.ToUpper());
            
            return foundUser is not null;
        }
    }
}