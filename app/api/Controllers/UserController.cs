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
                statusCode = 200,
                message = "Success, returning recent added user",
                data = _userService.AddUser(userInput)
            });
        }
    }
}