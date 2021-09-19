using api.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Moq;
using tests.Mocks;
using Xunit;
using api.Models.Views;
using System.Collections.Generic;
using System.Linq;
using api.Models.Responses;
using api.Models.Inputs;

namespace tests
{
    public class UserServiceTest
    {
        private Mock<IHttpContextAccessor> httpContext = new Mock<IHttpContextAccessor>();
        private IConfiguration configuration = new ConfigurationBuilder()
            // Path for folder containing variables
            .SetBasePath()
            // File containing variables
            .AddJsonFile()
            .Build();

        [Fact]
        public void GetUsers_WhenCalled_ReturnsExpectedListOfUserViewModel()
        {
            var userRepo = new UserRepositoryMock();
            var userService = new UserService(userRepo, configuration, httpContext.Object);

            var result = userService.GetUsers();

            Assert.IsType<List<UserViewModel>>(result);
            Assert.Equal(4, result.Count);
            Assert.Equal(1, result.First().UserId);
            Assert.Equal("Soraya", result.First().username);
        }

        [Fact]
        public void GetActualUser_WhenCalledWithValidParameters_ReturnsExpectedUserViewModel()
        {
            var userRepo = new UserRepositoryMock();
            HttpContextAccessorMock httpContext = new(1);
            var userService = new UserService(userRepo, configuration, httpContext);


            var result = userService.GetActualUser();

            Assert.IsType<UserViewModel>(result);
            Assert.Equal(1, result.UserId);
            Assert.Equal("Soraya", result.username);
        }

        [Fact]
        public void GetActualUser_WhenCalledWithInvalidId_ThrowsException()
        {
            var userRepo = new UserRepositoryMock();
            HttpContextAccessorMock httpContext = new(13);
            var userService = new UserService(userRepo, configuration, httpContext);
    
            try
            {
                var result = userService.GetActualUser();
            }
            catch(DomainException e)
            {
                Assert.Equal("ID not accepted. Try again", e.ErrorMessages.First());
                return;
            }

            throw new Xunit.Sdk.XunitException("GetActualUser_WhenCalledWithInvalidId_ThrowsException -> Test failed");
        }
    
        [Theory]
        [InlineData("Lola")]
        [InlineData("Ander")]
        [InlineData("May")]
        public void AddUser_WhenCalledWithValidParameters_IncreasesListAndReturnsExpectedOutput(string username)
        {
            var userRepo = new UserRepositoryMock();
            var userService = new UserService(userRepo, configuration, httpContext.Object);
            UserInputModel input = new()
            {
                username = username,
                password = "123p@ssword#"
            };

            var result = userService.AddUser(input);
            var allUserAfterAddition = userService.GetUsers();

            Assert.Equal(5, allUserAfterAddition.Count);
            Assert.IsType<UserViewModel>(result);
            Assert.Equal(username, result.username);
            
        }

        [Theory]
        [InlineData("Soraya")]
        [InlineData("Allex")]
        [InlineData("shawn")]
        [InlineData("louis")]
        public void AddUser_WhenCalledWithAlreadyInUseUsername_ThrowsException(string username)
        {
            var userRepo = new UserRepositoryMock();
            var userService = new UserService(userRepo, configuration, httpContext.Object);
            UserInputModel input = new()
            {
                username = username,
                password = "123p@ssword#"
            };

            try
            {
                var result = userService.AddUser(input);
            }
            catch(DomainException e)
            {
                Assert.Equal("username not accepted. Try again", e.ErrorMessages.First());
                return;
            }
            
            throw new Xunit.Sdk.XunitException("AddUser_WhenCalledWithAlreadyInUseUsername_ThrowsException -> Test failed");

        }

        [Theory]
        [InlineData("soraya")]
        [InlineData("allex")]
        [InlineData("SHawn")]
        [InlineData("LOUis")]
        public void AddUser_WhenCalledWithAlreadyInUseUsernameButDifferentCase_StillThrowsException(string username)
        {
            var userRepo = new UserRepositoryMock();
            var userService = new UserService(userRepo, configuration, httpContext.Object);
            UserInputModel input = new()
            {
                username = username,
                password = "123p@ssword#"
            };

            try
            {
                var result = userService.AddUser(input);
            }
            catch(DomainException e)
            {  
                
                Assert.Equal("username not accepted. Try again", e.ErrorMessages.First());
                return;
            }
            
            throw new Xunit.Sdk.XunitException("AddUser_WhenCalledWithAlreadyInUseUsernameButDifferentCase_StillThrowsException -> Test failed");

        }

        [Fact]
        public void AuthenticateUser_WhenCalledWithValidParameters_ReturnsAuthenticationToken()
        {
            var userRepo = new UserRepositoryMock();
            var userService = new UserService(userRepo, configuration, httpContext.Object);
            UserInputModel input = new()
            {
                username = "Mike",
                password = "123p@ssword#"
            };
            var user = userService.AddUser(input);

            var token = userService.AuthenticateUser(input);

            Assert.IsType<string>(token);
            string expectedBeginningForJWT = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9";
            Assert.StartsWith(expectedBeginningForJWT, token);
        }

        [Fact]
        public void AuthenticateUser_WhenCalledWithInvalidUsername_ThrowsException()
        {
            var userRepo = new UserRepositoryMock();
            var userService = new UserService(userRepo, configuration, httpContext.Object);
            UserInputModel input = new()
            {
                username = "Austin",
                password = "123p@ssword#"
            };
            _ = userService.AddUser(input);
            // Corrupting username input
            input.username = "Darwin";

            try
            {
                var result = userService.AuthenticateUser(input);
            }
            catch(DomainException e)
            {  
                
                Assert.Equal("Credentials invalid", e.ErrorMessages.First());
                return;
            }
            
            throw new Xunit.Sdk.XunitException("AuthenticateUser_WhenCalledWithInvalidUsername_ThrowsException -> Test failed");

        }
        [Fact]
        public void AuthenticateUser_WhenCalledWithInvalidPassword_ThrowsException()
        {
            var userRepo = new UserRepositoryMock();
            var userService = new UserService(userRepo, configuration, httpContext.Object);
            UserInputModel input = new()
            {
                username = "Mike",
                password = "123p@ssword#"
            };
            _ = userService.AddUser(input);
            // Corrupting password input
            input.password = "123p@lol#";

            try
            {
                var result = userService.AuthenticateUser(input);
            }
            catch(DomainException e)
            {  
                
                Assert.Equal("Credentials invalid", e.ErrorMessages.First());
                return;
            }
            
            throw new Xunit.Sdk.XunitException("AuthenticateUser_WhenCalledWithInvalidPassword_ThrowsException -> Test failed");

        }
    }
}
