using authenAutherApp.Dtos.Request;
using authenAutherApp.Services.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace authenAutherApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] SignUpRequest request)
        {
            var response = await _userService.SignupAsync(request);
            if (response.UserId != null)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [Authorize]
        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _userService.GetAllUsers();
            return Ok(response);
        }

        // [HttpPost("signin")]
        // public async Task<IActionResult> Signin([FromBody] SignInRequest request)
        // {
        //     var response = await _userService.SiginAsync(request);
        //     if (response.IsSuccess)
        //     {
        //         return Ok(response);
        //     }
        //     return Unauthorized(response);
        // }
    }
}