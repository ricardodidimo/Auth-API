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
            .SetBasePath("")
            // File containing variables
            .AddJsonFile("")
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
    

        [Fact]
        public void UpdateUser_WhenCalledWithValidValues_ReturnsUpdatedUser()
        {
            var userRepo = new UserRepositoryMock();
            HttpContextAccessorMock httpContext = new(1);
            var userService = new UserService(userRepo, configuration, httpContext);


            var userUpdated = userService.UpdateUser("Sarah", "123p@ssword#");

            Assert.Equal(1, userUpdated.UserId);
            Assert.Equal("Sarah", userUpdated.username);
            Assert.Equal("Sarah", userService.GetUsers().First().username);
        }


        [Fact]
        public void UpdateUser_WhenCalledWithInvalidId_ThrowsException()
        {
            var userRepo = new UserRepositoryMock();
            HttpContextAccessorMock httpContext = new(13);
            var userService = new UserService(userRepo, configuration, httpContext);

            try
            {
                var userUpdated = userService.UpdateUser("Sarah", "123p@ssword#");
            }
            catch(DomainException e)
            {  
                
                Assert.Equal("ID not accepted. Repeat authentication", e.ErrorMessages.First());
                return;
            }
            
            throw new Xunit.Sdk.XunitException("UpdateUser_WhenCalledWithInvalidId_ThrowsException -> Test failed");
        }
    

        [Fact]
        public void UpdateUser_WhenCalledWithOnlyUsernameParameter_ReturnsUpdatedUsernameAndSamePassword()
        {
            var userRepo = new UserRepositoryMock();
            HttpContextAccessorMock httpContext = new(3);
            var userService = new UserService(userRepo, configuration, httpContext);

            var userUpdated = userService.UpdateUser("Sarah", null);

            Assert.Equal(3, userUpdated.UserId);
            Assert.Equal("Sarah", userRepo.SelectUserById(3).username);
            Assert.Equal("123shawn@", userRepo.SelectUserById(3).password);
        }
        

        [Fact]
        public void UpdateUser_WhenCalledWithOnlyPasswordParameter_ReturnsUpdatedPasswordAndSameUsername()
        {
            var userRepo = new UserRepositoryMock();
            HttpContextAccessorMock httpContext = new(3);
            var userService = new UserService(userRepo, configuration, httpContext);

            var userUpdated = userService.UpdateUser(null, "123p@!@#word");

            Assert.Equal(3, userUpdated.UserId);
            Assert.Equal("shawn", userRepo.SelectUserById(3).username);
            // Password has change
            Assert.False("123shawn@".Equals(userRepo.SelectUserById(3).password));
        }


        [Fact]
        public void UpdateUser_WhenCalledWithInvalidUsername_ThrowsException()
        {
            var userRepo = new UserRepositoryMock();
            HttpContextAccessorMock httpContext = new(3);
            var userService = new UserService(userRepo, configuration, httpContext);

            try
            {
                // Breaking min. length validation rule
                var userUpdated = userService.UpdateUser("a", null);
            }
            catch(DomainException e)
            {  
                Assert.Equal("username must have min. length of 2 and a max of 25 chars", e.ErrorMessages.First());
                return;
            }
            
            throw new Xunit.Sdk.XunitException("UpdateUser_WhenCalledWithInvalidUsername_ThrowsException -> Test failed");

        }


        [Fact]
        public void UpdateUser_WhenCalledWithInvalidPassword_ThrowsException()
        {
            var userRepo = new UserRepositoryMock();
            HttpContextAccessorMock httpContext = new(3);
            var userService = new UserService(userRepo, configuration, httpContext);

            try
            {
                // Breaking min. length validation rule
                var userUpdated = userService.UpdateUser(null, "alph%2");
            }
            catch(DomainException e)
            {  
                Assert.Equal("password must have min. length of 8", e.ErrorMessages.First());
                return;
            }
            
            throw new Xunit.Sdk.XunitException("UpdateUser_WhenCalledWithInvalidPassword_ThrowsException -> Test failed");

        }
    
    
        [Fact]
        public void RemoveUser_WhenCalledWithValidValues_ShortenedListAndReturnsExpectedOutput()
        {
            var userRepo = new UserRepositoryMock();
            HttpContextAccessorMock httpContext = new(3);
            var userService = new UserService(userRepo, configuration, httpContext);

            var userDeleted = userService.RemoveUser();

            Assert.Equal(3, userService.GetUsers().Count);
            Assert.Null(userRepo.SelectUserById(3));

            Assert.Equal(3, userDeleted.UserId);
            Assert.Equal("shawn", userDeleted.username);
            
        }

        [Fact]
        public void RemoveUser_WhenCalledWithInvalidId_ThrowsException()
        {
            var userRepo = new UserRepositoryMock();
            HttpContextAccessorMock httpContext = new(13);
            var userService = new UserService(userRepo, configuration, httpContext);

            try
            {
                var userUpdated = userService.RemoveUser();
            }
            catch(DomainException e)
            {  
                
                Assert.Equal("ID not accepted. Try again", e.ErrorMessages.First());
                return;
            }
            
            throw new Xunit.Sdk.XunitException("RemoveUser_WhenCalledWithInvalidId_ThrowsException -> Test failed");
        }

    }
}
