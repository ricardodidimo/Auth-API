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
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
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

        [HttpPost("Register")]
        public ActionResult PostAUser(UserInputModel userInput)
        {
            return Ok(new APIResponse<UserViewModel>()
            {
                StatusCode = 201,
                Message = "Success, returning recent added user",
                Data = _userService.AddUser(userInput)
            });
        }

        [HttpPost("Login")]
        public ActionResult AuthenticateUser(UserInputModel userInput)
        {
            return Ok(new APIResponse<string>()
            {
                StatusCode = 201,
                Message = "Success, deliverying your authentication token",
                Data = _userService.AuthenticateUser(userInput)
            });
        }

        [HttpDelete]
        [Authorize]
        public ActionResult DeleteUser()
        {
            return Ok(new APIResponse<UserViewModel>()
            {
                StatusCode = 200,
                Message = "Success, user deleted",
                Data = _userService.RemoveUser()
            });
        }
    }
}