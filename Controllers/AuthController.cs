using englishCardsAPI.DTOs;
using englishCardsAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace englishCardsAPI.Controllers
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
        public async Task<IActionResult> Register([FromBody] RegisterUserDto registerUserDto)
        {
            var result = await _authService.RegisterAsync(registerUserDto);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Message);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto loginUserDto)
        {
            var result = await _authService.LoginAsync(loginUserDto);

            if (!result.Success)
            {
                return Unauthorized(result.Message);
            }

            return Ok(new
            {
                token = result.Token,
                expiration = result.Expiration
            });
        }

        [HttpDelete("delete")]
        [Authorize]
        public async Task<IActionResult> DeleteUser()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return Unauthorized("Invalid token data.");
            }

            var result = await _authService.DeleteUserAsync(userId);
            if (!result)
            {
                return BadRequest("Failed to delete the user.");
            }

            return Ok("User deleted successfully.");    
        }
    }
}
