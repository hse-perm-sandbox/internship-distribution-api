using InternshipDistribution.InputModels;
using InternshipDistribution.Models;
using InternshipDistribution.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;

namespace InternshipDistribution.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "RequireManager")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpPut("{userId}/role")]
        public async Task<IActionResult> UpdateUserRole(int userId, bool isManager)
        {
            var result = await _userService.UpdateUserRoleAsync(userId, isManager);
            if (!result)
                return NotFound("Пользователь не найден");

            return Ok($"Поле isManager = {isManager} у пользователя User c Id = {userId}");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var isDeleted = await _userService.DeleteUserAsync(id);
            if (!isDeleted)
                return NotFound();

            return NoContent();
        }
    }
}
