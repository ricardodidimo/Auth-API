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
using FluentValidation.Results;
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
        private readonly PasswordHasher<User> _passwordHasher = new();


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
                password = _passwordHasher.HashPassword(user, userInput.password)
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
            User userToAuth = await _userRepository.SelectUserByNameAsync(normalizedUsernameInput);
            if(userToAuth is null)
            {
                throw new DomainException(400, new string[]{"Credentials invalid"});
            }

            PasswordVerificationResult samePassword = _passwordHasher.VerifyHashedPassword(userToAuth, 
                userToAuth.password, userInput.password);
            
            if(((int) samePassword) is not 1)
            {
                throw new DomainException(400, new string[]{"Credentials invalid"});
            }

            return JSONWebTokenManager.CreateJWT(userToAuth, _configuration);
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
                    User alreadyInUse = await _userRepository.SelectUserByNameAsync(username);
                    if(alreadyInUse is not null)
                    {
                        throw new DomainException(400, new string[]{"Username already in use"});
                    }
                    userUpdate.username = username;
                    userUpdate.normalized_username = username.ToUpper();
                }
            }

            if(password is not null)
            {
                var validator = new UserInputModelPasswordValidator();

                if(IsValidInput<string>(validator, password) is true)
                {
                    userUpdate.password = _passwordHasher.HashPassword(userUpdate, password);
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
            ValidationResult result = validator.Validate(input);

            if(result.IsValid is false)
            {
                List<string> errors = new();

                foreach (ValidationFailure item in result.Errors)
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

            User userRemove = await _userRepository.SelectUserByIdAsync(currentUserId);

            if(userRemove is null)
            {
                throw new DomainException(400, new string[]{"ID not accepted. Try again"});
            }

            _ = await _userRepository.DeleteUserAsync(userRemove);

            return new UserViewModel()
            {
                UserId = userRemove.UserId,
                username = userRemove.username
            };
        
        }


    }
}