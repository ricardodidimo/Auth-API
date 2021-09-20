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

        /// <summary> Get all registered users</summary>
        /// <response code="200">A list of all users</response>
        /// <response code="500">Unexpected server error</response>    
        [Produces("application/json")]
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

        /// <summary> Get your identity</summary>
        /// <response code="200">Your identity; id and username</response>
        /// <response code="500">Unexpected server error</response>  
        [Produces("application/json")]
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

        /// <summary> Create a user</summary>
        /// <response code="201">Newly created user; id and username</response>
        /// <response code="400">Input validation error</response>
        /// <response code="500">Unexpected server error</response>  
        [Produces("application/json")]
        [HttpPost("Register")]
        public async Task<ActionResult> PostAUserAsync(UserInputModel userInput)
        {
            return Created(nameof(PostAUserAsync), new APIResponse<UserViewModel>()
            {
                StatusCode = 201,
                Message = "Success, returning newly added user",
                Data = await _userService.AddUserAsync(userInput)
            });
        }

        /// <summary> Confirm your identity and generate the authentication token</summary>
        /// <response code="201">Newly created token;</response>
        /// <response code="400">Input validation error</response>
        /// <response code="500">Unexpected server error</response>  
        /// <remarks>After identity confirmation you must click in the 'Authorize' button in the top 
        /// of the page and type the follow: "bearer {token}" replacing '{token}' by the response 
        /// from here. All following requests will auto contain the token and you'll gain authorization 
        /// to protected endpoints. </remarks>
        [Produces("application/json")]
        [HttpPost("Login")]
        public async Task<ActionResult> AuthenticateUserAsync(UserInputModel userInput)
        {
            return Created(nameof(AuthenticateUserAsync), new APIResponse<string>()
            {
                StatusCode = 201,
                Message = 
                "Success, deliverying the authentication token created after identity confirmation",
                Data = await _userService.AuthenticateUserAsync(userInput)
            });
        }

        /// <summary> Update your username or password, or both</summary>
        /// <response code="200">Newly updated user; id and username</response>
        /// <response code="400">Input validation error</response>
        /// <response code="500">Unexpected server error</response>
        /// <remarks>You can pass both parameters or just one of them, which will partially 
        /// update your user. </remarks>
        [Produces("application/json")]
        [HttpPut]
        [Authorize]
        public async Task<ActionResult> PutUserAsync(
            string newUsername = null, 
            string newPassword = null)
        {
            return Ok(new APIResponse<UserViewModel>()
            {
                StatusCode = 200,
                Message = "Success, returning updated user",
                Data = await _userService.UpdateUserAsync(newUsername, newPassword)
            });
        }

        /// <summary> Delete your user</summary>
        /// <response code="200">Newly deleted user; id and username</response>
        /// <response code="400">Input validation error</response>
        /// <response code="500">Unexpected server error</response>  
        [Produces("application/json")]
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