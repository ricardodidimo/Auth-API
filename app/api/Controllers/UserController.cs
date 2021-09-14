using api.Models.Inputs;
using api.Models.Responses;
using api.Models.Views;
using api.Services;
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
        
        [HttpPost("Register")]
        public ActionResult PostAUser(UserInputModel userInput)
        {
            return Ok(new APIResponse<UserViewModel>()
            {
                StatusCode = 200,
                Message = "Success, returning recent added user",
                Data = _userService.AddUser(userInput)
            });
        }

        [HttpPost("Login")]
        public ActionResult AuthenticateUser(UserInputModel userInput)
        {
            return Ok(new APIResponse<string>()
            {
                StatusCode = 200,
                Message = "Success, deliverying your authentication token",
                Data = _userService.AuthenticateUser(userInput)
            });
        }
    }
}