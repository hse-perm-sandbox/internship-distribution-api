using InternshipDistribution.InputModels;
using InternshipDistribution.Models;
using InternshipDistribution.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternshipDistribution.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterInput registerDto)
        {
            var result = await _authService.Register(registerDto);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginInput loginDto)
        {
            var token = await _authService.Login(loginDto);
            return Ok(new { Token = token });
        }

        [HttpGet("users")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            var users = await _authService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpPut("users/{userId}/role")]
        [Authorize]
        public async Task<IActionResult> UpdateUserRole(int userId, bool isManager)
        {
            var result = await _authService.UpdateUserRoleAsync(userId, isManager);
            if (!result)
                return NotFound("Пользователь не найден");

            return Ok($"Поле isManager = {isManager} у пользователя User c Id = {userId}");
        }
    }
}
