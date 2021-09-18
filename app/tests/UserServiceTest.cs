using api.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Moq;
using tests.Mocks;
using Xunit;
using api.Models.Views;
using System.Collections.Generic;
using System.Security.Claims;
using System.Linq;
using api.Models.Responses;
using api.Models.Inputs;
using System;

namespace tests
{
    public class UserServiceTest
    {
        private Mock<IHttpContextAccessor> httpContext = new Mock<IHttpContextAccessor>();
        private Mock<IConfiguration> configuration = new Mock<IConfiguration>();

        [Fact]
        public void GetUsers_WhenCalled_ReturnsExpectedListOfUserViewModel()
        {
            var userRepo = new UserRepositoryMock();
            var userService = new UserService(userRepo, configuration.Object, httpContext.Object);

            var result = userService.GetUsers();

            Assert.IsType<List<UserViewModel>>(result);
            Assert.Equal(result.Count, 4);
            Assert.Equal(1, result.First().UserId);
            Assert.Equal("Soraya", result.First().username);
        }

        [Fact]
        public void GetActualUser_WhenCalled_ReturnsExpectedUserViewModel()
        {
            var userRepo = new UserRepositoryMock();
            HttpContextAccessorMock httpContext = new(1);
            var userService = new UserService(userRepo, configuration.Object, httpContext);


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
            var userService = new UserService(userRepo, configuration.Object, httpContext);
    
            try
            {
                var result = userService.GetActualUser();
            }
            catch(DomainException e)
            {
                Assert.Equal(e.ErrorMessages.First(), "ID not accepted. Try again");
                return;
            }

            throw new Xunit.Sdk.XunitException("GetActualUser_WhenCalledWithInvalidId_ThrowsException -> Test failed");
        }
    
        [Fact]
        public void AddUser_WhenCalled_IncreasesListAndReturnsExpectedOutput()
        {
            var userRepo = new UserRepositoryMock();
            var userService = new UserService(userRepo, configuration.Object, httpContext.Object);
            UserInputModel input = new()
            {
                username = "Lola",
                password = "123lola#"
            };

            var result = userService.AddUser(input);
            var allUserAfterAddition = userService.GetUsers();

            Assert.Equal(5, allUserAfterAddition.Count);
            Assert.IsType<UserViewModel>(result);
            Assert.Equal("Lola", result.username);
            
        }

        [Fact]
        public void AddUser_WhenCalledWithAlreadyInUseUsername_ThrowsException()
        {
            var userRepo = new UserRepositoryMock();
            var userService = new UserService(userRepo, configuration.Object, httpContext.Object);
            UserInputModel input = new()
            {
                username = "Soraya",
                password = "123soraya#"
            };

            try
            {
                var result = userService.AddUser(input);
            }
            catch(DomainException e)
            {
                Assert.Equal(e.ErrorMessages.First(), "username not accepted. Try again");
                return;
            }
            
            throw new Xunit.Sdk.XunitException("AddUser_WhenCalledWithAlreadyInUseUsername_ThrowsException -> Test failed");

        }
        [Fact]
        public void AddUser_WhenCalledWithAlreadyInUseUsernameButDifferentCase_StillThrowsException()
        {
            var userRepo = new UserRepositoryMock();
            var userService = new UserService(userRepo, configuration.Object, httpContext.Object);
            UserInputModel input = new()
            {
                username = "soraya",
                password = "123soraya#"
            };

            try
            {
                var result = userService.AddUser(input);
            }
            catch(DomainException e)
            {  
                
                Assert.Equal(e.ErrorMessages.First(), "username not accepted. Try again");
                return;
            }
            
            throw new Xunit.Sdk.XunitException("AddUser_WhenCalledWithAlreadyInUseUsernameButDifferentCase_StillThrowsException -> Test failed");

        }


        
    }
}
