using AdWriter_Application.DTOs;
using AdWriter_Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AdWriter_AI.Controllers
{
    [ApiController]
    [Route("user")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepo;

        public UserController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMe()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr) || !Guid.TryParse(userIdStr, out var userId))
                return Unauthorized();

            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null)
                return Unauthorized();

            var result = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                RemainingGenerations = user.RemainingGenerations
            };

            return Ok(result);
        }
    }
}
