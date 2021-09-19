using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using api.Helpers;
using api.Models.Entities;
using api.Models.Inputs;
using api.Models.Responses;
using api.Models.Validators;
using api.Models.Views;
using api.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace api.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly PasswordHasher<User> passwordHasher = new();


        public UserService(IUserRepository userRepository, 
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<UserViewModel>> GetUsersAsync()
        {
            List<User> retrivedUsers = await _userRepository.SelectUsersAsync();
            List<UserViewModel> users = new();

            foreach (User user in retrivedUsers)
            {
                users.Add(new UserViewModel(){
                    UserId = user.UserId,
                    username = user.username
                });
            }

            return users;
        }

        public async Task<UserViewModel> GetActualUserAsync()
        {
            int currentUserId = Int32.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            User actual = await _userRepository.SelectUserByIdAsync(currentUserId);
            if(actual is null)
            {
                throw new DomainException(404, new string[]{"ID not accepted. Try again"});
            }

            return new UserViewModel(){
                UserId = actual.UserId,
                username = actual.username
            };
        }

        public async Task<UserViewModel> AddUserAsync(UserInputModel userInput)
        {
            User user = await _userRepository.SelectUserByNameAsync(userInput.username.ToUpper());
            if(user is not null)
            {
                throw new DomainException(400, new string[]{"username not accepted. Try again"});
            }

            user = new User()
            {
                username = userInput.username,
                normalized_username = userInput.username.ToUpper(),
                password = passwordHasher.HashPassword(user, userInput.password)
            };

            user = await _userRepository.InsertUserAsync(user);

            return new UserViewModel(){
                UserId = user.UserId,
                username = user.username
            };
        }

        public async Task<string> AuthenticateUserAsync(UserInputModel userInput)
        {
            string normalizedUsernameInput = userInput.username.ToUpper();
            User userExists = await _userRepository.SelectUserByNameAsync(normalizedUsernameInput);
            if(userExists is null)
            {
                throw new DomainException(400, new string[]{"Credentials invalid"});
            }

            PasswordVerificationResult samePassword = passwordHasher.VerifyHashedPassword(userExists, 
                userExists.password, userInput.password);
            
            if(((int) samePassword) is not 1)
            {
                throw new DomainException(400, new string[]{"Credentials invalid"});
            }

            return JSONWebTokenManager.CreateJWT(userExists, _configuration);
        }

    #nullable enable
        public async Task<UserViewModel> UpdateUserAsync(string? username, string? password)
        {
            int currentUserId = Int32.Parse(_httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            
            User userUpdate = await _userRepository.SelectUserByIdAsync(currentUserId);
            if(userUpdate is null)
            {
                throw new DomainException(400, new string[]{"ID not accepted. Repeat authentication"});
            }

            if(username is not null)
            {
                var validator = new UserInputModelUsernameValidator();
                
                if(IsValidInput<string>(validator, username) is true)
                {
                    userUpdate.username = username;
                    userUpdate.normalized_username = username.ToUpper();
                }
            }

            if(password is not null)
            {
                var validator = new UserInputModelPasswordValidator();

                if(IsValidInput<string>(validator, password) is true)
                {
                    userUpdate.password = passwordHasher.HashPassword(userUpdate, password);
                }
            }
            
            userUpdate = await _userRepository.UpdateUserAsync(userUpdate);
            return new UserViewModel(){
                UserId = userUpdate.UserId,
                username = userUpdate.username
            };
        }

        private bool IsValidInput<T>(AbstractValidator<T> validator, T input)
        {
            var result = validator.Validate(input);

            if(result.IsValid is false)
            {
                List<string> errors = new();

                foreach (var item in result.Errors)
                {
                    errors.Add(item.ErrorMessage);
                }
                throw new DomainException(400, errors);
            }

            return true;
        }
    #nullable disable
    
        public async Task<UserViewModel> RemoveUserAsync()
        {
            int currentUserId = Int32.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            User toRemove = await _userRepository.SelectUserByIdAsync(currentUserId);

            if(toRemove is null)
            {
                throw new DomainException(400, new string[]{"ID not accepted. Try again"});
            }

            _ = _userRepository.DeleteUserAsync(toRemove);

            return new UserViewModel()
            {
                UserId = toRemove.UserId,
                username = toRemove.username
            };
        
        }


    }
}