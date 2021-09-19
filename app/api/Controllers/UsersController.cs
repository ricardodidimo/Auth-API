using System.Collections.Generic;
using System.Threading.Tasks;
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
        public async Task<ActionResult> GetUsersAsync()
        {
            return Ok(new APIResponse<List<UserViewModel>>()
            {
                StatusCode = 200,
                Message = "Success, returning registered users",
                Data = await _userService.GetUsersAsync()
            });
        }

        [HttpGet("whoiam")]
        [Authorize]
        public async Task<ActionResult> GetActualUserAsync()
        {
            return Ok(new APIResponse<UserViewModel>()
            {
                StatusCode = 200,
                Message = "Success, returning authenticated user",
                Data = await _userService.GetActualUserAsync()
            });
        }
        [HttpPost("Register")]
        public async Task<ActionResult> PostAUserAsync(UserInputModel userInput)
        {
            return Ok(new APIResponse<UserViewModel>()
            {
                StatusCode = 201,
                Message = "Success, returning newly added user",
                Data = await _userService.AddUserAsync(userInput)
            });
        }

        [HttpPost("Login")]
        public async Task<ActionResult> AuthenticateUserAsync(UserInputModel userInput)
        {
            return Ok(new APIResponse<string>()
            {
                StatusCode = 201,
                Message = "Success, deliverying the authentication token created after identity confirmation",
                Data = await _userService.AuthenticateUserAsync(userInput)
            });
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult> PutUserAsync(string newUsername = null, string newPassword = null)
        {
            return Ok(new APIResponse<UserViewModel>()
            {
                StatusCode = 200,
                Message = "Success, returning updated user",
                Data = await _userService.UpdateUserAsync(newUsername, newPassword)
            });
        }

        [HttpDelete]
        [Authorize]
        public async Task<ActionResult> DeleteUserAsync()
        {
            return Ok(new APIResponse<UserViewModel>()
            {
                StatusCode = 200,
                Message = "Success, returning deleted user",
                Data = await _userService.RemoveUserAsync()
            });
        }
    }
}