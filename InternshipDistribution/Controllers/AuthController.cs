using InternshipDistribution.InputModels;
using InternshipDistribution.Models;
using InternshipDistribution.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternshipDistribution.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }
        [Authorize(Policy = "RequireManager"), ]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterInput registerDto)
        {
            var result = await _authService.Register(registerDto);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginInput loginDto)
        {
            var token = await _authService.Login(loginDto);
            return Ok(new { Token = token });
        }
    }
}
