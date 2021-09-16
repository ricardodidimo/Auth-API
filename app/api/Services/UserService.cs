using System;
using System.Collections.Generic;
using System.Security.Claims;
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

        public List<UserViewModel> GetUsers()
        {
            List<User> retrivedUsers = _userRepository.SelectUsers();
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

        public UserViewModel GetActualUser()
        {
            int currentUserId = Int32.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            User actual = _userRepository.SelectUserById(currentUserId);
            if(actual is null)
            {
                throw new DomainException(404, new string[]{"ID not accepted. Try again"});
            }

            return new UserViewModel(){
                UserId = actual.UserId,
                username = actual.username
            };
        }

        public UserViewModel AddUser(UserInputModel userInput)
        {
            User user = _userRepository.SelectUserByName(userInput.username.ToUpper());
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

            user = _userRepository.InsertUser(user);

            return new UserViewModel(){
                UserId = user.UserId,
                username = user.username
            };
        }

        public string AuthenticateUser(UserInputModel userInput)
        {
            string normalizedUsernameInput = userInput.username.ToUpper();
            User userExists = _userRepository.SelectUserByName(normalizedUsernameInput);
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
        public UserViewModel UpdateUser(string? username, string? password)
        {
            int currentUserId = Int32.Parse(_httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            
            User userUpdate = _userRepository.SelectUserById(currentUserId);
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
            
            userUpdate = _userRepository.UpdateUser(userUpdate);
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
    
        public UserViewModel RemoveUser()
        {
            int currentUserId = Int32.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            User toRemove = _userRepository.SelectUserById(currentUserId);

            if(toRemove is null)
            {
                throw new DomainException(400, new string[]{"ID not accepted. Try again"});
            }

            _ = _userRepository.DeleteUser(toRemove);

            return new UserViewModel()
            {
                UserId = toRemove.UserId,
                username = toRemove.username
            };
        
        }


    }
}