using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using api.Models.Entities;
using api.Models.Inputs;
using api.Models.Responses;
using api.Models.Views;
using api.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

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


        public UserViewModel AddUser(UserInputModel userInput)
        {
            bool alreadyAdded = CheckIfUserExistsUnderUsername(userInput.username);
            if(alreadyAdded is true)
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


        public string AuthenticateUser(UserInputModel userInput)
        {
            string normalizedUsernameInput = userInput.username.ToUpper();
            User userExists = _userRepository.SelectUserByName(normalizedUsernameInput);
            if(userExists is null)
            {
                throw new DomainException(404, new string[]{"User not found"});
            }

            PasswordVerificationResult samePassword = passwordHasher.VerifyHashedPassword(userExists, 
                userExists.password, userInput.password);
            
            if(((int) samePassword) is not 1)
            {
                throw new DomainException(404, new string[]{"Credentials invalid"});
            }

            return CreateJWT(userExists);
        }

        public UserViewModel RemoveUser()
        {
            int currentUserId = Int32.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            User toRemove = _userRepository.SelectUserById(currentUserId);

            if(toRemove is null)
            {
                throw new DomainException(404, new string[]{"User not found"});
            }

            _ = _userRepository.DeleteUser(toRemove);

            return new UserViewModel()
            {
                UserId = toRemove.UserId,
                username = toRemove.username
            };
        
        }
        private User BuildFormattedUser(UserInputModel userInput)
        {

            User userToAdd = new User()
            {
                username = userInput.username,
                normalized_username = userInput.username.ToUpper()
            };

            userToAdd.password = passwordHasher.HashPassword(userToAdd, userInput.password);
            
            return userToAdd;
        }

        private bool CheckIfUserExistsUnderUsername(string username)
        {
            User foundUser = _userRepository.SelectUserByName(username.ToUpper());
            
            return foundUser is not null;
        }

        private string CreateJWT(User userLogged)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JWTKey"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userLogged.username),
                    new Claim(ClaimTypes.NameIdentifier, userLogged.UserId.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}