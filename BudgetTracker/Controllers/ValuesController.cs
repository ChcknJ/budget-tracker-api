using BudgetTracker.DTO.Request;
using BudgetTracker.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BudgetTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;


        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            bool success = await _authService.RegisterAsync(request);

            if (!success)
            {
                return Conflict(new
                {
                    message = "Username already exists."
                });
            }

            return Created();
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            bool success = await _authService.LoginAsync(request);

            if (!success)
            {
                return Unauthorized(new
                {
                    message = "Invalid username or password."
                });
            }

            return Ok(new
            {
                message = "Login successful!"
            });
        }
    }
}
