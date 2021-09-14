using api.Models.Inputs;
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
           return Ok(_userService.AddUser(userInput));
        }
    }
}