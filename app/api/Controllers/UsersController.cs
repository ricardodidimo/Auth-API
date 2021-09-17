using System.Collections.Generic;
using api.Models.Inputs;
using api.Models.Responses;
using api.Models.Views;
using api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public ActionResult GetUsers()
        {
            return Ok(new APIResponse<List<UserViewModel>>()
            {
                StatusCode = 200,
                Message = "Success, returning registered users",
                Data = _userService.GetUsers()
            });
        }

        [HttpGet("whoiam")]
        [Authorize]
        public ActionResult GetActualUser()
        {
            return Ok(new APIResponse<UserViewModel>()
            {
                StatusCode = 200,
                Message = "Success, returning authenticated user",
                Data = _userService.GetActualUser()
            });
        }
        [HttpPost("Register")]
        public ActionResult PostAUser(UserInputModel userInput)
        {
            return Ok(new APIResponse<UserViewModel>()
            {
                StatusCode = 201,
                Message = "Success, returning newly added user",
                Data = _userService.AddUser(userInput)
            });
        }

        [HttpPost("Login")]
        public ActionResult AuthenticateUser(UserInputModel userInput)
        {
            return Ok(new APIResponse<string>()
            {
                StatusCode = 201,
                Message = "Success, deliverying the authentication token created after identity confirmation",
                Data = _userService.AuthenticateUser(userInput)
            });
        }

        [HttpPut]
        [Authorize]
        public ActionResult PutUser(string newUsername = null, string newPassword = null)
        {
            return Ok(new APIResponse<UserViewModel>()
            {
                StatusCode = 200,
                Message = "Success, returning updated user",
                Data = _userService.UpdateUser(newUsername, newPassword)
            });
        }

        [HttpDelete]
        [Authorize]
        public ActionResult DeleteUser()
        {
            return Ok(new APIResponse<UserViewModel>()
            {
                StatusCode = 200,
                Message = "Success, returning deleted user",
                Data = _userService.RemoveUser()
            });
        }
    }
}